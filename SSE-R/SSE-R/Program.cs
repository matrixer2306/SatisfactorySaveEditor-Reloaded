using System.Diagnostics;
using System.Windows.Forms;

namespace SSE_R
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // set input file and output directory, specific output file is specified within the functions
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

            OpenFileDialog filePicker = new OpenFileDialog();
            if (filePicker.ShowDialog() == DialogResult.OK)
            {
                inputPath = filePicker.FileName;
            }
            if (inputPath != null)
            {
                Parser p = new Parser();
                MemoryStream header = p.ParseHeader(inputPath, outputPath);
                MemoryStream body = p.ParseBody(inputPath, outputPath);
            }
        }
    }
}
