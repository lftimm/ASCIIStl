using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Core
{
    public class STLFile 
    {
        private string FilePath { get; set; }
        public string ThisFile { get => ReadFile(); }
        public STLFile(string path)
        {
            FilePath = path;
        }

        private string ReadFile()
        {
            if (!File.Exists(FilePath) )
                throw new Exception("File Path cannot be null");
            return File.ReadAllText(FilePath);
        }
    }
}
