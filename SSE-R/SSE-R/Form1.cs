using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSE_R
{
    public partial class Form1 : Form
    {
        public string? selectedFilePath { get; set; }

        public Form1()
        {
            InitializeComponent();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "sav files (*.sav)|*.sav";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                MessageBox.Show(selectedFilePath);
                DialogResult = DialogResult.OK;
                
            }
            else
            {
                DialogResult = DialogResult.No;
                
            }
        }
        
        //public Form1()
        //{
        //    InitializeComponent();
        //    Button selectFileButton = new Button();
        //    selectFileButton.Text = "Click to select file";
        //    selectFileButton.Location = new System.Drawing.Point(12, 12);
        //    selectFileButton.Click += new System.EventHandler(button1_Click);
        //    Controls.Add(selectFileButton);
        //}
        //
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFileDialog1 = new OpenFileDialog();
        //
        //    openFileDialog1.InitialDirectory = "c:\\";
        //    openFileDialog1.Filter = "sav files (*.sav)|*.sav";
        //    openFileDialog1.FilterIndex = 2;
        //    openFileDialog1.RestoreDirectory = true;
        //
        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        string selectedFilePath = openFileDialog1.FileName;
        //        MessageBox.Show(selectedFilePath);
        //        Close();
        //    }
        //}
    }
}
