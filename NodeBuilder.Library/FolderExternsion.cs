using NodeBuilder.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeBuilder.Library
{
    public static class FolderExtension
    {
        public static Folder<T> ToFolder<T>(this IEnumerable<T> items, Func<T, string> pathAccessor, params char[] pathSeparators)
        {
            if (pathSeparators == null) pathSeparators = new char[] { '\\', '/', ':', '.' };

            var folderedItems = items.Select(item =>
            {
                var folders = pathAccessor
                    .Invoke(item).Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(folder => folder.Trim());

                return new FolderAnalyzer<T>()
                {
                    Item = item,
                    Name = folders.First(),
                    RemainingFolders = folders.Skip(1)
                };
            });

            int maxLevel = folderedItems.Max(item => item.RemainingFolders.Count());

            Folder<T> root = new Folder<T>();
            root.Folders = GetChildFoldersR(root, folderedItems, 0, maxLevel);
            root.Items = GetItemsAtDepth<T>(0, folderedItems);
            return root;
        }

        private static IEnumerable<Folder<T>> GetChildFoldersR<T>(Folder<T> parent, IEnumerable<FolderAnalyzer<T>> items, int currentLevel, int maxLevel)
        {            
            List<Folder<T>> results = new List<Folder<T>>();

            results.AddRange(items
                .GroupBy(item => item.Name)
                .Select(grp => new Folder<T>()
                {
                    Name = grp.Key,
                    Items = GetItemsAtDepth(currentLevel, grp)
                }));

            if (currentLevel < maxLevel)
            {
                var nestedItems = items.Select(item => new FolderAnalyzer<T>()
                {
                    Item = item.Item,
                    Name = item.RemainingFolders.First(),
                    RemainingFolders = item.RemainingFolders.Skip(1)
                });

                foreach (var folder in results)
                {
                    currentLevel++;
                    folder.Folders = GetChildFoldersR<T>(folder, nestedItems, currentLevel, maxLevel);
                    currentLevel--;
                }
            }

            return results;
        }

        private static IEnumerable<T> GetItemsAtDepth<T>(int level, IEnumerable<FolderAnalyzer<T>> items)
        {
            return items.Where(item => !item.RemainingFolders.Any()).Select(leaf => leaf.Item);
        }
    }
}
