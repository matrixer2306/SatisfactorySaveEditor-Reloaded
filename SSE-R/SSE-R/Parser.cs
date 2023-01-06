using System.Diagnostics;
using System.IO.Compression;
using System.Net;

namespace SSE_R
{
    public class Parser
    {
        public void Parse(string inputPath, string outputPath)
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
                            Debug.WriteLine(inputStream.Position);
                            foundStart = true;
                        }
                    }
                }
                if (foundStart)
                {
                    using (FileStream outputStream = File.Create(Path.Combine(outputPath, "output.txt")))
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
            Console.WriteLine($"decompressed {count} chunks");
        }
    }
}