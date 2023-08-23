namespace SSE_R
{
    partial class MainApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
            tableLayoutPanel1 = new TableLayoutPanel();
            treeView1 = new TreeView();
            treeView2 = new TreeView();
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            flowLayoutPanel1 = new FlowLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.6666641F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(treeView1, 0, 1);
            tableLayoutPanel1.Controls.Add(treeView2, 0, 2);
            tableLayoutPanel1.Controls.Add(toolStrip1, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // treeView1
            // 
            treeView1.BackColor = Color.FromArgb(60, 62, 94);
            treeView1.BorderStyle = BorderStyle.FixedSingle;
            treeView1.Dock = DockStyle.Fill;
            treeView1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            treeView1.Indent = 19;
            treeView1.LineColor = Color.DarkGray;
            treeView1.Location = new Point(3, 73);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(527, 184);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // treeView2
            // 
            treeView2.BackColor = Color.FromArgb(60, 62, 94);
            treeView2.BorderStyle = BorderStyle.FixedSingle;
            treeView2.Dock = DockStyle.Fill;
            treeView2.Location = new Point(3, 263);
            treeView2.Name = "treeView2";
            treeView2.Size = new Size(527, 184);
            treeView2.TabIndex = 1;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.FromArgb(60, 62, 94);
            tableLayoutPanel1.SetColumnSpan(toolStrip1, 2);
            toolStrip1.Dock = DockStyle.Fill;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripButton2 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.RenderMode = ToolStripRenderMode.System;
            toolStrip1.Size = new Size(800, 70);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageAlign = ContentAlignment.BottomCenter;
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(67, 67);
            toolStripButton1.Text = "open save";
            toolStripButton1.TextImageRelation = TextImageRelation.ImageAboveText;
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // toolStripButton2
            // 
            toolStripButton2.Alignment = ToolStripItemAlignment.Right;
            toolStripButton2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageAlign = ContentAlignment.BottomCenter;
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(117, 67);
            toolStripButton2.Text = "Clear output folder";
            toolStripButton2.TextImageRelation = TextImageRelation.ImageAboveText;
            toolStripButton2.Click += toolStripButton2_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(536, 73);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            tableLayoutPanel1.SetRowSpan(flowLayoutPanel1, 2);
            flowLayoutPanel1.Size = new Size(261, 374);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // MainApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(60, 62, 94);
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            ForeColor = SystemColors.Control;
            Name = "MainApp";
            Text = "MainApp";
            Load += MainApp_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TreeView treeView1;
        private TreeView treeView2;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private static FlowLayoutPanel flowLayoutPanel1;
        private ToolStripButton toolStripButton2;
    }
}