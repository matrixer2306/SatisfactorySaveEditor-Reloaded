using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using Windows.Devices.Power;

namespace SSE_R
{
    public class Parser
    {
        // decompresses chunks and writes the data to an output file
        public void ParseBody(string inputPath, string outputPath)
        {
            FileStream inputStream = new(inputPath, FileMode.Open);
            FileStream outputStream = File.Create(Path.Combine(outputPath, "Body.bin"));

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
                            Debug.WriteLine($"Input stream length is {inputStream.Length}\n Read {totalBytesRead} bytes from {chunkCount} chunks.");
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
                                return;
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
        
        public void ParseHeader(string inputPath, string outputPath)
        {
            using (FileStream inputStream = File.OpenRead(inputPath))
            {
                using (FileStream outputStream = File.Create(Path.Combine(outputPath, "Header.bin")))
                {
                    // set up readers for different encodings
                    BinaryReader readerUTF16 = new(inputStream, Encoding.Default);
                    BinaryReader readerUTF8 = new(inputStream, Encoding.UTF8);

                    // set up variables for reading strings
                    int nextStringLength;
                    byte[] buffer = new byte[1000000];

                    // reading of variables
                    int headerVersion = readerUTF16.ReadInt32(); 
                    int saveVersion = readerUTF16.ReadInt32();
                    int buildVersion = readerUTF16.ReadInt32();

                    string mapName = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    buffer = readerUTF16.ReadBytes(nextStringLength);
                    foreach(byte b in buffer) { mapName += (char)b; }

                    string mapOptions = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    if (nextStringLength < 0) { buffer = new byte[-nextStringLength]; int count = 0;  while (count < Math.Abs(nextStringLength)) { buffer[count] = readerUTF16.ReadByte(); readerUTF16.BaseStream.Position++; count++; } foreach (byte b in buffer) { mapOptions += (char)b; } }
                    if (nextStringLength > 0) { buffer = readerUTF8.ReadBytes(nextStringLength); foreach (byte b in buffer) { mapOptions += (char)b; }}
                    if (nextStringLength == 0) { mapOptions = ""; inputStream.Position += nextStringLength; }

                    string sessionID = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    if (nextStringLength < 0) { buffer = new byte[-nextStringLength]; int count = 0; while (count < Math.Abs(nextStringLength)) { buffer[count] = readerUTF16.ReadByte(); readerUTF16.BaseStream.Position++; count++; } foreach (byte b in buffer) { sessionID += (char)b; } }
                    if (nextStringLength > 0) { buffer = readerUTF8.ReadBytes(nextStringLength); foreach (byte b in buffer) { sessionID += (char)b; } }
                    if (nextStringLength == 0) { sessionID = ""; inputStream.Position += nextStringLength; }

                    int playedSeconds = readerUTF16.ReadInt32();
                    long tickTimeStamp = readerUTF16.ReadInt64();
                    byte sessionVisibility = readerUTF16.ReadByte();
                    int unrealVersion = readerUTF16.ReadInt32();

                    string modMetaData = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    buffer = readerUTF16.ReadBytes(nextStringLength);
                    foreach (byte b in buffer) { modMetaData += (char)b; }

                    int modFlags = readerUTF16.ReadInt32();
                    // writes all the data gathered from the chunk header to the output file
                    using (BinaryWriter writer = new(outputStream, Encoding.Default))
                    {
                        Debug.WriteLine(headerVersion);
                        Debug.WriteLine(saveVersion);
                        Debug.WriteLine(buildVersion);
                        Debug.WriteLine(mapName);
                        Debug.WriteLine(mapOptions);
                        Debug.WriteLine(sessionID);
                        Debug.WriteLine(playedSeconds);
                        Debug.WriteLine(tickTimeStamp);
                        Debug.WriteLine(sessionVisibility);
                        Debug.WriteLine(unrealVersion);
                        Debug.WriteLine(modMetaData);
                        Debug.WriteLine(modFlags);

                        writer.Write($"Header version = {headerVersion}\n");
                        writer.Write($"Save version = {saveVersion}\n");
                        writer.Write($"Build version = {buildVersion}\n");
                        writer.Write($"Map name = {mapName}\n");
                        writer.Write($"Map options string: {mapOptions}\n");
                        writer.Write($"Playtime in seconds = {playedSeconds}\n");
                        writer.Write($"Tick timestamp = {tickTimeStamp}\n");
                        writer.Write($"Session visibility = {sessionVisibility}\n");
                        writer.Write($"unrealVersion = {unrealVersion}\n");
                        writer.Write($"modMetaData: {modMetaData}\n");
                        writer.Write($"modFlags: {modFlags}\n");
                    }
                }
            }
        }
    }
}