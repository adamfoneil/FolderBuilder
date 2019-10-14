using System.Collections.Generic;

namespace FolderBuilder.Library.Models
{
    public class Folder<T>
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<Folder<T>> Folders { get; set; }
    }
}
