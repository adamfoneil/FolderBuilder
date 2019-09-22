﻿using NodeBuilder.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeBuilder.Library
{
    public static class FolderExtension
    {
        public static Folder<T> ToFolder<T>(this IEnumerable<T> items, Func<T, string> pathAccessor, params char[] pathSeparators)
        {
            if (!pathSeparators?.Any() ?? true) pathSeparators = new char[] { '\\', '/', ':' };

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
            root.Folders = GetChildFoldersR(root, folderedItems, new Stack<string>());
            root.Items = GetItemsAtPath<T>(new Stack<string>(), folderedItems);
            return root;
        }

        private static IEnumerable<Folder<T>> GetChildFoldersR<T>(Folder<T> parent, IEnumerable<FolderAnalyzer<T>> items, Stack<string> path)
        {
            List<Folder<T>> results = new List<Folder<T>>();

            results
                .AddRange(items
                .GroupBy(item => item.Name)
                .Select(grp => 
                {
                    path.Push(grp.Key);

                    var result = new Folder<T>()
                    {
                        Name = grp.Key,
                        Items = GetItemsAtPath(path, grp)
                    };                    

                    var nestedItems = grp
                        .Where(item => item.RemainingFolders.Any())
                        .Select(item => new FolderAnalyzer<T>
                        {
                            Item = item.Item,
                            Name = item.RemainingFolders.First(),
                            RemainingFolders = item.RemainingFolders.Skip(1).ToArray()
                        }).ToArray();
                    
                    result.Folders = GetChildFoldersR(result, nestedItems, path);                    

                    path.Pop();
                    return result;
                }));

            return results;
        }

        private static IEnumerable<T> GetItemsAtPath<T>(Stack<string> path, IEnumerable<FolderAnalyzer<T>> items)
        {            
            return items
                .Where(item => path.SequenceEqual(item.RemainingFolders))
                .Select(leaf => leaf.Item).ToArray();
        }
    }
}
