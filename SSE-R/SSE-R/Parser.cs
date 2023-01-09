using System.Diagnostics;
using System.Drawing.Text;
using System.IO.Compression;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using Windows.Devices.PointOfService;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace SSE_R
{
    public class Parser
    {
        public void ParseBody(string inputPath, string outputPath)
        {
            using (FileStream inputStream = new(inputPath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new(inputStream, Encoding.Unicode, true))
                {
                    int count = 0;
                    long outputPosition = 0;
                    bool foundStart = false;
                    int nextChunkSize = 0;
                    int b = 0;
                    inputStream.Position = 0;
                    int outputSize = 0;
                    while (true)
                    {
                        while (!foundStart)
                        {
                            try
                            {
                                b = r.ReadByte();
                            }
                            catch (EndOfStreamException ex)
                            {
                                Debug.WriteLine("Reached the end of the stream");
                                Debug.WriteLine(count, "count");
                                Debug.WriteLine(nextChunkSize, "last chunk size = ");
                                Debug.WriteLine(outputSize, "body.txt size in bytes should be ");
                                return;
                            }

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
                                            Debug.WriteLine(r.BaseStream.Position, "chunkposition: ");
                                            r.BaseStream.Seek(20, SeekOrigin.Current);
                                            nextChunkSize = r.ReadInt32();
                                            Debug.WriteLine(nextChunkSize, "next chunk size: ");
                                            r.BaseStream.Seek(22, SeekOrigin.Current);
                                            inputStream.Position = r.BaseStream.Position;
                                            outputSize += nextChunkSize;
                                        }
                                    }
                                }
                            }
                        }

                        using (FileStream outputStream = File.Create(Path.Combine(outputPath, "Body.txt")))
                        {
                            using (DeflateStream d = new(inputStream, CompressionMode.Decompress, true))
                            {
                                Debug.WriteLine(inputStream.Position, "decomp position");
                                byte[] buffer = new byte[nextChunkSize];
                                d.Read(buffer, 0, nextChunkSize);
                                outputStream.Position = outputPosition;
                                outputStream.Write(buffer, 0, nextChunkSize);
                                Debug.WriteLine($"successfully decompressed chunk {count}");
                                foundStart = false;
                                outputStream.WriteByte(0x0A);
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

                    long Pos16()
                    {
                       return readerUTF16.BaseStream.Position;
                    }
                    long Pos8()
                    {
                        return readerUTF8.BaseStream.Position;
                    }
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
                    if (nextStringLength > 0) { buffer = readerUTF8.ReadBytes(nextStringLength); foreach (byte b in buffer) { mapOptions += (char)b; Debug.WriteLine(Pos8()); }}
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