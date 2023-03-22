using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SSE_R
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //show form (see Form1.cs and Form1.cs [Design] for design and functions)
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
            //Environment.Exit(0);
            // all code in this file below this line will eventually have to move to form1.cs, probably as event handlers
            // set input file and output directory, specific output file is specified within the functions
            //BugReport.CreateIssue();
            string docsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            string inputPath = "";
            
            if(!File.Exists(Path.Combine(docsFolder, "Outputs")))
            {
                Directory.CreateDirectory(Path.Combine(docsFolder, "Outputs"));
                Debug.WriteLine("Created Folder: Outputs");
            }
            if(!File.Exists(Path.Combine(docsFolder, "Outputs", "SSE-R")))
            {
                Directory.CreateDirectory(Path.Combine(docsFolder, "Outputs", "SSE-R"));
                Debug.WriteLine("Created Folder: SSE-R");
            }

            string outputPath = Path.Combine(docsFolder, "Outputs", "SSE-R");
            
            OpenFileDialog inputFile = new OpenFileDialog();
            inputFile.InitialDirectory = "c:\\";
            inputFile.Filter = "save files (.sav)|*.sav";
            inputFile.RestoreDirectory = true;
            if (inputFile.ShowDialog() == DialogResult.OK)
            {
                inputPath = inputFile.FileName;
            }
            if (inputPath != null && inputPath != "")
            {
                Parser p = new Parser();
                MemoryStream header = p.ParseHeader(inputPath, outputPath);
                MemoryStream body = p.ParseBody(inputPath, outputPath);
                LevelReader l = new LevelReader();
                List<int> offsets = l.GetOffsets(body);
                Debug.WriteLine($"found {offsets.Count} offsets");
                body.Position = 0;
                body.CopyTo(File.Create(Path.Combine(outputPath, "body.bin")));
                foreach (int offset in offsets) //read all sublevels
                {
                    Debug.Write($"{offset}, ");
                    l.readLevel(body, offset);
                }
                l.readLevel(body, body.Position, false); //read the peristent level
            }
            Debug.Write("\n Successfully parsed file! \n");
        }
    }
}
