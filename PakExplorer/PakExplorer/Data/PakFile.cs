using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public class PakHeader
    {
        public char[] magic { get; set; }
        public int offIndex { get; set; }
        public int lenIndex { get; set; }
    }

    public class PakFile
    {
        public string name { get; set; }
        public int offset { get; set; }
        public int size { get; set; }
    }

    public class PakData
    {
        public PakHeader Header { get; set; }
        public Dictionary<string, PakFile> PakFiles { get; set; }
        public FileStream fs { get; set; }
        public BinaryReader reader { get; set; }
    }
}
