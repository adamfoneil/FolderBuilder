using System.IO;
using System.Windows.Forms;
using WinForms.Library;

namespace WinFormsTest.Controls
{
    public class FileNode : TreeNode
    {
        public FileNode(FileInfo fileInfo, ImageList imageList) : base(Path.GetFileName(fileInfo.FullName))
        {
            ImageKey = FileSystem.AddIcon(imageList, fileInfo.FullName, FileSystem.IconSize.Large, "folder.png");
            SelectedImageKey = ImageKey;
            FileInfo = fileInfo;
        }

        public FileInfo FileInfo { get; }
    }
}
