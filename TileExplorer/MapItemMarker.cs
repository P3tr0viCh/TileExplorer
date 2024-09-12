using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using static TileExplorer.Database;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public class MapItemMarker : GMapMarker, IMapItem
    {
        public MapItemType Type => MapItemType.Marker;

        private const int DefaultSize = 6;

        private const int DefaultWidth = 1;
        private const int DefaultWidthSelected = 2;

        public static Pen DefaultStroke = new Pen(Color.Green)
        {
            Width = DefaultWidth,
            LineJoin = LineJoin.Round,
            StartCap = LineCap.RoundAnchor
        };

        public static Pen DefaultSelectedStroke = new Pen(Color.Red)
        {
            Width = DefaultWidthSelected,
            LineJoin = LineJoin.Round,
            StartCap = LineCap.RoundAnchor
        };

        public static SolidBrush DefaultFill = new SolidBrush(Color.GreenYellow);

        public static SolidBrush DefaultSelectedFill = new SolidBrush(Color.DarkBlue);

        [NonSerialized]
        public Pen Stroke = DefaultStroke;

        [NonSerialized]
        public Pen SelectedStroke = DefaultSelectedStroke;

        [NonSerialized]
        public SolidBrush Fill = DefaultFill;

        [NonSerialized]
        public SolidBrush SelectedFill = DefaultSelectedFill;

        private readonly MapItem<Models.Marker> item;

        public MapItemMarker(Models.Marker marker) : base(new PointLatLng())
        {
            item = new MapItem<Models.Marker>(this, marker);

            Size = new Size(DefaultSize, DefaultSize);

            ToolTip = new MapItemMarkerToolTip(this);

            NotifyModelChanged();

            UpdateColors();
        }

        public Models.Marker Model { get => item.Model; set => item.Model = value; }
        Models.BaseId IMapItem.Model { get => Model; set => Model = (Models.Marker)value; }

        public bool Selected { get => item.Selected; set => item.Selected = value; }

        public void NotifyModelChanged()
        {
            ToolTipText = Model.Text;
            ToolTipMode = Model.IsTextVisible ? MarkerTooltipMode.Always : MarkerTooltipMode.OnMouseOver;

            Position = new PointLatLng(Model.Lat, Model.Lng);

            ToolTip.Offset.X = Model.OffsetX;
            ToolTip.Offset.Y = Model.OffsetY;
        }

        public void UpdateColors()
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

        public override void OnRender(Graphics g)
        {
            var rectangle = new Rectangle(
                LocalPosition.X - Size.Width / 2,
                LocalPosition.Y - Size.Height / 2,
                Size.Width, Size.Height);

            g.FillEllipse(Fill, rectangle);

            g.DrawEllipse(Stroke, rectangle);
        }
    }
}