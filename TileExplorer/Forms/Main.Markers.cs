using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
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

            var status = ProgramStatus.Start(Status.LoadData);

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

                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }

        private void MarkerAdd(Marker marker)
        {
            var prevMarkersVisible = overlayMarkers.IsVisibile;

            overlayMarkers.IsVisibile = true;

#if DEBUG
            marker.Text = DateTime.Now.ToString();
#endif

            markerNewPosition.Position = new PointLatLng(marker.Lat, marker.Lng);
            markerNewPosition.IsVisible = true;

            FrmMarker.ShowDlg(this, marker);

            markerNewPosition.IsVisible = false;

            overlayMarkers.IsVisibile = prevMarkersVisible;
        }

        private async Task MarkerChangeAsync(Marker marker)
        {
            if (marker == null) return;

            FrmMarker.ShowDlg(this, marker);

            await SelectMapItemAsync(this, marker);
        }

        public async Task MarkerChangedAsync(Marker marker)
        {
            var mapItem = FindMapItem(marker);

            if (mapItem == null)
            {
                markerNewPosition.IsVisible = false;

                overlayMarkers.Markers.Add(MapItemMarkerNewInstance(marker));
            }
            else
            {
                mapItem.Model = marker;

                mapItem.NotifyModelChanged();

                gMapControl.Invalidate();
            }

            await UpdateDataAsync(DataLoad.ObjectChange, marker);

            await SelectMapItemAsync(this, marker);
        }

        private async Task MarkerDeleteAsync(List<Marker> markers)
        {
            if (markers?.Count == 0) return;

            var firstMarker = markers.FirstOrDefault();

            var name = firstMarker.Text;

            if (name.IsEmpty())
            {
                name = $"{firstMarker.Lat}:{firstMarker.Lng}";
            }

            var question = markers.Count == 1 ? Resources.QuestionMarkerDelete : Resources.QuestionMarkersDelete;

            if (!Msg.Question(question, name, markers.Count - 1)) return;

            if (!await Database.Actions.MarkerDeleteAsync(markers)) return;

            foreach (var marker in markers)
            {
                overlayMarkers.Markers.Remove(
                    overlayMarkers.Markers.Cast<IMapItem>().Where(i => i.Model.Id == marker.Id)?
                        .Cast<MapItemMarker>().FirstOrDefault());

                await UpdateDataAsync(DataLoad.ObjectDelete, marker);
            }

            Selected = null;
        }

        private async Task MarkerDeleteAsync(Marker marker)
        {
            await MarkerDeleteAsync(new List<Marker>() { marker });
        }
    }
}