using PakExplorer.Data;
using SkiaSharp;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace PakExplorer.Helpers
{
    public static class TextureHelper
    {
        public static BitmapDataPackage ExtractSKBitmapFromLMP(string gfxFile, QuakeFileType type, PakData pak, PaletteLmp palette, int Width = 0, int Height = 0)
        {
            byte[] bytes = DataHelper.ExtractFile(gfxFile, pak);
            return ExtractSKBitmapFromLMP(bytes, type, palette, Width, Height);
        }

        public static BitmapDataPackage ExtractSKBitmapFromLMP(byte[] bytes, QuakeFileType type, PaletteLmp palette, int Width = 0, int Height = 0)
        {
            byte[] rest = null;
            if (palette != null && bytes != null && bytes.Length > 0)
            {
                if (type == QuakeFileType.LMP)
                {
                    var widthArray = new byte[] {
                    bytes[0],
                    bytes[1],
                    bytes[2],
                    bytes[3]
                    };

                    var heightArray = new byte[] {
                    bytes[4],
                    bytes[5],
                    bytes[6],
                    bytes[7]
                };
                    uint width = BinaryPrimitives.ReadUInt32LittleEndian(widthArray);
                    uint height = BinaryPrimitives.ReadUInt32LittleEndian(heightArray);
                    Width = (int)width;
                    Height = (int)height;
                    rest = bytes.Skip(8).ToArray();
                }
                else
                {
                    rest = bytes;
                }
                SKBitmap bitmap = new SKBitmap(Width, Height);
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        bitmap.SetPixel(x, y, palette.PaletteLookup[rest[y * Width + x]]);
                    }
                }
                var package = new BitmapDataPackage
                {
                    Width = Width,
                    Height = Height,
                    OriginalBitmap = bitmap,
                    UpdatedBitmap = bitmap
                };
                return package;
            }
            return null;
        }


        public static BitmapSource BitmapImageFromByteArray(byte[] imageBytes)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new System.IO.MemoryStream(imageBytes);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public static BitmapSource GetBitmapFromSKBitmap(SKBitmap bitmap)
        {
            if (bitmap != null)
            {
                SKImageInfo imageInfo = new SKImageInfo(bitmap.Width, bitmap.Height);
                using (SKSurface surface = SKSurface.Create(imageInfo))
                {
                    SKCanvas canvas = surface.Canvas;
                    using (SKPaint paint = new SKPaint())
                    {
                        canvas.DrawBitmap(bitmap, 0, 0);
                    }
                    using (SKImage datImage = surface.Snapshot())
                    using (SKData data = datImage.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        //using (MemoryStream ms = new MemoryStream(data.ToArray()))
                        //{
                        byte[] imageBytes = data.ToArray();
                        BitmapSource bmsource = BitmapImageFromByteArray(imageBytes);

                        return bmsource;
                        //}
                    }
                }
            }
            return null;
        }

        public static bool SaveImageToFile(string filename, SKBitmap bitmap, SKEncodedImageFormat format)
        {
            try
            {
                using (var image = SKImage.FromBitmap(bitmap))
                {
                    using (var data = image.Encode(format, 100))
                    {
                        using (var stream = new System.IO.FileStream(filename, FileMode.Create))
                        {
                            data.SaveTo(stream);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
