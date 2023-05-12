using GMap.NET;
using GMap.NET.WindowsForms;
using System.Collections.Generic;
using System.Drawing;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class MapTile : GMapPolygon, IMapItem
    {
        public MapItemType Type => MapItemType.Tile;

        private readonly MapItem<Models.Tile> item;

        public MapTile(Models.Tile tile) : base(new List<PointLatLng>(), "")
        {
            item = new MapItem<Models.Tile>(this, tile);

            NotifyModelChanged();
            UpdateColors();
        }

        public Models.Tile Model { get => item.Model; set => item.Model = value; }
        Models.BaseId IMapItem.Model { get => Model; set => Model = (Models.Tile)value; }

        public bool Selected { get => item.Selected; set => item.Selected = value; }

        public void NotifyModelChanged()
        {
            Name = string.Format("{0}x{1}", Model.X, Model.Y);

            Color colorFill;
            Color colorStroke;

            switch (Model.Status)
            {
                case TileStatus.Visited:
                    colorFill = Color.FromArgb(AppSettings.Default.ColorTileVisitedAlpha, AppSettings.Default.ColorTileVisited);
                    colorStroke = Color.FromArgb(AppSettings.Default.ColorTileVisitedLineAlpha, AppSettings.Default.ColorTileVisited);
                    break;
                case TileStatus.Cluster:
                    colorFill = Color.FromArgb(AppSettings.Default.ColorTileClusterAlpha, AppSettings.Default.ColorTileCluster);
                    colorStroke = Color.FromArgb(AppSettings.Default.ColorTileClusterLineAlpha, AppSettings.Default.ColorTileCluster);
                    break;
                case TileStatus.MaxCluster:
                    colorFill = Color.FromArgb(AppSettings.Default.ColorTileMaxClusterAlpha, AppSettings.Default.ColorTileMaxCluster);
                    colorStroke = Color.FromArgb(AppSettings.Default.ColorTileMaxClusterLineAlpha, AppSettings.Default.ColorTileMaxCluster);
                    break;
                case TileStatus.MaxSquare:
                    colorFill = Color.FromArgb(AppSettings.Default.ColorTileMaxSquareAlpha, AppSettings.Default.ColorTileMaxSquare);
                    colorStroke = Color.FromArgb(AppSettings.Default.ColorTileMaxSquareLineAlpha, AppSettings.Default.ColorTileMaxSquare);
                    break;
                case TileStatus.Selected:
                    colorFill = Color.FromArgb(AppSettings.Default.ColorTileTrackSelectedAlpha, AppSettings.Default.ColorTileTrackSelected);
                    colorStroke = Color.FromArgb(AppSettings.Default.ColorTileTrackSelectedLineAlpha, AppSettings.Default.ColorTileTrackSelected);
                    break;
                default:
                    colorFill = Color.Empty;
                    colorStroke = Color.FromArgb(100, Color.Gray);
                    break;
            }

            Points.Clear();
            Points.AddRange(Utils.TilePoints(Model));

            Fill = new SolidBrush(colorFill);
            Stroke = new Pen(colorStroke, 1f);
        }

        public void UpdateColors()
        {
        }
    }
}