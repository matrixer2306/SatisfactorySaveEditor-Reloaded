using System.Diagnostics;
using System.IO.Compression;
using System.Net;

namespace SSE_R
{
    public class Parser
    {
        public void Parse(string inputPath, string outputPath)
        {
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
                using (FileStream decompressedStream = File.Create(Path.Combine(outputPath, $"output{count}.txt")))
                {
                    using (DeflateStream decompressionStream = new(inputStream, CompressionMode.Decompress, true))
                    {
                        decompressionStream.CopyTo(decompressedStream, 131072);
                        Debug.WriteLine($"Successfully decompressed the chunk {count}");
                        count ++;
                        Debug.WriteLine(inputStream.Position);
                    }
                }
            }
            inputStream.Close();
        }
    }
}