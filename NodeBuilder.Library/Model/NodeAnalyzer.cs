using System.Collections.Generic;

namespace NodeBuilder.Library.Model
{
    public class NodeAnalyzer
    {        
        public string Name { get; set; }
        public IEnumerable<string> RemainingFolders { get; set; }
    }
}
