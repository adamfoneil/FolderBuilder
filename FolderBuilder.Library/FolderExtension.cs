using FolderBuilder.Library.Internal;
using FolderBuilder.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FolderBuilder.Library
{
    public static class FolderExtension
    {
        public static Folder<T> ToFolderStructure<T>(this IEnumerable<T> items, Func<T, string> pathAccessor, params char[] pathSeparators)
        {
            if (!pathSeparators?.Any() ?? true) pathSeparators = new char[] { '\\', '/' };

            var folderedItems = items.Select(item =>
            {
                var folders = pathAccessor
                    .Invoke(item).Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(folder => folder.Trim());

                return new FolderAnalyzer<T>()
                {
                    Item = item,
                    Name = folders.First(),
                    RemainingFolders = folders.Skip(1).ToArray()
                };
            }).ToArray();            

            Folder<T> root = new Folder<T>();
            root.Folders = GetChildFoldersR(folderedItems);
            root.Items = GetLeafItems(string.Empty, folderedItems);
            return root;
        }

        private static IEnumerable<Folder<T>> GetChildFoldersR<T>(IEnumerable<FolderAnalyzer<T>> items)
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
                    
                    result.Folders = GetChildFoldersR(nestedItems);                    
                    
                    return result;
                }));

            return results;
        }

        private static IEnumerable<T> GetLeafItems<T>(string folderName, IEnumerable<FolderAnalyzer<T>> items)
        {            
            return items
                .Where(item => item.Name.Equals(folderName) && item.RemainingFolders.Count() == 1)
                .Select(leaf => leaf.Item).ToArray();                           
        }
    }
}
