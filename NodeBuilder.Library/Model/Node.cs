using System.Collections.Generic;

namespace NodeBuilder.Library.Model
{
    public class Node<T>
    {
        public string Name { get; set; }
        public IEnumerable<Node<T>> Children { get; set; }
    }
}
