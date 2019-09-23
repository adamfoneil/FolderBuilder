using System.Collections.Generic;

namespace FolderBuilder.Library.Internal
{
    internal class FolderAnalyzer<T>
    {        
        public T Item { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> RemainingFolders { get; set; }
    }
}
