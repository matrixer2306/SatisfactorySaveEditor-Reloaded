using System;
using System.Diagnostics;
using static SSE_R.misc;

namespace SSE_R
{
    public partial class MainApp : Form
    {
        public MemoryStream? header;
        public MemoryStream? body;
        public List<SubLevel>? subLevels;

        public MainApp()
        {
            InitializeComponent();
            toolStrip1.Renderer = new SSE_R.misc.MySR();
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog filePicker = new OpenFileDialog();
            string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            if (!File.Exists(Path.Combine(appDataFolder, "SSE-R")))
            {
                Directory.CreateDirectory(Path.Combine(appDataFolder, "SSE-R"));
            }
            string inputPath = "";
            string outputPath = Path.Combine(appDataFolder, "SSE-R");
            if (filePicker.ShowDialog() == DialogResult.OK)
            {
                inputPath = filePicker.FileName;
            }

            if (inputPath != null && inputPath != "")
            {
                Loading form = new Loading();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
                string saveFileName = "";
                string formattedDateTime = DateTime.Now.ToString("d-M-yyyy_HH-mm-ss");
                try
                {
                    form.Text = "reading header";
                    var headerobj = Parser.ParseHeader(inputPath, outputPath);
                    header = headerobj.Item1;
                    saveFileName = headerobj.Item2;
                    saveFileName = saveFileName.TrimEnd('\0');
                    if (File.Exists(Path.Combine(outputPath, "Header.bin")))
                    {
                        File.Move(Path.Combine(outputPath, "Header.bin"), Path.Combine(outputPath, $"{"[HEADER] " + saveFileName + " " + formattedDateTime}.bin"));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "invalid save version", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly | MessageBoxOptions.ServiceNotification, false);
                    form.Close();
                    return;
                }
                form.progressBar1.Value = 10;
                form.Text = "decompressing body";
                body = Parser.ParseBody(inputPath, outputPath, $"{"[BODY] " + saveFileName + " " + formattedDateTime}.bin");
                Debug.WriteLine($"Sent file to {Path.Combine(outputPath, $"{"[BODY] " + saveFileName + " " + formattedDateTime}.bin")}");
                form.progressBar1.Value = 90;
                form.Text = "finding sublevels";
                subLevels = Finder.FindSubLevels(body);
                foreach (SubLevel s in subLevels)
                {
                    string name = s.name;
                    name = name.Remove(0, 40);
                    if (!name.Contains("Audio"))
                    {
                        TreeNode node = new TreeNode("Sublevel: " + name);
                        node.ForeColor = Color.White;
                        node.Tag = s.name;
                        treeView1.Nodes.Add(node);
                    }
                }
                form.progressBar1.Value = 100;
                form.Close();
            }
        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int index = subLevels.FindIndex(s => s.name == (string)treeView1.SelectedNode.Tag);
            if (index != subLevels.Count - 1)
            {
                LevelReaders.ReadLevel(body, subLevels[index].offset, true);
            }

            foreach (TreeNode node in treeView1.SelectedNode.Nodes)
            {
                treeView2.Nodes.Add(node);
            }
        }
    }
}
