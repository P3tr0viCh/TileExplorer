using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using static TileExplorer.Database;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public class MapItemMarkerCross : GMarkerCross, IMapItem
    {
        private const int DefaultWidth = 2;

        private readonly MapItem<Models.Marker> item;
        
        static MapItemMarkerCross()
        {
            DefaultPen.Width = DefaultWidth;
        }

        public MapItemMarkerCross(PointLatLng p) : base(p)
        {
            item = new MapItem<Models.Marker>(this, new Models.Marker());
        }

        public MapItemType Type => MapItemType.Marker;

        public Models.Marker Model { get => item.Model; set => item.Model = value; }
        Models.BaseId IMapItem.Model { get => Model; set => Model = (Models.Marker)value; }

        public bool Selected { get => item.Selected; set => item.Selected = value; }

        public void NotifyModelChanged()
        {
            Position = new PointLatLng(Model.Lat, Model.Lng);
        }

        public void UpdateColors()
        {
            throw new System.NotImplementedException();
        }
    }
}