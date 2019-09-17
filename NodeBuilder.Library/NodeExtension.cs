using NodeBuilder.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeBuilder.Library
{
    public static class NodeExtension
    {
        public static Node<T> ToNode<T>(this IEnumerable<T> items, Func<T, string> pathAccessor, char pathSeparator = '\\')
        {
            var folderedItems = items.Select(item =>
            {
                var folders = pathAccessor.Invoke(item).Split(new char[] { pathSeparator }, StringSplitOptions.RemoveEmptyEntries);
                return new NodeAnalyzer()
                {
                    Name = folders.First(),
                    RemainingFolders = folders.Skip(1)
                };
            });

            Node<T> root = new Node<T>();
            root.Children = GetChildNodesR(root, folderedItems);
            return root;
        }

        private static IEnumerable<Node<T>> GetChildNodesR<T>(Node<T> parent, IEnumerable<NodeAnalyzer> items)
        {            
            List<Node<T>> results = new List<Node<T>>();

            results.AddRange(items
                .GroupBy(item => item.Name)
                .Select(grp => new Node<T>()
                {
                    Name = grp.Key                    
                }));

            foreach (var node in results)
            {

            }

            return results;
        }

        private static IEnumerable<NodeAnalyzer> GetFolderedItems<T>(IEnumerable<T> items, Func<T, string> pathAccessor, char pathSeparator)
        {
            return items.Select(item =>
            {
                var folders = pathAccessor.Invoke(item).Split(new char[] { pathSeparator }, StringSplitOptions.RemoveEmptyEntries);
                return new NodeAnalyzer()
                {                    
                    Name = folders.First(),
                    RemainingFolders = folders.Skip(1)
                };
            });
        }
    }
}
