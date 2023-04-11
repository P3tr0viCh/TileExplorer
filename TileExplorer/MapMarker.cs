using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class MapMarker : GMapMarker
    {
        private const int DEFAULT_SIZE = 20;

        private const int DEFAULT_IMAGE_SIZE = 6;

        public static readonly Pen DefaultStroke;
        public static readonly Pen DefaultSelectedStroke;

        public static readonly SolidBrush DefaultFill;
        public static readonly SolidBrush DefaultSelectedFill;

        [NonSerialized]
        public Pen Stroke = DefaultStroke;

        [NonSerialized]
        public Pen SelectedStroke = DefaultSelectedStroke;

        [NonSerialized]
        public SolidBrush Fill = DefaultFill;

        [NonSerialized]
        public SolidBrush SelectedFill = DefaultSelectedFill;

        private MarkerModel marker;

        static MapMarker()
        {
            DefaultStroke = new Pen(Color.FromArgb(Settings.Default.ColorMarkerLineAlpha, Settings.Default.ColorMarkerLine))
            {
                Width = Settings.Default.WidthMarkerLine,
                LineJoin = LineJoin.Round,
                StartCap = LineCap.RoundAnchor
            };

            DefaultSelectedStroke = new Pen(Color.FromArgb(Settings.Default.ColorMarkerLineAlpha, Settings.Default.ColorMarkerSelectedLine))
            {
                Width = Settings.Default.WidthMarkerLineSelected,
                LineJoin = LineJoin.Round,
                StartCap = LineCap.RoundAnchor
            };

            DefaultFill = new SolidBrush(Color.FromArgb(Settings.Default.ColorMarkerFillAlpha, Settings.Default.ColorMarkerFill));

            DefaultSelectedFill = new SolidBrush(Color.FromArgb(Settings.Default.ColorMarkerFillAlpha, Settings.Default.ColorMarkerSelectedFill));
        }

        public MapMarker(MarkerModel markerModel) : base(new PointLatLng())
        {
            Size = new Size(DEFAULT_SIZE, DEFAULT_SIZE);
            Offset = new Point(-DEFAULT_SIZE / 2, -DEFAULT_SIZE / 2);

            ToolTip = new MapMarkerToolTip(this);

            Marker = markerModel;

            UpdateColors();
        }

        public MarkerModel Marker
        {
            get
            {
                return marker;
            }
            set
            {
                marker = value;

                NotifyModelChanged();
            }
        }

        public void NotifyModelChanged()
        {
            ToolTipText = marker.Text;
            ToolTipMode = marker.IsTextVisible ? MarkerTooltipMode.Always : MarkerTooltipMode.OnMouseOver;

            Position = new PointLatLng(marker.Lat, marker.Lng);

            ToolTip.Offset.X = marker.OffsetX;
            ToolTip.Offset.Y = marker.OffsetY;
        }

        public override void OnRender(Graphics g)
        {
            Rectangle rectangle = new Rectangle(
                LocalPosition.X + (DEFAULT_SIZE - DEFAULT_IMAGE_SIZE) / 2,
                LocalPosition.Y + (DEFAULT_SIZE - DEFAULT_IMAGE_SIZE) / 2,
                DEFAULT_IMAGE_SIZE, DEFAULT_IMAGE_SIZE);

            g.FillEllipse(Fill, rectangle);

            g.DrawEllipse(Stroke, rectangle);
        }

        private void UpdateColors()
        {
            if (Selected)
            {
                Fill = DefaultSelectedFill;
                Stroke = DefaultSelectedStroke;

                ToolTip.Stroke = DefaultSelectedStroke;

            }
            else
            {
                Fill = DefaultFill;
                Stroke = DefaultStroke;

                ToolTip.Stroke = DefaultStroke;
            }
        }

        private bool selected = false;

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == value) return;

                selected = value;

                UpdateColors();
            }
        }
    }
}