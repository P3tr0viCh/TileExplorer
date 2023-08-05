using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class Main
    {
#if !DEBUG
        private enum SaveFileDialogType
        {
            Png, Osm
        }

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
            }

            return saveFileDialog.ShowDialog(this) == DialogResult.OK;
        }
#endif

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
#if DEBUG
                SaveToImage(Files.TempFileName("xxx.png"));
#else
                if (ShowSaveFileDialog(SaveFileDialogType.Png))
                {
                    SaveToImage(saveFileDialog.FileName);
                }
#endif
            }
            catch (Exception e)
            {
                Utils.WriteError(e);

                Msg.Error(e.Message);
            }
        }
        private void SaveTileBoundaryToFile()
        {
            var pointFrom = gMapControl.FromLocalToLatLng(0, 0);
            var pointTo = gMapControl.FromLocalToLatLng(gMapControl.Width, gMapControl.Height);

            var tiles = new List<Tile>();

            for (var x = Utils.Osm.LngToTileX(pointFrom); x <= Utils.Osm.LngToTileX(pointTo); x++)
                for (var y = Utils.Osm.LatToTileY(pointFrom); y <= Utils.Osm.LatToTileY(pointTo); y++)
                {
                    tiles.Add(new Tile()
                    {
                        X = x,
                        Y = y,
                    });
                }

            try
            {
#if DEBUG
                Utils.Osm.SaveTilesToFile(Files.TempFileName("xxx.osm"), tiles);
#else
                if (ShowSaveFileDialog(SaveFileDialogType.Osm))
                {
                    Utils.Osm.SaveTilesToFile(saveFileDialog.FileName, tiles);
                }
#endif
            }
            catch (Exception e)
            {
                Utils.WriteError(e);

                Msg.Error(e.Message);
            }
        }

        private void SaveTileStatusToFile()
        {
            var pointFrom = gMapControl.FromLocalToLatLng(0, 0);
            var pointTo = gMapControl.FromLocalToLatLng(gMapControl.Width, gMapControl.Height);

            var tiles = new List<Tile>();

            for (var x = Utils.Osm.LngToTileX(pointFrom); x <= Utils.Osm.LngToTileX(pointTo); x++)
                for (var y = Utils.Osm.LatToTileY(pointFrom); y <= Utils.Osm.LatToTileY(pointTo); y++)
                {
                    tiles.Add(new Tile()
                    {
                        X = x,
                        Y = y,
                    });
                }
            foreach (var tile in tilesOverlay.Polygons.Cast<MapItemTile>())
            {
                //                tile.Selected = false;
            }

            try
            {
#if DEBUG
                Utils.Gpx.SaveTileStatusToFile(Files.TempFileName("xxx.gpx"), tiles);
#else
                if (ShowSaveFileDialog(SaveFileDialogType.Osm))
                {
                    Utils.Osm.SaveTilesToFile(saveFileDialog.FileName, tiles);
                }
#endif
            }
            catch (Exception e)
            {
                Utils.WriteError(e);

                Msg.Error(e.Message);
            }
        }
    }
}