//#define SHOW_BOUNDARY_TILES

using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private bool ShowSaveFileDialog(SaveFileDialogType type)
        {
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
                if (!ShowSaveFileDialog(SaveFileDialogType.Png)) return;

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
            var status = ProgramStatus.Start(Status.SaveData);

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
                    if (!ShowSaveFileDialog(SaveFileDialogType.Osm)) return;

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
                ProgramStatus.Stop(status);
            }
        }

        private void SaveTileStatusToFile()
        {
            var boudaryTiles = Utils.Tiles.FindTilesBoundary(
                tilesOverlay.Polygons.Cast<MapItemTile>()
                    .Select(t => t.Model).ToList());

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
                saveFileDialog.FileName = AppSettings.Default.TileStatusFileWptType;

                if (!ShowSaveFileDialog(SaveFileDialogType.Gpx)) return;

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