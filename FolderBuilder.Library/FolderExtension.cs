using FolderBuilder.Library.Internal;
using FolderBuilder.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FolderBuilder.Library
{
    public static class FolderExtension
    {
        public static Folder<T> ToFolderStructure<T>(this IEnumerable<T> items, Func<T, string> pathAccessor, char pathSeparator = '/')
        {
            var folderedItems = items.Select(item =>
            {
                var folders = pathAccessor
                    .Invoke(item).Split(new char[] { pathSeparator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(folder => folder.Trim());

                return new FolderAnalyzer<T>()
                {
                    Item = item,
                    Name = folders.First(),
                    RemainingFolders = folders.Skip(1).ToArray()
                };
            }).ToArray();            

            Folder<T> root = new Folder<T>();
            root.Folders = GetChildFoldersR(folderedItems.Where(item => item.RemainingFolders.Count() > 0), null, pathSeparator);
            root.Items = GetLeafItems(string.Empty, folderedItems);
            return root;
        }

        private static IEnumerable<Folder<T>> GetChildFoldersR<T>(IEnumerable<FolderAnalyzer<T>> items, string parentName, char pathSeparator)
        {
            List<Folder<T>> results = new List<Folder<T>>();

            results
                .AddRange(items
                .GroupBy(item => item.Name)
                .Select(grp => 
                {
                    var result = new Folder<T>()
                    {
                        Name = grp.Key,
                        FullName = string.IsNullOrEmpty(parentName) ? grp.Key : parentName + pathSeparator + grp.Key,
                        Items = GetLeafItems(grp.Key, grp)
                    };                    

                    var nestedItems = grp
                        .Where(item => item.RemainingFolders.Count() >= 2)
                        .Select(item => new FolderAnalyzer<T>
                        {
                            Item = item.Item,
                            Name = item.RemainingFolders.First(),
                            RemainingFolders = item.RemainingFolders.Skip(1).ToArray()
                        }).ToArray();
                    
                    result.Folders = GetChildFoldersR(nestedItems, result.FullName, pathSeparator);
                    
                    return result;
                }));

            return results;
        }

        private static IEnumerable<T> GetLeafItems<T>(string folderName, IEnumerable<FolderAnalyzer<T>> items)
        {
            return (!string.IsNullOrEmpty(folderName)) ?
                items
                    .Where(item => item.Name.Equals(folderName) && item.RemainingFolders.Count() == 1)
                    .Select(leaf => leaf.Item).ToArray() :
                items
                    .Where(item => item.RemainingFolders.Count() == 0)
                    .Select(leaf => leaf.Item).ToArray();
        }
    }
}
