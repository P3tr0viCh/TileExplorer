using GMap.NET;
using GMap.NET.WindowsForms;
using System.Collections.Generic;
using System.Drawing;
using static TileExplorer.Database;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public class MapItemTile : GMapPolygon, IMapItem
    {
        public MapItemType Type => MapItemType.Tile;

        private readonly MapItem<Models.Tile> item;

        public MapItemTile(Models.Tile tile) : base(new List<PointLatLng>(), "")
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
            Name = $"{Model.X}x{Model.Y}";

            Points.Clear();

            Points.AddRange(Utils.Tiles.TilePoints(Model));
        }

        public void UpdateColors()
        {
            Color colorFill;
            Color colorStroke;
            
            if (Selected)
            {
                colorFill = Color.FromArgb(AppSettings.Roaming.Default.ColorTileTrackSelectedAlpha, AppSettings.Roaming.Default.ColorTileTrackSelected);
                colorStroke = Color.FromArgb(AppSettings.Roaming.Default.ColorTileTrackSelectedLineAlpha, AppSettings.Roaming.Default.ColorTileTrackSelected);
            }
            else
            {
                switch (Model.Status)
                {
                    case TileStatus.Visited:
                        colorFill = Color.FromArgb(AppSettings.Roaming.Default.ColorTileVisitedAlpha, AppSettings.Roaming.Default.ColorTileVisited);
                        colorStroke = Color.FromArgb(AppSettings.Roaming.Default.ColorTileVisitedLineAlpha, AppSettings.Roaming.Default.ColorTileVisited);
                        break;
                    case TileStatus.Cluster:
                        colorFill = Color.FromArgb(AppSettings.Roaming.Default.ColorTileClusterAlpha, AppSettings.Roaming.Default.ColorTileCluster);
                        colorStroke = Color.FromArgb(AppSettings.Roaming.Default.ColorTileClusterLineAlpha, AppSettings.Roaming.Default.ColorTileCluster);
                        break;
                    case TileStatus.MaxCluster:
                        colorFill = Color.FromArgb(AppSettings.Roaming.Default.ColorTileMaxClusterAlpha, AppSettings.Roaming.Default.ColorTileMaxCluster);
                        colorStroke = Color.FromArgb(AppSettings.Roaming.Default.ColorTileMaxClusterLineAlpha, AppSettings.Roaming.Default.ColorTileMaxCluster);
                        break;
                    case TileStatus.MaxSquare:
                        colorFill = Color.FromArgb(AppSettings.Roaming.Default.ColorTileMaxSquareAlpha, AppSettings.Roaming.Default.ColorTileMaxSquare);
                        colorStroke = Color.FromArgb(AppSettings.Roaming.Default.ColorTileMaxSquareLineAlpha, AppSettings.Roaming.Default.ColorTileMaxSquare);
                        break;
                    default:
                        colorFill = Color.Empty;
                        colorStroke = Color.FromArgb(100, Color.Gray);
                        break;
                }

                colorFill = Color.FromArgb(Model.HeatmapValue, Color.Red);
                colorStroke = Color.FromArgb(AppSettings.Roaming.Default.ColorTileVisitedAlpha, Color.Red);
            }

            Fill = new SolidBrush(colorFill);
            Stroke = new Pen(colorStroke, 1f);
        }
    }
}