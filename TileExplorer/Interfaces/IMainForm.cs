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

        Task UpdateDataAsync(DataLoad load = default);

        Task<bool> ListItemAddAsync(BaseId value);
        Task<bool> ListItemChangeAsync(IEnumerable<BaseId> list);

        PointLatLng MapCenter { get; }

        Task<bool> MarkerChangeAsync(Marker marker);

        Task TrackChangedAsync(Track track);
        Task TrackChangedAsync(IEnumerable<Track> tracks);
        Task MarkerChangedAsync(Marker marker);

        void ShowMarkerPosition(PointLatLng value);
        void ShowMarkerNewPosition(PointLatLng value);
    }
}