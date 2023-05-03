using GMap.NET;
using GMap.NET.WindowsForms;
using System.Collections.Generic;
using System.Drawing;
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
                    colorFill = Color.FromArgb(Properties.Settings.Default.ColorTileVisitedAlpha, Properties.Settings.Default.ColorTileVisited);
                    colorStroke = Color.FromArgb(Properties.Settings.Default.ColorTileVisitedLineAlpha, Properties.Settings.Default.ColorTileVisited);
                    break;
                case TileStatus.Cluster:
                    colorFill = Color.FromArgb(Properties.Settings.Default.ColorTileClusterAlpha, Properties.Settings.Default.ColorTileCluster);
                    colorStroke = Color.FromArgb(Properties.Settings.Default.ColorTileClusterLineAlpha, Properties.Settings.Default.ColorTileCluster);
                    break;
                case TileStatus.MaxCluster:
                    colorFill = Color.FromArgb(Properties.Settings.Default.ColorTileMaxClusterAlpha, Properties.Settings.Default.ColorTileMaxCluster);
                    colorStroke = Color.FromArgb(Properties.Settings.Default.ColorTileMaxClusterLineAlpha, Properties.Settings.Default.ColorTileMaxCluster);
                    break;
                case TileStatus.MaxSquare:
                    colorFill = Color.FromArgb(Properties.Settings.Default.ColorTileMaxSquareAlpha, Properties.Settings.Default.ColorTileMaxSquare);
                    colorStroke = Color.FromArgb(Properties.Settings.Default.ColorTileMaxSquareLineAlpha, Properties.Settings.Default.ColorTileMaxSquare);
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