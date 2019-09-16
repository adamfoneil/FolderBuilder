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
            var nodeable = items.Select(i => new Nodeable<T>()
            {
                Item = i,
                Folders = pathAccessor.Invoke(i).Split(new char[] { pathSeparator }, StringSplitOptions.RemoveEmptyEntries).ToArray()
            });

            int maxDepth = nodeable.Max(i => i.Folders.Length);

            Node<T> root = new Node<T>();

            GetChildNodes(root, 0, nodeable);

            return root;
        }

        private static IEnumerable<Node<T>> GetChildNodes<T>(Node<T> parent, int level, IEnumerable<Nodeable<T>> nodeables)
        {            
            var folderedItems = nodeables.GroupBy(item => item.Folders[level]);

            parent.Children = folderedItems.Select(grp => new Node<T>()
            {
                Name = grp.Key,
                Children = grp.Select(item => )
            });            
        }
    }
}
