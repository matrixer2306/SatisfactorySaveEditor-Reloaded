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
            p.ParseHeader(inputPath, outputPath);
            p.ParseBody(inputPath, outputPath);
        }
    }
}