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
        public static class LogFile
        {
            private static FileStream stream = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R", "Logs", "Log.bin"));

        }
    }
}
