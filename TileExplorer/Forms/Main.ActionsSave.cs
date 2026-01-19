//#define SHOW_BOUNDARY_TILES

using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class Main
    {
        private bool ShowSaveFileDialog(SaveFileDialogType type, string initialDirectory)
        {
            saveFileDialog.FileName = string.Empty;
            saveFileDialog.InitialDirectory = initialDirectory;

            switch (type)
            {
                case SaveFileDialogType.Png:
                    saveFileDialog.DefaultExt = Resources.FileSaveDefaultExtPng;
                    saveFileDialog.Filter = Resources.FileSaveFilterPng;
                    break;
                case SaveFileDialogType.Osm:
                    saveFileDialog.DefaultExt = Resources.FileSaveDefaultExtOsm;
                    saveFileDialog.Filter = Resources.FileSaveFilterOsm;
                    break;
                case SaveFileDialogType.Gpx:
                    saveFileDialog.DefaultExt = Resources.FileSaveDefaultExtGpx;
                    saveFileDialog.Filter = Resources.FileSaveFilterGpx;
                    break;
            }
#if DEBUG
            saveFileDialog.FileName = Files.TempFileName("xxx") + "." + saveFileDialog.DefaultExt;

            DebugWrite.Line(saveFileDialog.FileName);

            return true;
#else
            return saveFileDialog.ShowDialog(this) == DialogResult.OK;
#endif
        }

        private void SaveToImage(string fileName)
        {
            SuspendLayout();
            toolStripContainer.ContentPanel.SuspendLayout();

            var savedBounds = gMapControl.Bounds;

            var imageSize = new Size(Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);

            try
            {
                gMapControl.Dock = DockStyle.None;

                gMapControl.SetBounds(
                    (toolStripContainer.ContentPanel.Width - imageSize.Width) / 2,
                    (toolStripContainer.ContentPanel.Height - imageSize.Height) / 2,
                    imageSize.Width,
                    imageSize.Height);

                UpdateGrid();

                gMapControl.ToImage().Save(fileName, ImageFormat.Png);
            }
            finally
            {
                gMapControl.Bounds = savedBounds;

                gMapControl.Dock = DockStyle.Fill;

                toolStripContainer.ContentPanel.ResumeLayout(false);
                toolStripContainer.PerformLayout();
                ResumeLayout(false);
            }
        }

        private void SaveMapToImage()
        {
            try
            {
                if (!ShowSaveFileDialog(SaveFileDialogType.Png, AppSettings.Local.Default.DirectoryLastMapImage)) return;

                AppSettings.Local.Default.DirectoryLastMapImage = Directory.GetParent(saveFileDialog.FileName).FullName;

                SaveToImage(saveFileDialog.FileName);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);
            }
        }

        private void SaveTileBoundaryToFile()
        {
            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                var pointFrom = gMapControl.FromLocalToLatLng(0, 0);
                var pointTo = gMapControl.FromLocalToLatLng(gMapControl.Width, gMapControl.Height);

                var tiles = new List<Tile>();

                for (var x = Utils.Osm.LngToTileX(pointFrom); x <= Utils.Osm.LngToTileX(pointTo); x++)
                    for (var y = Utils.Osm.LatToTileY(pointFrom); y <= Utils.Osm.LatToTileY(pointTo); y++)
                    {
                        tiles.Add(new Tile(x, y));
                    }

                try
                {
                    if (!ShowSaveFileDialog(SaveFileDialogType.Osm, AppSettings.Local.Default.DirectoryLastTileBoundary)) return;

                    AppSettings.Local.Default.DirectoryLastTileBoundary = Directory.GetParent(saveFileDialog.FileName).FullName;

                    Utils.Osm.SaveTilesToFile(saveFileDialog.FileName, tiles);
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);
                }
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }
        }

        private void SaveTileStatusToFile()
        {
            var boudaryTiles = Utils.Tiles.FindTilesBoundary(
                overlayTiles.Polygons.Cast<MapItemTile>().Select(t => t.Model).ToList());

#if SHOW_BOUNDARY_TILES
            foreach (var tile in boudaryTiles)
            {
                tilesOverlay.Polygons.Add(new MapItemTile(tile)
                {
                    Selected = true
                });
            }
#endif

            try
            {
                saveFileDialog.FileName = AppSettings.Roaming.Default.TileStatusFileWptType;

                if (!ShowSaveFileDialog(SaveFileDialogType.Gpx, AppSettings.Local.Default.DirectoryLastTileStatus)) return;

                AppSettings.Local.Default.DirectoryLastTileStatus = Directory.GetParent(saveFileDialog.FileName).FullName;

                Utils.Gpx.SaveTileStatusToFile(saveFileDialog.FileName, boudaryTiles);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);
            }
        }
    }
}