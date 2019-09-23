I wanted to build a standard way of converting a flat list of `T` into a hierarchical list based on some kind of `path` contained in `T`. A typical example would be to take a list of file names, and derive a tree structure of that list. However, this is intended to work with any data that has path info, not just file names.

This will have a role in my upcoming **SqlChartify** app, but this requirement comes up every now and then in other places, so I wanted to make this an open source library (and Nuget package eventually). As usual with things like this, this is both a learning exercise for myself and something with a practical intended use. I struggle with recursion-based things like this, and I found it a difficult and worthwhile exercise.

Here is the method itself: [ToFolderStructure](https://github.com/adamosoftware/FolderBuilder/blob/master/FolderBuilder.Library/FolderExtension.cs#L10).

Here are some [test cases](https://github.com/adamosoftware/FolderBuilder/blob/master/Tests/FolderTests.cs) that show what it's meant to do. The tests rely on text comparison with some json content, which is [here](https://github.com/adamosoftware/FolderBuilder/tree/master/Tests/Resources) as embedded resources.

To make it more visually obvious what this does, I added a [WinForms](https://github.com/adamosoftware/FolderBuilder/tree/master/WinFormsTest) project using a TreeView control.

![img](https://adamosoftware.blob.core.windows.net:443/images/FolderBuilder.png)

This prompts you for a folder, then fills a tree view control the folder structure of the files. Source [here](https://github.com/adamosoftware/FolderBuilder/blob/master/WinFormsTest/Form1.cs#L28). The `ToFolderStructure` extension method is used here. First, it gets all the file names from the selected directory. Then, the filename list is converted to an enumerable of `FileInfo` because I want my tree structure to convey all the file properties (size, date stamps, etc), not just the names. Finally, the `ToFolderStructure` call derives the hierarchical structure.
```
var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
var fileInfos = files.Select(fileName => new FileInfo(fileName));
var fileTree = fileInfos.ToFolderStructure((fi) => fi.FullName);
```
Clicking on a node in the tree view shows the corresponding file properties in the property grid control on the right. Note how the property grid `SelectedObject` comes from the node's [FileInfo property](https://github.com/adamosoftware/FolderBuilder/blob/master/WinFormsTest/Controls/FileNode.cs#L16). Note also the icons are built using my [WinForms.Library](https://github.com/adamosoftware/WinForms.Library) project [AddIcon](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/FileSystem_Icons.cs#L92) method.
```
private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
{
    var fileNode = e.Node as FileNode;
    if (fileNode != null)
    {
        propertyGrid1.SelectedObject = fileNode.FileInfo;
    }            
}
```

