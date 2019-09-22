using System.Collections.Generic;

namespace NodeBuilder.Library.Model
{
    public class Folder<T>
    {
        public string Name { get; set; }
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<Folder<T>> Folders { get; set; }
    }
}
