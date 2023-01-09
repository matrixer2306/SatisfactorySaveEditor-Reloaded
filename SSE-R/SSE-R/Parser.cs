using System.Diagnostics;
using System.IO.Compression;
using System.Text;

namespace SSE_R
{
    public class Parser
    {
        // decompresses chunks and writes the data to an output file
        public void ParseBody(string inputPath, string outputPath)
        {
            using (FileStream inputStream = new(inputPath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new(inputStream, Encoding.Unicode, true))
                {
                    // initialising variables
                    int count = 0;
                    long outputPosition = 0;
                    bool foundStart = false;
                    int nextChunkSize = 0;
                    int b = 0;
                    inputStream.Position = 0;
                    int outputSize = 0;
                    // loops through all the chunks, stopping when the catch is triggered
                    while (true)
                    {
                        while (!foundStart)
                        {
                            // catches System.IO.EndOfStreamException to close the stream and write debug data when the end is reached
                            try
                            {
                                b = r.ReadByte();
                            }
                            catch (EndOfStreamException)
                            {
                                Debug.WriteLine("Reached the end of the stream");
                                Debug.WriteLine(count, "count");
                                Debug.WriteLine(nextChunkSize, "last chunk size = ");
                                Debug.WriteLine(outputSize, "body.txt size in bytes should be ");
                                return;
                            }
                            // find location of the chunk header, finds the length of the chunk and moves the stream to the beginning of the compressed data
                            if (b == 0xC1)
                            {
                                b = r.ReadByte();
                                if (b == 0x83)
                                {
                                    b = r.ReadByte();
                                    if (b == 0x2A)
                                    {
                                        b = r.ReadByte();
                                        if (b == 0x9E)
                                        {
                                            foundStart = true;
                                            r.BaseStream.Seek(20, SeekOrigin.Current);
                                            nextChunkSize = r.ReadInt32();
                                            r.BaseStream.Seek(22, SeekOrigin.Current);
                                            inputStream.Position = r.BaseStream.Position;
                                            outputSize += nextChunkSize;
                                        }
                                    }
                                }
                            }
                        }
                        // decompresses the chunk body using the nextChunkSize variable as the amount of bytes that are read
                        using (FileStream outputStream = File.Create(Path.Combine(outputPath, "Body.txt")))
                        {
                            using (DeflateStream d = new(inputStream, CompressionMode.Decompress, true))
                            {
                                byte[] buffer = new byte[nextChunkSize];
                                d.Read(buffer, 0, nextChunkSize);
                                outputStream.Position = outputPosition;
                                outputStream.Write(buffer, 0, nextChunkSize);
                                foundStart = false;
                            }
                        }
                        count++;
                        outputPosition += nextChunkSize;
                    }
                }
            }
        }
        public void ParseHeader(string inputPath, string outputPath)
        {
            using (FileStream inputStream = File.OpenRead(inputPath))
            {
                using (FileStream outputStream = File.Create(Path.Combine(outputPath, "Header.txt")))
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
                        writer.Write(headerVersion);
                        writer.Write(saveVersion);
                        writer.Write(buildVersion);
                        writer.Write(mapName);
                        writer.Write(mapOptions);
                        writer.Write(sessionID);
                        writer.Write(playedSeconds);
                        writer.Write(tickTimeStamp);
                        writer.Write(sessionVisibility);
                        writer.Write(unrealVersion);
                        writer.Write(modMetaData);
                        writer.Write(modFlags);
                    }
                }
            }
        }
    }
}