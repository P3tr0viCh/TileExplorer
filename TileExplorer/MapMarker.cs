using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    internal class MapMarker : GMapMarker
    {
        private const int DEFAULT_SIZE = 20;

        private const int DEFAULT_IMAGE_SIZE = 6;

        public static readonly Pen DefaultStroke;

        [NonSerialized]
        public Pen Stroke = DefaultStroke;

        public static readonly Brush DefaultFill;

        [NonSerialized]
        public Brush Fill = DefaultFill;

        private MarkerModel markerModel;

        static MapMarker()
        {
            DefaultStroke = new Pen(Color.FromArgb(140, Color.MidnightBlue));
            DefaultFill = new SolidBrush(Color.FromArgb(222, Color.AliceBlue));
            DefaultStroke.Width = 1f;
            DefaultStroke.LineJoin = LineJoin.Round;
            DefaultStroke.StartCap = LineCap.RoundAnchor;
        }

        public MapMarker(MarkerModel markerModel) : base(new PointLatLng())
        {
            Size = new Size(DEFAULT_SIZE, DEFAULT_SIZE);
            Offset = new Point(-DEFAULT_SIZE / 2, -DEFAULT_SIZE / 2);

            Stroke = new Pen(Color.FromArgb(
                Settings.Default.ColorMarkerLineAlpha, Settings.Default.ColorMarkerLine));
            Fill = new SolidBrush(Color.FromArgb(
                Settings.Default.ColorMarkerFillAlpha, Settings.Default.ColorMarkerFill));

            ToolTip = new MapMarkerToolTip(this)
            {
                Font = Settings.Default.FontMarker,

                TextPadding = new Size(4, 4),

                Foreground = new SolidBrush(Color.FromArgb(
                    Settings.Default.ColorMarkerTextAlpha, Settings.Default.ColorMarkerText)),
                Stroke = new Pen(Color.FromArgb(
                    Settings.Default.ColorMarkerLineAlpha, Settings.Default.ColorMarkerLine)),
                Fill = new SolidBrush(Color.FromArgb(
                    Settings.Default.ColorMarkerTextFillAlpha, Settings.Default.ColorMarkerTextFill))
            };

            ToolTip.Stroke.Width = 1f;

            ToolTip.Format.LineAlignment = StringAlignment.Center;
            ToolTip.Format.Alignment = StringAlignment.Center;

            MarkerModel = markerModel;
        }

        public MarkerModel MarkerModel
        {
            get
            {
                return markerModel;
            }
            set
            {
                markerModel = value;

                NotifyModelChanged();
            }
        }

        public void NotifyModelChanged()
        {
            ToolTipText = markerModel.Text;
            ToolTipMode = markerModel.IsTextVisible ? MarkerTooltipMode.Always : MarkerTooltipMode.OnMouseOver;

            Position = new PointLatLng(markerModel.Lat, markerModel.Lng);

            ToolTip.Offset.X = markerModel.OffsetX;
            ToolTip.Offset.Y = markerModel.OffsetY;
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
    }
}