using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private bool tilesLoaded = false;

        private CancellationTokenSource ctsTiles;

        private async Task CalcTilesAsync(List<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.Status = TileStatus.Visited;
            }

            var calcResult = await Task.Run(() =>
            {
                return Utils.Tiles.CalcTiles(tiles);
            }, ctsTiles.Token);

            this.InvokeIfNeeded(() =>
            {
                statusStripPresenter.TilesVisited = calcResult.Visited;
                statusStripPresenter.TilesMaxCluster = calcResult.MaxCluster;
                statusStripPresenter.TilesMaxSquare = calcResult.MaxSquare;
            });
        }

        private async Task LoadTilesAsync()
        {
            DebugWrite.Line("start");

            try
            {
                tilesLoaded = false;

                overlayTiles.Clear();

                var tiles = await Database.Default.ListLoadAsync<Tile>();

                await CalcTilesAsync(tiles);

                foreach (var tile in tiles)
                {
                    if (ctsTiles.IsCancellationRequested) return;

                    overlayTiles.Polygons.Add(new MapItemTile(tile));

#if DEBUG && CHECK_TILES
                if (tile.Status > TileStatus.Visited)
                {
                    markersOverlay.Markers.Add(new MapMarker(new MarkerModel()
                    {
                        Lat = Osm.TileYToLat(tile.Y, Const.TILE_ZOOM),
                        Lng = Osm.TileXToLng(tile.X, Const.TILE_ZOOM),
                        Text = tile.ClusterId.ToString(),
                        IsTextVisible = true
                    }));
                }
#endif
                }

                tilesLoaded = true;
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadTilesFail, e.Message);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private void LoadTilesStop()
        {
            ctsTiles?.Cancel();
            ctsTiles?.Dispose();
        }

        private void LoadTiles()
        {
            LoadTilesStop();

            ctsTiles = new CancellationTokenSource();

            Task.Run(() => this.InvokeIfNeeded(async () => await LoadTilesAsync()), ctsTiles.Token);
        }
    }
}