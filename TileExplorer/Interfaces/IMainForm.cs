using GMap.NET;
using P3tr0viCh.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer.Interfaces
{
    public interface IMainForm
    {
        void ChildFormOpened(IChildForm frm);
        void ChildFormClosed(IChildForm frm);

        Task SelectMapItemAsync(object sender, BaseId value);

        Task UpdateDataAsync(DataLoad load);

        PointLatLng MapCenter { get; }

        Task<bool> MarkerChangeAsync(Marker marker);
        Task MarkerChangedAsync(Marker marker);
        void MarkersDeleted(IEnumerable<Marker> markers);

        Task<bool> TrackChangeAsync(List<Track> tracks);
        Task TrackChangedAsync(IEnumerable<Track> tracks);
        Task TracksDeletedAsync(IEnumerable<Track> tracks);

        void ShowMarkerPosition(PointLatLng value);
        void ShowMarkerNewPosition(PointLatLng value);
    }
}