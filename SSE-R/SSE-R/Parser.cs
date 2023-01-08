using System.Diagnostics;
using System.Drawing.Text;
using System.IO.Compression;
using System.Net;
using System.Text;
using Windows.Devices.PointOfService;
using Windows.Media.Playback;

namespace SSE_R
{
    public class Parser
    {
        public void ParseBody(string inputPath, string outputPath)
        {
            long outputPosition = 0;
            int count = 0;
            FileStream inputStream = File.OpenRead(inputPath);
            Debug.WriteLine(inputStream.Length);
            while (inputStream.Position < inputStream.Length)
            {
                bool foundStart = false;
                while (!foundStart)
                {
                    int b = inputStream.ReadByte();
                    if (b == -1)
                    {
                        inputStream.Close();
                        return;
                    }
                    if (b == 0x78)
                    {
                        b = inputStream.ReadByte();
                        if (b == 0x9c)
                        {
                            Debug.WriteLine($"Found chunk start at {inputStream.Position}");
                            foundStart = true;
                        }
                    }
                }
                if (foundStart)
                {
                    using (FileStream outputStream = File.Create(Path.Combine(outputPath, "Body.txt")))
                    {
                        using (DeflateStream decompressionStream = new(inputStream, CompressionMode.Decompress, true))
                        {
                            if (!decompressionStream.CanRead)
                            {
                                throw new Exception("cant read from decompressionstream");
                            }
                            else if (!outputStream.CanWrite)
                            {
                                throw new Exception("cant write to output.txt");
                            }
                            if (outputStream.CanWrite && decompressionStream.CanRead)
                            {
                                outputStream.Position = outputPosition;
                                byte[] buffer = new byte[131072];
                                int bytesread = decompressionStream.Read(buffer, 0, 131072);
                                outputStream.Write(buffer, 0, 131072);
                                foundStart = false;
                                inputStream.Position += bytesread;
                                outputPosition = outputStream.Position;
                                count++;
                            }
                        }
                    }
                }
            }
            inputStream.Close();
            Debug.WriteLine($"decompressed {count} chunks");
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
                    Debug.WriteLine(Pos16());

                    string mapName = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    Debug.WriteLine($"string length is {nextStringLength}");
                    buffer = readerUTF16.ReadBytes(nextStringLength);
                    foreach(byte b in buffer) { mapName += (char)b; }
                    Debug.WriteLine(Pos16());

                    string mapOptions = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    Debug.WriteLine($"string length is {nextStringLength}");
                    if (nextStringLength < 0) { buffer = new byte[-nextStringLength]; int count = 0;  while (count < Math.Abs(nextStringLength)) { buffer[count] = readerUTF16.ReadByte(); readerUTF16.BaseStream.Position++; count++; Debug.WriteLine($"count is: {count}"); }  foreach (byte b in buffer) { mapOptions += (char)b; } Debug.WriteLine(Pos16()); }
                    if (nextStringLength > 0) { buffer = readerUTF8.ReadBytes(nextStringLength); foreach (byte b in buffer) { mapOptions += (char)b; Debug.WriteLine(Pos8()); }}
                    if (nextStringLength == 0) { mapOptions = ""; inputStream.Position += nextStringLength; }

                    string sessionID = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    Debug.WriteLine($"string length is {nextStringLength}");
                    if (nextStringLength < 0) { buffer = new byte[-nextStringLength]; int count = 0; while (count < Math.Abs(nextStringLength)) { buffer[count] = readerUTF16.ReadByte(); readerUTF16.BaseStream.Position++; count++; } foreach (byte b in buffer) { sessionID += (char)b; } }
                    if (nextStringLength > 0) { buffer = readerUTF8.ReadBytes(nextStringLength); foreach (byte b in buffer) { sessionID += (char)b; } }
                    if (nextStringLength == 0) { sessionID = ""; inputStream.Position += nextStringLength; }

                    int playedSeconds = readerUTF16.ReadInt32();
                    long tickTimeStamp = readerUTF16.ReadInt64();
                    byte sessionVisibility = readerUTF16.ReadByte();
                    int unrealVersion = readerUTF16.ReadInt32();

                    Debug.WriteLine(Pos16());
                    string modMetaData = "";
                    nextStringLength = readerUTF16.ReadInt32();
                    Debug.WriteLine($"string length is {nextStringLength}");
                    buffer = readerUTF16.ReadBytes(nextStringLength);
                    foreach (byte b in buffer) { modMetaData += (char)b; }

                    int modFlags = readerUTF16.ReadInt32();

                    Debug.WriteLine($"Header version is: {headerVersion}");
                    Debug.WriteLine($"Save version is: {saveVersion}");
                    Debug.WriteLine($"Build version is: {buildVersion}");
                    Debug.WriteLine($"Map name is: {mapName}");
                    Debug.WriteLine($"Map Options are: {mapOptions}");
                    Debug.WriteLine($"Session ID is: {sessionID}");
                    Debug.WriteLine($"Played seconds: {playedSeconds}");
                    Debug.WriteLine($"Tick timestamp is: {tickTimeStamp}");
                    Debug.WriteLine($"Session visibility is: {sessionVisibility}");
                    Debug.WriteLine($"Mod metadata is: {modMetaData}");
                    Debug.WriteLine($"modded?: {modFlags}");
                }
            }
        }
    }
}