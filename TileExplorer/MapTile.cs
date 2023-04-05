using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh;
using System.Collections.Generic;
using System.Drawing;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    internal class MapTile : GMapPolygon
    {
        public MapTile(TileModel tile) : base(new List<PointLatLng>(), "")
        {
            Color colorFill;
            Color colorStroke;

            switch (tile.Status)
            {
                case TileStatus.Visited:
                    colorFill = Color.FromArgb(
                        Settings.Default.ColorTileVisitedAlpha, Settings.Default.ColorTileVisited);
                    colorStroke = Color.FromArgb(
                        Settings.Default.ColorTileVisitedLineAlpha, Settings.Default.ColorTileVisited);
                    break;
                case TileStatus.Cluster:
                    colorFill = Color.FromArgb(
                        Settings.Default.ColorTileClusterAlpha, Settings.Default.ColorTileCluster);
                    colorStroke = Color.FromArgb(
                        Settings.Default.ColorTileClusterLineAlpha, Settings.Default.ColorTileCluster);
                    break;
                case TileStatus.MaxCluster:
                    colorFill = Color.FromArgb(
                        Settings.Default.ColorTileMaxClusterAlpha, Settings.Default.ColorTileMaxCluster);
                    colorStroke = Color.FromArgb(
                        Settings.Default.ColorTileMaxClusterLineAlpha, Settings.Default.ColorTileMaxCluster);
                    break;
                case TileStatus.MaxSquare:
                    colorFill = Color.FromArgb(
                        Settings.Default.ColorTileMaxSquareAlpha, Settings.Default.ColorTileMaxSquare);
                    colorStroke = Color.FromArgb(
                        Settings.Default.ColorTileMaxSquareLineAlpha, Settings.Default.ColorTileMaxSquare);
                    break;
                default:
                    colorFill = Color.Empty;
                    colorStroke = Color.FromArgb(100, Color.Gray);
                    break;
            }

            double lat1 = Osm.TileYToLat(tile.Y, Const.TILE_ZOOM);
            double lng1 = Osm.TileXToLng(tile.X, Const.TILE_ZOOM);

            double lat2 = Osm.TileYToLat(tile.Y + 1, Const.TILE_ZOOM);
            double lng2 = Osm.TileXToLng(tile.X + 1, Const.TILE_ZOOM);

            Points.Add(new PointLatLng(lat1, lng1));
            Points.Add(new PointLatLng(lat1, lng2));
            Points.Add(new PointLatLng(lat2, lng2));
            Points.Add(new PointLatLng(lat2, lng1));

            Fill = new SolidBrush(colorFill);
            Stroke = new Pen(colorStroke, 1f);
        }
    }
}