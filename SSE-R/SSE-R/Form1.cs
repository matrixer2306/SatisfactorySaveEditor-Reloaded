namespace SSE_R
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MainMenuBar.Renderer = new Misc.MySR();
        }

        private void mainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            childTreeView.BeginUpdate();
            childTreeView.Nodes.Clear();
            TreeView parent = (TreeView)sender;
            if (parent == null)
            {
                return;
            };
            int childNodeCount = parent.SelectedNode.GetNodeCount(false);
            for (int i = 0; i < childNodeCount; i++)
            {
                TreeNode childNode = (TreeNode)parent.SelectedNode.Nodes[i].Clone();
                childTreeView.Nodes.Add(childNode);

            }
            childTreeView.ExpandAll();
            childTreeView.EndUpdate();
        }
    }
}