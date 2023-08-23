using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static SSE_R.MainApp;

namespace SSE_R
{
    public class misc
    {
        public class MySR : ToolStripSystemRenderer
        {
            public MySR() { }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                //base.OnRenderToolStripBorder(e);
            }
        }
        public class SubLevel
        {
            public long offset { get; set; }
            public string name { get; set; }

            public SubLevel(long Offset, string Name)
            {
                offset = Offset;
                name = Name;
            }
        }
        public class LogFile
        {
            
            public static async Task AddLogEntry(string info)
            {
                using (FileStream logFile = File.OpenWrite(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R", "Logs", "Log.txt")))
                {
                    string logPrefix = "[";
                    logPrefix += DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss");

                    string logOut = logPrefix + "] " + info;

                    logFile.Position = logFile.Length;
                    byte[] logArray = Encoding.ASCII.GetBytes(logOut);
                    await logFile.WriteAsync(logArray, 0, logArray.Length);
                }
            }
        }
    }
}
