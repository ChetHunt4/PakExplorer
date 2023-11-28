using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public class WadFileData
    {
        public int Offset { get; set; }
        public int DSize { get; set; }
        public int Size { get; set; }
        public WadType Type { get; set; }
        public bool Compressed { get; set; }
        public string Name { get; set; }
    }

    public class GfxWadFile
    {
        public int NumEntries { get; set; }
        private int _directoryOffset { get; set; }
        private PaletteLmp _palette { get; set; }
        private long _baseStartPos { get; set; }
        private BinaryReader _br { get; set; }

        public Dictionary<string, WadFileData> WadFiles { get; set; }


        public GfxWadFile(byte[] wad, PaletteLmp palette, BinaryReader br)
        {
            _palette = palette;
            _br = br;
            var stream = br.BaseStream;
            stream.Position = stream.Position - wad.Length;
            _baseStartPos = stream.Position;
            ReadHeader(wad, br);

            stream = br.BaseStream;
            stream.Position = _baseStartPos + _directoryOffset;
            WadFiles = new Dictionary<string, WadFileData>();
            for (int i = 0; i < NumEntries; i++)
            {
                ReadFile(br);
            }
        }

        private bool ReadHeader(byte[] wad, BinaryReader br)
        {
            var magic = new string(br.ReadChars(4));
            if (magic != "WAD2")
            {
                return false;
            }
            NumEntries = br.ReadInt32();
            _directoryOffset = br.ReadInt32();
            return true;
        }

        private void ReadFile(BinaryReader br)
        {
            WadFileData newWad = new WadFileData();
            newWad.Offset = br.ReadInt32();
            newWad.DSize = br.ReadInt32();
            newWad.Size = br.ReadInt32();
            char type = br.ReadChar();
            switch (type)
            {
                case '@':
                    newWad.Type = WadType.Palette;
                    break;
                case 'B':
                    newWad.Type = WadType.StatusBar;
                    break;
                case 'D':
                    newWad.Type = WadType.MipMap;
                    break;
                case 'E':
                    newWad.Type = WadType.Console;
                    break;
            }
            newWad.Compressed = br.ReadChar() == '0' ? true : false;
            //read dummy
            br.ReadInt16();
            string name = new string(br.ReadChars(16));
            int index = name.IndexOf('\0');
            if (index != -1)
            {
                name = name.Substring(0, index);
            }
            newWad.Name = name;
            WadFiles.Add(newWad.Name, newWad);
        }

        public List<string> GetFileNames()
        {
            return WadFiles.Select(s => s.Key).ToList();
        }

        public byte[] ExtractWadFile(string name)
        {
            if (WadFiles.ContainsKey(name))
            {
                var wad = WadFiles[name];
                if (wad != null)
                {
                    var stream = _br.BaseStream;
                    stream.Position = _baseStartPos + wad.Offset;
                    byte[] buffer = _br.ReadBytes(wad.Size);
                    return buffer;
                }
            }
            return null;
        }

        public WadType GetWadTypeFromFile(string name)
        {
            if (WadFiles.ContainsKey(name))
            {
                return WadFiles[name].Type;
            }
            return WadType.Unknown;
        }
    }
}
