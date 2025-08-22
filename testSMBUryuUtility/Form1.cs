using shittyFileManager;
namespace testSMBUryuUtility
{
    public partial class Form1 : Form
    {
        SBMUFM shittybigboyTools = new SBMUFM();
        string SelectedLmao;

        private void refreshList()
        {
            treeView1.Nodes.Clear();
            Dictionary<string, Dictionary<string, string>> AllMods = shittybigboyTools.getAllMods();
            foreach (var item in AllMods)
            {
                TreeNode mainNode = new();
                mainNode.Text = item.Key;
                foreach (var item1 in item.Value)
                {
                    TreeNode node = new();
                    node.Text = item1.Key;
                    node.ToolTipText = item1.Value;
                    mainNode.Nodes.Add(node);
                }

                treeView1.Nodes.Add(mainNode);
            }
        }
        public Form1()
        {
            InitializeComponent();
            refreshList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            shittybigboyTools.Toggle(SelectedLmao);
            refreshList();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SelectedLmao = e.Node.ToolTipText;
            label1.Text = SelectedLmao;
            checkBox1.CheckState = !shittybigboyTools.modState(SelectedLmao) ? CheckState.Checked : CheckState.Unchecked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            shittybigboyTools.addArc();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            shittybigboyTools.addSkyline();
        }
    }
}
