using System;
using System.Diagnostics;
using Windows.UI.WebUI;
using static SSE_R.misc;
using static SSE_R.misc.LogFile;

namespace SSE_R
{
    //TODO: investigate how viable a  AddFormItem(string type){} function is
    public partial class MainApp : Form
    {
        public MemoryStream? header;
        public MemoryStream? body;
        public List<SubLevel>? subLevels;
        public static FileStream StreamOffsets = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R", "Offsets.txt"));

        public MainApp()
        {
            InitializeComponent();
            toolStrip1.Renderer = new SSE_R.misc.MySR();
        }

        //public static void AddFormItem(string type)
        //{
        //    switch (type)
        //    {
        //        default:
        //            throw new Exception("Unhandled type, check spelling or implement");
        //        case "Textbox":
        //            flowLayoutPanel1.Controls.Add(new TextBox());
        //            break;
        //    }
        //}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            treeView2.BeginUpdate();
            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            header = null;
            body = null;
            if(subLevels != null)
            {
                subLevels.Clear();
            }
            GC.Collect(); //collect garbage
            treeView1.EndUpdate();
            treeView2.EndUpdate();
            OpenFileDialog filePicker = new OpenFileDialog();
            string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
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
                //form.Show();
                string saveFileName = "";
                string formattedDateTime = DateTime.Now.ToString("d-M-yyyy_HH-mm-ss");
                //try
                //{
                form.Text = "reading header";
                var headerobj = Parser.ParseHeader(inputPath, outputPath);
                header = headerobj.Item1;
                saveFileName = headerobj.Item2;
                if(headerobj.Item3 == true)
                {
                    return;
                }
                saveFileName = saveFileName.TrimEnd('\0');
                if (File.Exists(Path.Combine(outputPath, "Header.bin")))
                {
                    File.Move(Path.Combine(outputPath, "Header.bin"), Path.Combine(outputPath, $"{"[HEADER] " + saveFileName + " " + formattedDateTime}.bin"));
                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "invalid save version", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly | MessageBoxOptions.ServiceNotification, false);
                //    form.Close();
                //    return;
                //}
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
                LevelReaders reader = new LevelReaders();
                reader.panel = flowLayoutPanel1;
                reader.treeView = treeView1;

                SubLevel PersistentLevel = Finder.FindPersistentLevel(body);
                subLevels.Add(PersistentLevel);

                treeView1.BeginUpdate();
                TreeNode Treenode = new TreeNode(PersistentLevel.name);
                Treenode.ForeColor = Color.White;
                Treenode.Tag = PersistentLevel.name;
                treeView1.Nodes.Add(Treenode);
                treeView1.EndUpdate();

                form.progressBar1.Value = 100;
                form.Close();
            }
        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public TreeNode previousnode;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView1.BeginUpdate();
            treeView2.BeginUpdate();
            if (treeView1.SelectedNode.Level == 0)
            {
                previousnode?.Nodes?.Clear();
            }
            previousnode = treeView1.SelectedNode;
            LevelReaders reader = new LevelReaders();
            reader.panel = flowLayoutPanel1;
            reader.treeView = treeView1;
            if (treeView1.SelectedNode.Parent == null)
            {
                int index = subLevels.FindIndex(s => s.name == (string)treeView1.SelectedNode.Tag);
                if (index != subLevels.Count - 1)
                {
                    reader.ReadLevel(body, subLevels[index].offset, true);
                }
                if (index == subLevels.Count - 1)
                {
                    reader.ReadLevel(body, subLevels[index].offset, false);
                }
            }
            treeView2.Nodes.Clear();
            foreach (TreeNode node in previousnode.Nodes)
            {
                if (node != null)
                {
                    node.Parent.Nodes.Remove(node);

                    treeView2.Nodes.Add(node);
                }

            }
            treeView1.EndUpdate();
            treeView2.EndUpdate();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            foreach (string file in Directory.GetFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SSE-R"), "*.bin"))
            {
                File.Delete(file);
            }
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            treeView2.BeginUpdate();
            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();
            header = null;
            body = null;
            subLevels.Clear();
            GC.Collect(); //collect garbage
            treeView1.EndUpdate();
            treeView2.EndUpdate();
        }
    }
}
