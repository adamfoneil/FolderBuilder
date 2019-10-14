using FolderBuilder.Library;
using FolderBuilder.Library.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WinFormsTest.Controls;

namespace WinFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FillTreeView(dlg.SelectedPath);
            }
        }

        private void FillTreeView(string path)
        {
            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            var fileInfos = files.Select(fileName => new FileInfo(fileName));
            var fileTree = fileInfos.ToFolderStructure((fi) => fi.FullName);

            treeView1.Nodes.Clear();
            var node = treeView1.Nodes.Add(path);
            node.ImageKey = "folder.png";

            try
            {
                treeView1.BeginUpdate();
                AddChildNodesR(node, fileTree);
            }
            finally
            {
                treeView1.EndUpdate();
            }

            node.Expand();
        }

        private void AddChildNodesR(TreeNode parentNode, Folder<FileInfo> fileTree)
        {
            foreach (var file in fileTree.Items)
            {
                parentNode.Nodes.Add(new FileNode(file, imlSmallIcons));
            }

            foreach (var child in fileTree.Folders)
            {
                var node = new FolderNode(child.Name);
                parentNode.Nodes.Add(node);
                AddChildNodesR(node, child);
                node.Expand();
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var fileNode = e.Node as FileNode;
            if (fileNode != null)
            {
                propertyGrid1.SelectedObject = fileNode.FileInfo;
            }            
        }
    }
}
