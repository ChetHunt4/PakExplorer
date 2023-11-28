using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public class BitmapDataPackage
    {
        public SKBitmap OriginalBitmap { get; set; }
        public SKBitmap UpdatedBitmap { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
