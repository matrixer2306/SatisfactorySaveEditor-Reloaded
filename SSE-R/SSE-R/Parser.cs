using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using Windows.Devices.Power;

namespace SSE_R
{
    public class Parser
    {
        // decompresses chunks and writes the data to an output file
        public MemoryStream ParseBody(string inputPath, string outputPath)
        {
            FileStream inputStream = new(inputPath, FileMode.Open);
            //FileStream outputStream = File.Create(Path.Combine(outputPath, "Body.bin"));
            MemoryStream outputStream = new MemoryStream();

            byte[] buffer = new byte[4];
            bool foundStart = false;
            int totalBytesRead = 0;
            int chunkCount = 0;
            int compressedChunkLength = 0;
            int decompressedChunkLength = 0;
            int combinedChunkLength = 0;
            long chunkStart = 0;
            int bytesread = 0;
            long writeStart = 0;
            long writeEnd = 0;
            List<long> offsets = new List<long>();

            while (true)
            {
                Debug.WriteLine($"Starting search for C1 83 2A 9E at {inputStream.Position}");
                while (!foundStart)
                {
                    var b = inputStream.ReadByte();
                    switch (b)
                    {
                        case -1:
                            Debug.WriteLine($"Input stream length is {inputStream.Length/1000} kb or {inputStream.Length / 1024} kib \n Read {totalBytesRead/1000} kb or {totalBytesRead/1024} kib from {chunkCount} chunks.");
                            if (totalBytesRead < combinedChunkLength)
                            {
                                Debug.WriteLine($"Should have written {combinedChunkLength} bytes, was off by {combinedChunkLength - totalBytesRead}");
                            }
                            inputStream.Close();
                            try
                            {
                                throw new Exception("reached the end of the stream");
                            }
                            catch (Exception)
                            {
                                return outputStream;
                            }
                        case 0xC1:
                            b = inputStream.ReadByte();
                            if (b == 0xc1)
                            {
                                b = inputStream.ReadByte();
                            }
                            if (b == 0x83)
                            {
                                b = inputStream.ReadByte();
                                if (b == 0x2A)
                                {
                                    b = inputStream.ReadByte();
                                    if (b == 0x9E)
                                    {
                                        inputStream.Seek(12, SeekOrigin.Current);
                                        inputStream.Read(buffer, 0, 4);
                                        compressedChunkLength = BitConverter.ToInt32(buffer, 0);
                                        inputStream.Seek(4, SeekOrigin.Current);
                                        inputStream.Read(buffer, 0, 4);
                                        decompressedChunkLength = BitConverter.ToInt32(buffer, 0);
                                        inputStream.Seek(22, SeekOrigin.Current);
                                        foundStart = true;
                                        chunkCount++;
                                        combinedChunkLength += decompressedChunkLength;
                                        chunkStart = inputStream.Position;
                                        offsets.Add(chunkStart);
                                        Debug.WriteLine($"found chunk at {inputStream.Position}");
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                // intitialize deflatestream which is disposed after each iteration of the loop
                using (DeflateStream deflateStream = new(inputStream, CompressionMode.Decompress, true))
                {
                    // set position of the deflatestream and log startposition for debugging
                    deflateStream.BaseStream.Position = chunkStart;
                    long startposition = deflateStream.BaseStream.Position;
                    byte[] readData = new byte[decompressedChunkLength];
                    while (bytesread < decompressedChunkLength)
                    {
                        bytesread += deflateStream.Read(readData, bytesread, decompressedChunkLength - bytesread);
                    }
                    long endposition = deflateStream.BaseStream.Position;
                    outputStream.Seek(0, SeekOrigin.End);
                    writeStart = outputStream.Position;
                    outputStream.Write(readData, 0, decompressedChunkLength);
                    writeEnd = outputStream.Position;
                    inputStream.Position = chunkStart;
                    foundStart = false;
                    totalBytesRead += bytesread;
                    Debug.WriteLine($"Writing debug data for chunk {chunkCount}\n Started reading at offset {startposition} and stopped at offset {endposition}\n The first 4 bytes read are {BitConverter.ToString(readData, 0, 4)}\n Started write at offset {writeStart} and stopped at offset {writeEnd}\n Read {bytesread} bytes and wrote {writeEnd - writeStart} bytes\n ");
                    bytesread = 0;
                }
            }
        }
        
        public MemoryStream ParseHeader(string inputPath, string outputPath)
        {
            using (FileStream inputStream = File.OpenRead(inputPath))
            {
                using (FileStream outputStream = File.Create(Path.Combine(outputPath, "Header.bin")))
                {
                    // set up readers for different encodings
                    BinaryReader readerUTF16 = new(inputStream, Encoding.Default);
                    BinaryReader readerUTF8 = new(inputStream, Encoding.UTF8);
                    MemoryStream header = new MemoryStream();
                    int headerLength = 0;

                    // set up variables for reading strings
                    int nextStringLength;
                    byte[] buffer = new byte[1000000];

                    // reading of variables
                    int headerVersion = readerUTF16.ReadInt32();
                    headerLength += 4;
                    int saveVersion = readerUTF16.ReadInt32();
                    headerLength += 4;
                    int buildVersion = readerUTF16.ReadInt32();
                    headerLength += 4;

                    string mapName = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    buffer = readerUTF16.ReadBytes(nextStringLength);
                    foreach(byte b in buffer) { mapName += (char)b; }
                    headerLength += nextStringLength;

                    string mapOptions = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    if (nextStringLength < 0) { buffer = new byte[-nextStringLength]; int count = 0;  while (count < Math.Abs(nextStringLength)) { buffer[count] = readerUTF16.ReadByte(); readerUTF16.BaseStream.Position++; count++; } foreach (byte b in buffer) { mapOptions += (char)b; } }
                    if (nextStringLength > 0) { buffer = readerUTF8.ReadBytes(nextStringLength); foreach (byte b in buffer) { mapOptions += (char)b; }}
                    if (nextStringLength == 0) { mapOptions = ""; inputStream.Position += nextStringLength; }
                    headerLength += nextStringLength;

                    string sessionID = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    if (nextStringLength < 0) { buffer = new byte[-nextStringLength]; int count = 0; while (count < Math.Abs(nextStringLength)) { buffer[count] = readerUTF16.ReadByte(); readerUTF16.BaseStream.Position++; count++; } foreach (byte b in buffer) { sessionID += (char)b; } }
                    if (nextStringLength > 0) { buffer = readerUTF8.ReadBytes(nextStringLength); foreach (byte b in buffer) { sessionID += (char)b; } }
                    if (nextStringLength == 0) { sessionID = ""; inputStream.Position += nextStringLength; }
                    headerLength += nextStringLength;

                    int playedSeconds = readerUTF16.ReadInt32();
                    headerLength += 4;
                    long tickTimeStamp = readerUTF16.ReadInt64();
                    headerLength += 4;
                    byte sessionVisibility = readerUTF16.ReadByte();
                    headerLength += 1;
                    int unrealVersion = readerUTF16.ReadInt32();
                    headerLength += 4;

                    string modMetaData = "";
                    if (headerVersion >= 8)
                    {
                        nextStringLength = readerUTF16.ReadInt32();
                        Debug.WriteLine($"modmetadata is {nextStringLength} bytes long");
                        buffer = new byte[nextStringLength];
                        buffer = readerUTF16.ReadBytes(nextStringLength);
                        foreach (byte b in buffer)
                        {
                            modMetaData += (char)b;
                        }
                        headerLength += nextStringLength;
                    }
                    else
                    {
                        modMetaData = "Modmetadata only exists in header version 8 or above";
                    }

                    int modFlags = readerUTF16.ReadInt32();
                    headerLength += 4;
                    // writes all the data gathered from the chunk header to the output file
                    using (BinaryWriter writer = new(outputStream, Encoding.ASCII))
                    {
                        Debug.Write($"Header version = {headerVersion}\n");
                        Debug.Write($"Save version = {saveVersion}\n");
                        Debug.Write($"Build version = {buildVersion}\n");
                        Debug.Write($"Map name = {mapName}\n");
                        Debug.Write($"Map options string: {mapOptions}\n");
                        Debug.Write($"Playtime in seconds = {playedSeconds}\n");
                        Debug.Write($"Tick timestamp = {tickTimeStamp}\n");
                        Debug.Write($"Session visibility = {sessionVisibility}\n");
                        Debug.Write($"unrealVersion = {unrealVersion}\n");
                        if (modMetaData.Length != 0)
                        {
                            Debug.Write($"modMetaData: {modMetaData}\n");
                        }
                        else { Debug.Write("There is no modMetaData\n"); }
                        Debug.Write($"modFlags: {modFlags}\n");
                        
                        writer.Write($"Header version = {headerVersion}\n");
                        writer.Write($"Save version = {saveVersion}\n");
                        writer.Write($"Build version = {buildVersion}\n");
                        writer.Write($"Map name = {mapName}\n");
                        writer.Write($"Map options string: {mapOptions}\n");
                        writer.Write($"Playtime in seconds = {playedSeconds}\n");
                        writer.Write($"Tick timestamp = {tickTimeStamp}\n");
                        writer.Write($"Session visibility = {sessionVisibility}\n");
                        writer.Write($"unrealVersion = {unrealVersion}\n");
                        if (modMetaData.Length != 0)
                        {
                            writer.Write($"modMetaData: {modMetaData}\n");
                        }
                        else { writer.Write("There is no modMetaData\n"); }
                        writer.Write($"modFlags: {modFlags}\n");
                    }
                    inputStream.CopyTo(header, headerLength);
                    return header;
                }
            }
        }
    }
}