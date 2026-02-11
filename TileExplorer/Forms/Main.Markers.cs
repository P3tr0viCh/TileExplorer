using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class Main
    {
        private bool markersLoaded = false;

        private readonly MapItemMarkerCircle markerPosition = new MapItemMarkerCircle(default)
        {
            IsVisible = false
        };

        private readonly GMapMarker markerNewPosition = new MapItemMarkerCross(default)
        {
            IsVisible = false
        };

        private MapItemMarker MapItemMarkerNewInstance(Marker marker)
        {
            var item = new MapItemMarker(marker);

            item.ToolTip.Font = AppSettings.Roaming.Default.FontMarker;

            return item;
        }

        private readonly WrapperCancellationTokenSource ctsMarkers = new WrapperCancellationTokenSource();

        private async Task LoadMarkersAsync()
        {
            DebugWrite.Line("start");

            ctsMarkers.Start();

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                markersLoaded = false;

                overlayMarkers.Clear();

                var markers = await Database.Default.ListLoadAsync<Marker>();

                foreach (var marker in markers)
                {
                    // await Task.Delay(3000, ctsMarkers.Token);

                    if (ctsMarkers.IsCancellationRequested) return;

                    overlayMarkers.Markers.Add(MapItemMarkerNewInstance(marker));
                }

                markersLoaded = true;
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadListMarkersFail, e.Message);
            }
            finally
            {
                ctsMarkers.Finally();

                ProgramStatus.Default.Stop(status);

                DebugWrite.Line("end");
            }
        }

        public void ShowMarkerPosition(PointLatLng value)
        {
            markerPosition.Position = value;

            markerPosition.IsVisible = value != default;
        }

        public void ShowMarkerNewPosition(PointLatLng value)
        {
            markerNewPosition.Position = value;

            markerNewPosition.IsVisible = value != default;
        }

        public async Task<bool> MarkerChangeAsync(Marker marker)
        {
            if (marker == null) return false;

            if (!FrmMarker.ShowDlg(this, marker)) return false;

            await SelectMapItemAsync(this, marker);

            return true;
        }

        public async Task MarkerChangedAsync(Marker marker)
        {
            var mapItem = FindMapItem(marker);

            if (mapItem == null)
            {
                overlayMarkers.Markers.Add(MapItemMarkerNewInstance(marker));
            }
            else
            {
                mapItem.Model = marker;

                mapItem.NotifyModelChanged();

                gMapControl.Invalidate();
            }

            Utils.Forms.ChildFormsListItemsChange(ChildFormType.MarkerList, new List<Marker>() { marker });

            await SelectMapItemAsync(this, marker);
        }

        private async Task<bool> MarkerDeleteAsync(Marker marker)
        {
            var name = marker.Text;

            if (name.IsEmpty())
            {
                name = $"{marker.Lat}:{marker.Lng}";
            }

            if (!Msg.Question(Resources.QuestionMarkerDelete, name)) return false;

            if (!await Database.Actions.ListItemDeleteAsync(marker)) return false;

            var markers = new List<Marker>() { marker };

            MarkersDeleted(markers);

            return true;
        }

        public void MarkersDeleted(IEnumerable<Marker> markers)
        {
            foreach (var marker in markers)
            {
                overlayMarkers.Markers.Remove(
                    overlayMarkers.Markers.Cast<MapItemMarker>().Where(i => i.Model.Id == marker.Id)?
                        .FirstOrDefault());
            }

            Utils.Forms.ChildFormsListItemsDelete(ChildFormType.MarkerList, markers);

            Selected = null;
        }
    }
}