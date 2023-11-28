using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Data
{
    public enum QuakeFileType
    {
        Sound,
        Model,
        Model2,
        BSP,
        LMP,
        PCX,
        Palette,
        Colormap,
        Wad,
        Sprite,
        Unknown
    }

    public enum WadType
    {
        Palette,
        StatusBar,
        MipMap,
        Console,
        Unknown
    }
}
