namespace SSE_R
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Outputs", "Zlib" ,"input.sav");
            string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Outputs", "Zlib");

            Parser p = new();
            p.Parse(inputPath, outputPath);
        }
    }
}