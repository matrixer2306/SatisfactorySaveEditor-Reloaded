namespace SSE_R
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            ApplicationConfiguration.Initialize();
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R"));
            }
            if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R", "Logs")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R", "Logs"));
            }
            if(File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R", "Logs", "Log.txt")))
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R", "Logs", "Log.txt"));
            }
            Application.Run(new MainApp());
        }
    }
}
