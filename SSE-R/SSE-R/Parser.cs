using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Text;
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
                        throw new Exception("Reached end of file before finding the requested bytes");
                        // replace with ending of file reading later
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
                                decompressionStream.Read(buffer, 0, 131072);
                                outputStream.Write(buffer, 0, 131072);
                                foundStart = false;
                                inputStream.Position += 131072;
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
            FileStream inputStream = File.OpenRead(inputPath);
            inputStream.Position = 0;
            using (FileStream outputStream = File.Create(Path.Combine(outputPath, "Header.txt")))
            {
                Debug.WriteLine($"input stream is {inputStream.Length} bytes long");
                inputStream.Position = 0;
                int count = 0;
                while (inputStream.Position < outputStream.Length)
                {
                    
                    byte[] buffer = new byte[2];
                    inputStream.Read(buffer, 0, 2);
                    if (buffer[0] == 0x78 && buffer[1] == 0x9c)
                    {
                        Debug.WriteLine($"found 78 9c after {count*2} bytes");
                        break;
                    }
                    else { outputStream.Write(buffer, 0, 2); }
                    count++;
                }
                Debug.WriteLine($"stopped after {count * 2} bytes");
            }
            inputStream.Close();
        }
    }
}