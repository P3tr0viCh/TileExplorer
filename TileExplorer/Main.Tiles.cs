﻿using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private bool tilesLoaded = false;

        private readonly WrapperCancellationTokenSource ctsTiles = new WrapperCancellationTokenSource();

        private void CalcTiles(List<Tile> tiles)
        {
            var calcResult = Utils.Tiles.CalcTiles(tiles);

            statusStripPresenter.TilesVisited = calcResult.Visited;
            statusStripPresenter.TilesMaxCluster = calcResult.MaxCluster;
            statusStripPresenter.TilesMaxSquare = calcResult.MaxSquare;
        }

        private async Task LoadTilesAsync()
        {
            DebugWrite.Line("start");

            ctsTiles.Start();

            var status = ProgramStatus.Start(Status.LoadData);

            try
            {
                tilesLoaded = false;

                overlayTiles.Clear();

                var tiles = await Database.Default.ListLoadAsync<Tile>();

                CalcTiles(tiles);

                tiles.ForEach(tile =>
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
                });

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
                ctsTiles.Finally();

                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }
    }
}