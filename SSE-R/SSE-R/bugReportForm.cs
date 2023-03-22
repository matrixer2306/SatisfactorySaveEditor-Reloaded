using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SSE_R
{
    public partial class bugReportForm : Form
    {
        public bugReportForm()
        {
            InitializeComponent();
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.DeselectAll();
            richTextBox1.HideSelection = true;
        }
        private void richTextBox1_LinkClicked(object sender, EventArgs e)
        {
            Process myProcess = new();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = "https://github.com/Joelminer-satisfactory/SatisfactorySaveEditor-Reloaded/issues/new";
            myProcess.Start();
        }
    }
}
