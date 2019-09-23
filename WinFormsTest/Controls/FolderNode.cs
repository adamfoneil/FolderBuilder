using System.IO;
using System.Windows.Forms;

namespace WinFormsTest.Controls
{
    public class FolderNode : TreeNode
    {
        public FolderNode(string name) : base(name)
        {
            ImageKey = "folder.png";
        }
    }
}
