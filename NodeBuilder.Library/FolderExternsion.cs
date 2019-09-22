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

            Folder<T> root = new Folder<T>();
            root.Folders = GetChildFoldersR(root, folderedItems);
            return root;
        }

        private static IEnumerable<Folder<T>> GetChildFoldersR<T>(Folder<T> parent, IEnumerable<FolderAnalyzer<T>> items)
        {            
            List<Folder<T>> results = new List<Folder<T>>();

            results.AddRange(items
                .GroupBy(item => item.Name)
                .Select(grp => new Folder<T>()
                {
                    Name = grp.Key                    
                }));

            foreach (var folder in results)
            {
                //folder.Folders = GetChildFoldersR<T>(folder, )
            }

            return results;
        }
    }
}
