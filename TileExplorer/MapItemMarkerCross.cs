using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using System.Drawing;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class MapItemMarkerCross : GMarkerCross, IMapItem
    {
        private readonly MapItem<Models.MapMarker> item;

        public MapItemMarkerCross(PointLatLng p) : base(p)
        {
            item = new MapItem<Models.MapMarker>(this, new Models.MapMarker());
            
            Pen = new Pen(Brushes.Red, 2f);
        }

        public MapItemType Type => MapItemType.Marker;

        public Models.MapMarker Model { get => item.Model; set => item.Model = value; }
        Models.BaseId IMapItem.Model { get => Model; set => Model = (Models.MapMarker)value; }

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