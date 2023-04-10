namespace SSE_R
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // set input file and output directory, specific output file is specified within the functions
            ApplicationConfiguration.Initialize();
            Application.Run(new MainApp());
        }
    }
}
