using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Database;
using System.Collections.Generic;
using System.Drawing;
using TileExplorer.Interfaces;
using static TileExplorer.Database;

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
        BaseId IMapItem.Model { get => Model; set => Model = (Models.Tile)value; }

        public bool Selected { get => item.Selected; set => item.Selected = value; }

        private bool heatmap;
        public bool Heatmap
        {
            get => heatmap;
            set
            {
                heatmap = value;

                UpdateColors();
            }
        }

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
                colorFill = Color.FromArgb(AppSettings.Roaming.ColorTileTrackSelectedAlpha, AppSettings.Roaming.ColorTileTrackSelected);
                colorStroke = Color.FromArgb(AppSettings.Roaming.ColorTileTrackSelectedLineAlpha, AppSettings.Roaming.ColorTileTrackSelected);
            }
            else
            {
                if (Heatmap)
                {
                    colorFill = Color.FromArgb(Model.HeatmapValue, AppSettings.Roaming.ColorTileHeatmap);
                    colorStroke = Color.FromArgb(AppSettings.Roaming.ColorTileHeatmapLineAlpha, AppSettings.Roaming.ColorTileHeatmap);
                }
                else
                {
                    switch (Model.Status)
                    {
                        case TileStatus.Visited:
                            colorFill = Color.FromArgb(AppSettings.Roaming.ColorTileVisitedAlpha, AppSettings.Roaming.ColorTileVisited);
                            colorStroke = Color.FromArgb(AppSettings.Roaming.ColorTileVisitedLineAlpha, AppSettings.Roaming.ColorTileVisited);
                            break;
                        case TileStatus.Cluster:
                            colorFill = Color.FromArgb(AppSettings.Roaming.ColorTileClusterAlpha, AppSettings.Roaming.ColorTileCluster);
                            colorStroke = Color.FromArgb(AppSettings.Roaming.ColorTileClusterLineAlpha, AppSettings.Roaming.ColorTileCluster);
                            break;
                        case TileStatus.MaxCluster:
                            colorFill = Color.FromArgb(AppSettings.Roaming.ColorTileMaxClusterAlpha, AppSettings.Roaming.ColorTileMaxCluster);
                            colorStroke = Color.FromArgb(AppSettings.Roaming.ColorTileMaxClusterLineAlpha, AppSettings.Roaming.ColorTileMaxCluster);
                            break;
                        case TileStatus.MaxSquare:
                            colorFill = Color.FromArgb(AppSettings.Roaming.ColorTileMaxSquareAlpha, AppSettings.Roaming.ColorTileMaxSquare);
                            colorStroke = Color.FromArgb(AppSettings.Roaming.ColorTileMaxSquareLineAlpha, AppSettings.Roaming.ColorTileMaxSquare);
                            break;
                        default:
                            colorFill = Color.Empty;
                            colorStroke = Color.FromArgb(100, Color.Gray);
                            break;
                    }
                }
            }

            Fill = new SolidBrush(colorFill);
            Stroke = new Pen(colorStroke, 1f);
        }
    }
}