using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class Main
    {
        private MapItemMarker MapItemMarkerNewInstance(Marker marker)
        {
            var item = new MapItemMarker(marker);

            item.ToolTip.Font = AppSettings.Roaming.Default.FontMarker;

            return item;
        }

        private async Task LoadMarkersAsync()
        {
            DebugWrite.Line("start");

            overlayMarkers.Clear();

            var markers = await Database.Default.ListLoadAsync<Marker>();

            foreach (var marker in markers)
            {
                overlayMarkers.Markers.Add(MapItemMarkerNewInstance(marker));
            }

            DebugWrite.Line("end");
        }

        public MapItemMarker SelectedMarker
        {
            get
            {
                if (Selected == null) return null;

                if (Selected.Type != MapItemType.Marker) return null;

                return (MapItemMarker)selected;
            }
        }

        private readonly GMapMarker markerNewPosition = new MapItemMarkerCross(default);

        private void MarkerAdd(Marker marker)
        {
            bool prevMarkersVisible = overlayMarkers.IsVisibile;

            overlayMarkers.IsVisibile = true;

#if DEBUG
            marker.Text = DateTime.Now.ToString();
#endif

            markerNewPosition.Position = new PointLatLng(marker.Lat, marker.Lng);

            overlayMarkers.Markers.Add(markerNewPosition);

            FrmMarker.ShowDlg(this, marker);

            overlayMarkers.Markers.Remove(markerNewPosition);

            overlayMarkers.IsVisibile = prevMarkersVisible;
        }

        private void MarkerChange(Marker marker)
        {
            if (marker == null) return;

            FrmMarker.ShowDlg(this, marker);

            SelectMapItem(this, marker);
        }

        public void MarkerChanged(Marker marker)
        {
            var mapItem = FindMapItem(marker);

            if (mapItem == null)
            {
                overlayMarkers.Markers.Remove(markerNewPosition);

                overlayMarkers.Markers.Add(MapItemMarkerNewInstance(marker));
            }
            else
            {
                mapItem.Model = marker;

                mapItem.NotifyModelChanged();
            }

            Utils.GetFrmList(ChildFormType.MarkerList)?.ListItemChange(marker);

            SelectMapItem(this, marker);
        }

        private void MarkerDelete(Marker marker)
        {
            if (marker == null) return;

            var name = marker.Text;

            if (name.IsEmpty())
            {
                name = marker.Lat.ToString() + ":" + marker.Lng.ToString();
            }

            if (!Msg.Question(string.Format(Resources.QuestionMarkerDelete, name))) return;

            if (Database.Actions.MarkerDeleteAsync(marker).Result)
            {
                overlayMarkers.Markers.Remove(
                    overlayMarkers.Markers.Cast<IMapItem>().Where(i => i.Model.Id == marker.Id)?
                        .Cast<MapItemMarker>().FirstOrDefault());

                Utils.GetFrmList(ChildFormType.MarkerList)?.ListItemDelete(marker);

                Selected = null;
            }
        }
    }
}