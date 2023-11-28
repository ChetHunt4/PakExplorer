using PakExplorer.Data;
using PakExplorer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakExplorer.Controllers
{
    public class PakController
    {
        public PakData PakData { get; set; }

        public PakController(string fileName)
        {
            PakData = DataHelper.GetPakData(fileName);
        }

        #region Extraction Methods
        public GfxWadFile GetWadFromFile(string wad, PaletteLmp palette)
        {
            if (DataHelper.GetFileType(wad) == QuakeFileType.Wad)
            {
                var bytes = DataHelper.ExtractFile(wad, PakData);
                return new GfxWadFile(bytes, palette, PakData.reader);
            }
            return null;
        }

        public GfxMdl GetModelFromFile(string mdl, PaletteLmp palette)
        {
            if (DataHelper.GetFileType(mdl) == QuakeFileType.Model)
            {
                var bytes = DataHelper.ExtractFile(mdl, PakData);
                return new GfxMdl(bytes, palette, PakData.reader);
            }
            return null;
        }

        public GfxBSPLoader GetBspFromFile(string bsp, PaletteLmp palette)
        {
            if (DataHelper.GetFileType(bsp) == QuakeFileType.BSP)
            {
                var bytes = DataHelper.ExtractFile(bsp, PakData);
                return new GfxBSPLoader(bytes, palette, PakData.reader);
            }
            return null;
        }

        public GfxSpr GetSpriteFromFile(string sprite, PaletteLmp palette)
        {
            if (DataHelper.GetFileType(sprite) == QuakeFileType.Sprite)
            {
                var bytes = DataHelper.ExtractFile(sprite, PakData);
                return new GfxSpr(bytes, palette, PakData.reader);
            }
            return null;
        }
        #endregion
    }
}
