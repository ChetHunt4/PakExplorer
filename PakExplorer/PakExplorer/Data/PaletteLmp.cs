using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public class QCColorValue
    {
        public float fR { get; set; }
        public float fG { get; set; }
        public float fB { get; set; }
        public float fA { get; set; }

        public byte bR { get; set; }
        public byte bG { get; set; }
        public byte bB { get; set; }
        public byte bA { get; set; }
        public QCColorValue(byte r, byte g, byte b, byte a)
        {
            fR = (float)(r / 256.0f);
            fG = (float)(g / 256.0f);
            fB = (float)(b / 256.0f);
            fA = (float)(a / 256.0f);

            bR = r;
            bG = g;
            bB = b;
            bA = a;
        }
    }

    public class PaletteLmp
    {
        public Dictionary<int, SKColor> PaletteLookup { get; set; }
        public PaletteLmp(Byte[] values)
        {
            PaletteLookup = new Dictionary<int, SKColor>();
            if (values != null && values.Length > 0)
            {
                for (int i = 0; i < values.Length / 3; i++)
                {
                    byte r = values[i * 3];
                    byte g = values[i * 3 + 1];
                    byte b = values[i * 3 + 2];
                    SKColor curValue = new SKColor(r, g, b, 255);
                    PaletteLookup.Add(i, curValue);
                }
            }
            PaletteLookup[255] = new SKColor(0, 0, 0, 0);
        }
    }
}
