using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Database;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using TileExplorer.Interfaces;
using static TileExplorer.Database;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public class MapItemMarkerCircle : GMapMarker, IMapItem
    {
        private readonly MapItem<Models.Marker> item;

        private const int DefaultSize = 14;

        private const int DefaultWidth = 2;

        public static SolidBrush DefaultFill = new SolidBrush(Color.Black);

        public static Pen DefaultStroke = new Pen(Color.White)
        {
            Width = DefaultWidth,
            LineJoin = LineJoin.Round,
            StartCap = LineCap.RoundAnchor
        };

        [NonSerialized]
        public Pen Stroke = DefaultStroke;

        [NonSerialized]
        public SolidBrush Fill = DefaultFill;

        public MapItemMarkerCircle(PointLatLng p) : base(p)
        {
            item = new MapItem<Models.Marker>(this, new Models.Marker());

            Size = new Size(DefaultSize, DefaultSize);
        }

        public MapItemType Type => MapItemType.Marker;

        public Models.Marker Model { get => item.Model; set => item.Model = value; }
        BaseId IMapItem.Model { get => Model; set => Model = (Models.Marker)value; }

        public bool Selected { get => item.Selected; set => item.Selected = value; }

        public void NotifyModelChanged()
        {
            Position = new PointLatLng(Model.Lat, Model.Lng);
        }

        public void UpdateColors()
        {
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