using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public class SpriteHeader
    {
        public char[] magic { get; set; }
        public int version { get; set; }
        public int type { get; set; }
        public float radius { get; set; }
        public int maxWidth { get; set; }
        public int maxHeight { get; set; }
        public int nFrames { get; set; }
        public float beamLength { get; set; }
        public int syncType { get; set; }
    }

    public class SpriteFrame
    {
        public int groups { get; set; }
        public int nPics { get; set; }
        public float[] frames { get; set; }
        public SpritePic[] spritePic { get; set; }
    }


    public class SpritePic
    {
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public byte[] pixels { get; set; }
    }

    public class GfxSpr
    {
        public SpriteHeader Header { get; set; }

        public GfxSpr(byte[] sprite, PaletteLmp palette, BinaryReader br)
        {
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            ReadHeader(sprite, br);


        }

        private void ReadHeader(byte[] sprite, BinaryReader br)
        {
            Header = new SpriteHeader
            {
                magic = br.ReadChars(4)
            };
            string magic = new string(Header.magic);
            if (magic != "IDSP")
            {
                return;
            }
            Header.version = br.ReadInt32();
            Header.type = br.ReadInt32();
            Header.radius = br.ReadSingle();
            Header.maxWidth = br.ReadInt32();
            Header.maxHeight = br.ReadInt32();
            Header.nFrames = br.ReadInt32();
            Header.beamLength = br.ReadSingle();
            Header.syncType = br.ReadInt32();
        }
    }
}
