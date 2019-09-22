﻿using System;
using System.Collections.Generic;

namespace NodeBuilder.Library.Model
{
    internal class FolderAnalyzer<T>
    {        
        public T Item { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> RemainingFolders { get; set; }
    }
}
