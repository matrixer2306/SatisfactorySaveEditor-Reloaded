namespace SSE_R
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            tableLayoutPanel1 = new TableLayoutPanel();
            label4 = new Label();
            mainTreeView = new TreeView();
            childTreeView = new TreeView();
            MainMenuBar = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            tableLayoutPanel1.SuspendLayout();
            MainMenuBar.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AllowDrop = true;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel1.Controls.Add(label4, 1, 1);
            tableLayoutPanel1.Controls.Add(mainTreeView, 0, 1);
            tableLayoutPanel1.Controls.Add(childTreeView, 0, 2);
            tableLayoutPanel1.Controls.Add(MainMenuBar, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(944, 501);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel1.SetColumnSpan(label4, 2);
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(314, 75);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            tableLayoutPanel1.SetRowSpan(label4, 2);
            label4.Size = new Size(630, 426);
            label4.TabIndex = 3;
            label4.Text = "Values";
            label4.TextAlign = ContentAlignment.TopCenter;
            // 
            // mainTreeView
            // 
            mainTreeView.BackColor = Color.FromArgb(60, 62, 98);
            mainTreeView.BorderStyle = BorderStyle.FixedSingle;
            mainTreeView.Dock = DockStyle.Fill;
            mainTreeView.ForeColor = Color.White;
            mainTreeView.Indent = 19;
            mainTreeView.Location = new Point(0, 75);
            mainTreeView.Margin = new Padding(0);
            mainTreeView.Name = "mainTreeView";
            mainTreeView.Size = new Size(314, 275);
            mainTreeView.TabIndex = 4;
            mainTreeView.AfterSelect += mainTreeView_AfterSelect;
            // 
            // childTreeView
            // 
            childTreeView.BackColor = Color.FromArgb(60, 62, 98);
            childTreeView.BorderStyle = BorderStyle.FixedSingle;
            childTreeView.Dock = DockStyle.Fill;
            childTreeView.ForeColor = Color.White;
            childTreeView.Location = new Point(0, 350);
            childTreeView.Margin = new Padding(0);
            childTreeView.Name = "childTreeView";
            childTreeView.Size = new Size(314, 151);
            childTreeView.TabIndex = 5;
            // 
            // MainMenuBar
            // 
            MainMenuBar.BackColor = Color.FromArgb(60, 62, 98);
            tableLayoutPanel1.SetColumnSpan(MainMenuBar, 3);
            MainMenuBar.Dock = DockStyle.Fill;
            MainMenuBar.GripStyle = ToolStripGripStyle.Hidden;
            MainMenuBar.Items.AddRange(new ToolStripItem[] { toolStripButton1 });
            MainMenuBar.Location = new Point(0, 0);
            MainMenuBar.Name = "MainMenuBar";
            MainMenuBar.Padding = new Padding(0);
            MainMenuBar.RenderMode = ToolStripRenderMode.System;
            MainMenuBar.Size = new Size(944, 75);
            MainMenuBar.TabIndex = 6;
            MainMenuBar.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.AutoSize = false;
            toolStripButton1.ForeColor = Color.White;
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(72, 72);
            toolStripButton1.Text = "Load Save";
            toolStripButton1.TextAlign = ContentAlignment.BottomCenter;
            toolStripButton1.TextImageRelation = TextImageRelation.ImageAboveText;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(60, 62, 98);
            ClientSize = new Size(944, 501);
            Controls.Add(tableLayoutPanel1);
            ForeColor = Color.FromArgb(60, 62, 98);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "SSE-R";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            MainMenuBar.ResumeLayout(false);
            MainMenuBar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label4;
        private TreeView mainTreeView;
        private TreeView childTreeView;
        private ToolStrip MainMenuBar;
        private ToolStripButton toolStripButton1;
    }
}