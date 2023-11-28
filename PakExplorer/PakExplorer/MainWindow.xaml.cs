using Microsoft.Win32;
using Newtonsoft.Json;
using PakExplorer.Controllers;
using PakExplorer.Data;
using PakExplorer.Helpers;
using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace PakExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Global Variables
        public PakController PC { get; set; }
        public PaletteLmp Palette { get; set; }

        private BitmapDataPackage _graphicData { get; set; }
        private GfxWadFile _wad { get; set; }
        private GfxMdl _mdl { get; set; }
        private GfxBSPLoader _bsp { get; set; }

        const string _paletteName = "gfx/palette.lmp";
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events
        #region Menu
        private void menuItem_OpenProjectClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pak Files(*.Pak)| *.Pak| All files(*.*) | *.*";
            openFileDialog.DefaultExt = ".pak";

            var openResult = openFileDialog.ShowDialog();
            if (openResult == true)
            {
                PC = new PakController(openFileDialog.FileName);
                //data = DataHelper.GetPakData(openFileDialog.FileName);
                var filename = Path.GetFileName(openFileDialog.FileName);
                this.Title = "Pak Explorer - " + filename;
                lstPakFile.Items.Clear();
                if (PC.PakData != null && PC.PakData.PakFiles != null && PC.PakData.PakFiles.Count > 0)
                {
                    foreach (var pakFile in PC.PakData.PakFiles)
                    {
                        lstPakFile.Items.Add(pakFile.Key);
                    }
                }
            }
        }

        private void menuItem_QuitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region ListViews
        private void lstSelection_Changed(object sender, RoutedEventArgs e)
        {
            var selected = lstPakFile.SelectedItem.ToString();
            if (!string.IsNullOrWhiteSpace(selected))
            {
                var selectedItem = lstPakFile.SelectedItem.ToString();
                if (!string.IsNullOrWhiteSpace(selectedItem))
                {
                    var file = Path.GetFileName(selectedItem);
                    lblFilename.Content = "Filename: " + file;
                    lblFilename.Visibility = Visibility.Visible;
                    var fileType = DataHelper.GetFileType(file);
                    resetUI(fileType, false);
                    var bytes = DataHelper.ExtractFile(selectedItem, PC.PakData);
                    if (bytes != null)
                    {
                        switch (fileType)
                        {
                            case QuakeFileType.Palette:
                                Palette = new PaletteLmp(bytes);
                                break;
                            case QuakeFileType.LMP:
                                if (!loadPalette())
                                {
                                    resetUI(fileType, false);
                                }
                                else
                                {
                                    resetUI(fileType, false);
                                    _graphicData = TextureHelper.ExtractSKBitmapFromLMP(selectedItem, fileType, PC.PakData, Palette);
                                    resetUI(fileType, true);
                                }
                                break;
                            case QuakeFileType.Wad:
                                if (!loadPalette())
                                {
                                    resetUI(fileType, false);
                                }
                                else
                                {
                                    resetUI(fileType, false);
                                    _wad = PC.GetWadFromFile(selectedItem, Palette);
                                    resetUI(fileType, true);
                                }
                                break;
                            case QuakeFileType.Model:
                                if (!loadPalette())
                                {
                                    resetUI(fileType, false);
                                }
                                else
                                {
                                    resetUI(fileType, false);
                                    _mdl = PC.GetModelFromFile(selectedItem, Palette);
                                    resetUI(fileType, true);
                                }
                                break;
                            case QuakeFileType.BSP:
                                if (!loadPalette())
                                {
                                    resetUI(fileType, false);
                                }
                                else
                                {
                                    resetUI(fileType, false);
                                    _bsp = PC.GetBspFromFile(selectedItem, Palette);
                                    resetUI(fileType, true);
                                }
                                break;
                            case QuakeFileType.Sprite:
                                if (!loadPalette())
                                {
                                    resetUI(fileType, false);
                                }
                                else
                                {
                                    resetUI(fileType, false);
                                    GfxSpr sprite = PC.GetSpriteFromFile(selectedItem, Palette);
                                    resetUI(fileType, true);
                                }
                                break;
                        }
                    }
                    resetUI(fileType, true);
                }
            }

        }

        private void lstWadSelection_Changed(object sender, RoutedEventArgs e)
        {
            if (_wad != null && lstWadFile.SelectedItem != null)
            {
                var wadPath = lstWadFile.SelectedItem.ToString();
                var wadType = _wad.GetWadTypeFromFile(wadPath);
                var bytes = _wad.ExtractWadFile(wadPath);
                switch (wadType)
                {
                    case WadType.StatusBar:
                        _graphicData = TextureHelper.ExtractSKBitmapFromLMP(bytes, QuakeFileType.LMP, Palette);
                        txtWidth.Text = _graphicData.Width.ToString();
                        txtHeight.Text = _graphicData.Height.ToString();
                        break;
                    case WadType.MipMap:
                        if (wadPath.ToUpper() == "CONCHARS")
                        {
                            _graphicData = TextureHelper.ExtractSKBitmapFromLMP(bytes, QuakeFileType.Wad, Palette, 128, 128);
                            txtWidth.Text = "128";
                            txtHeight.Text = "128";

                        }
                        break;
                }
                iImage.MaxWidth = 200;
                iImage.MaxHeight = 200;
                iImage.Source = TextureHelper.GetBitmapFromSKBitmap(_graphicData.UpdatedBitmap);
                pnlImage.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region Combo Boxes
        private void cmbModelTexture_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cmbModelTextures.SelectedIndex >= 0)
            {
                var index = cmbModelTextures.SelectedIndex;
                _graphicData = _mdl.Textures[index];
                iImage.Source = TextureHelper.GetBitmapFromSKBitmap(_graphicData.UpdatedBitmap);

            }
        }
        #endregion

        #region Buttons
        private void btnFindPalette_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pak Files(*.Pak)| *.Pak| All files(*.*) | *.*";
            openFileDialog.DefaultExt = ".pak";

            var openResult = openFileDialog.ShowDialog();
            if (openResult == true)
            {
                PakController localController = new PakController(openFileDialog.FileName);
                if (localController.PakData.PakFiles.ContainsKey(_paletteName))
                {
                    var paletteBytes = DataHelper.ExtractFile(_paletteName, localController.PakData);
                    Palette = new PaletteLmp(paletteBytes);
                }
                //var bytes = localController.ExtractFile()
                if (lstWadFile.Visibility == Visibility.Visible)
                {
                    lstWadSelection_Changed(sender, e);
                }
                else
                {
                    lstSelection_Changed(sender, e);
                }
            }
            //resetUI(QuakeFileType.Palette, true);
        }

        private void btnExportGfx_Click(object sender, EventArgs e)
        {
            var filename = lstWadFile.Visibility == Visibility.Visible ? lstWadFile.SelectedItem.ToString() : lstPakFile.SelectedItem.ToString();
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Files(*.PNG)| *.PNG| All files(*.*) | *.*";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.FileName = Path.GetFileNameWithoutExtension(filename) + "." + saveFileDialog.DefaultExt;
            var saveDialogResult = saveFileDialog.ShowDialog();
            if (saveDialogResult == true)
            {
                switch (Path.GetExtension(saveFileDialog.FileName).ToUpper())
                {
                    case ".PNG":
                        TextureHelper.SaveImageToFile(saveFileDialog.FileName, _graphicData.UpdatedBitmap, SKEncodedImageFormat.Png);
                        MessageBox.Show("Image saved successfully!", "Image saved", MessageBoxButton.OK);
                        break;
                }
            }
        }

        private void btnExportModelToObj_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Obj Files(*.obj)| *.obj| All files(*.*) | *.*";
            saveFileDialog.DefaultExt = ".obj";

            var saveResult = saveFileDialog.ShowDialog();
            if (saveResult == true)
            {
                ModelHelper.ExportObjFromMdl(saveFileDialog.FileName, _mdl);
                MessageBox.Show("Model successfully saved!", "Model saved", MessageBoxButton.OK);
            }
        }

        private void btnExportMapTextures_Click(object sender, RoutedEventArgs e)
        {
            if (_bsp != null && _bsp.WallTextures != null && _bsp.WallTextures.Count > 0)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Files(*.PNG)| *.PNG| All files(*.*) | *.*";
                saveFileDialog.DefaultExt = ".png";
                var saveResult = saveFileDialog.ShowDialog();
                if (saveResult == true)
                {
                    foreach (var texture in _bsp.WallTextures)
                    {
                        var textureName = texture.Key;
                        if (textureName.StartsWith('*'))
                        {
                            textureName = textureName.Substring(1);
                        }
                        var filename = Path.GetDirectoryName(saveFileDialog.FileName) + "\\" + Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + "_" + textureName + Path.GetExtension(saveFileDialog.FileName);
                        TextureHelper.SaveImageToFile(filename, texture.Value, SKEncodedImageFormat.Png);
                    }
                    MessageBox.Show("Textures saved successfully", "Textures saved");
                }
            }
            else
            {
                MessageBox.Show("No Textures to export", "No Textures", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnExportMapEntities_Click(object sender, RoutedEventArgs e)
        {
            if (_bsp != null && _bsp.Entities != null && _bsp.Entities.Count > 0)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JSON Files(*.JSON)| *.JSON| All files(*.*) | *.*";
                saveFileDialog.DefaultExt = ".json";
                var saveResult = saveFileDialog.ShowDialog();
                if (saveResult == true)
                {
                    var serialized = JsonConvert.SerializeObject(_bsp.Entities);
                    File.WriteAllText(saveFileDialog.FileName, serialized);
                    MessageBox.Show("Json successfully save", "Json saved", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("No Entities to save!", "No entities", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnExportMapToObj_Click(object sender, RoutedEventArgs e)
        {
            if (_bsp != null && _bsp.Models != null && _bsp.Models.Count > 0)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "OBJ Files(*.OBJ)| *.OBJ| All files(*.*) | *.*";
                saveFileDialog.DefaultExt = ".obj";
                var saveResult = saveFileDialog.ShowDialog();
                if (saveResult == true)
                {
                    if (ModelHelper.ExportBspToObj(saveFileDialog.FileName, _bsp))
                    {
                        MessageBox.Show("Obj Saved Successfully!", "Obj Saved Successfully", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Obj Failed to Save", "Obj Failed to Save", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        #endregion
        #endregion

        #region Private Helper Methods
        private bool loadPalette()
        {
            if (Palette == null)
            {
                if (PC.PakData.PakFiles.ContainsKey(_paletteName))
                {
                    var paletteBytes = DataHelper.ExtractFile(_paletteName, PC.PakData);
                    Palette = new PaletteLmp(paletteBytes);
                    return true;
                }
                return false;
            }
            else
                return true;
        }

        private void showPaletteLoadUI()
        {
            pnlLoadPalette.Visibility = Visibility.Visible;
            pnlPalette.Visibility = Visibility.Collapsed;
        }

        private void resetUI(QuakeFileType type, bool loaded)
        {
            switch (type)
            {
                case QuakeFileType.Palette:
                    pnlPalette.Visibility = Visibility.Visible;
                    lblPalette.Content = loaded ? "Palette successfully loaded" : "Palette loading...";

                    pnlLoadPalette.Visibility = Visibility.Collapsed;
                    lstWadFile.Visibility = Visibility.Collapsed;
                    pnlImage.Visibility = Visibility.Collapsed;
                    break;
                case QuakeFileType.LMP:
                    if (Palette != null)
                    {
                        if (loaded)
                        {
                            txtWidth.Text = _graphicData.Width.ToString();
                            txtHeight.Text = _graphicData.Height.ToString();
                            iImage.MaxWidth = 400;
                            iImage.MaxHeight = 400;
                            iImage.Source = TextureHelper.GetBitmapFromSKBitmap(_graphicData.UpdatedBitmap);

                            pnlImage.Visibility = Visibility.Visible;
                            pnlPalette.Visibility = Visibility.Collapsed;
                            pnlLoadPalette.Visibility = Visibility.Collapsed;

                        }
                        else
                        {
                            pnlImage.Visibility = Visibility.Collapsed;
                            lblPalette.Content = "Image loading...";
                            pnlPalette.Visibility = Visibility.Visible;
                            pnlLoadPalette.Visibility = Visibility.Visible;
                        }
                        pnlModelTextures.Visibility = Visibility.Collapsed;
                        pnlMdlDetails.Visibility = Visibility.Collapsed;
                        btnExportModelToObj.Visibility = Visibility.Collapsed;

                    }
                    else
                    {
                        pnlImage.Visibility = Visibility.Collapsed;
                        showPaletteLoadUI();
                    }
                    lstWadFile.Visibility = Visibility.Collapsed;
                    pnlMap.Visibility = Visibility.Collapsed;
                    break;
                case QuakeFileType.Wad:
                    if (Palette != null)
                    {
                        if (loaded)
                        {
                            if (_wad != null)
                            {
                                lstWadFile.Items.Clear();
                                foreach (var item in _wad.WadFiles)
                                {
                                    lstWadFile.Items.Add(item.Key);
                                }
                                lstWadFile.Visibility = Visibility.Visible;
                                pnlLoadPalette.Visibility = Visibility.Collapsed;

                            }
                            else
                            {
                                lstWadFile.Visibility = Visibility.Collapsed;
                            }
                            pnlModelTextures.Visibility = Visibility.Collapsed;
                            pnlMdlDetails.Visibility = Visibility.Collapsed;
                            btnExportModelToObj.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            lstWadFile.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        showPaletteLoadUI();
                    }
                    pnlImage.Visibility = Visibility.Collapsed;
                    pnlMap.Visibility = Visibility.Collapsed;

                    break;
                case QuakeFileType.Model:
                    if (Palette != null)
                    {
                        if (loaded)
                        {
                            _graphicData = _mdl.Textures[0];
                            txtWidth.Text = _graphicData.Width.ToString();
                            txtHeight.Text = _graphicData.Height.ToString();
                            lblVerts.Content = "Triangles: " + _mdl.Frames[0].Frame.Verts.Count;
                            lblFrames.Content = "Frames: " + _mdl.Frames.Count;
                            cmbModelTextures.Items.Clear();
                            if (_mdl.Textures.Count > 0)
                            {
                                for (int i = 0; i < _mdl.Textures.Count; i++)
                                {
                                    cmbModelTextures.Items.Add("Texture" + i.ToString());
                                }
                                cmbModelTextures.SelectedIndex = 0;

                            }
                            iImage.MaxWidth = 400;
                            iImage.MaxHeight = 400;
                            iImage.Source = TextureHelper.GetBitmapFromSKBitmap(_graphicData.UpdatedBitmap);

                            pnlImage.Visibility = Visibility.Visible;
                            pnlModelTextures.Visibility = Visibility.Visible;
                            pnlMdlDetails.Visibility = Visibility.Visible;
                            btnExportModelToObj.Visibility = Visibility.Visible;
                            pnlPalette.Visibility = Visibility.Collapsed;
                            pnlLoadPalette.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            pnlImage.Visibility = Visibility.Collapsed;
                            lblPalette.Content = "Model loading...";
                            pnlPalette.Visibility = Visibility.Visible;
                            pnlLoadPalette.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        showPaletteLoadUI();
                        pnlImage.Visibility = Visibility.Collapsed;
                    }
                    pnlMap.Visibility = Visibility.Collapsed;
                    break;
                case QuakeFileType.BSP:
                    if (Palette != null)
                    {
                        if (loaded)
                        {
                            pnlMap.Visibility = Visibility.Visible;
                            pnlPalette.Visibility = Visibility.Collapsed;
                            pnlLoadPalette.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            lblPalette.Content = "BSP Loading...";
                            pnlPalette.Visibility = Visibility.Visible;
                            pnlLoadPalette.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        showPaletteLoadUI();

                        pnlMap.Visibility = Visibility.Collapsed;
                    }
                    pnlImage.Visibility = Visibility.Collapsed;
                    pnlMdlDetails.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        #endregion
    }
}
