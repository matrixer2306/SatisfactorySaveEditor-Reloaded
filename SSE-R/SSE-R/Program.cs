using System.Diagnostics;

namespace SSE_R
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // set input file and output directory, specific output file is specified within the functions
            string inputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Outputs", "Zlib" ,"input3.sav");
            string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Outputs", "Zlib");

            Parser p = new();
            MemoryStream Header = p.ParseHeader(inputPath, outputPath);
            MemoryStream Body = p.ParseBody(inputPath, outputPath);
        }
    }
}