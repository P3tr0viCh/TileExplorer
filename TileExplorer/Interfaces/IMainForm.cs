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

        Task UpdateDataAsync(DataLoad load = default, object value = null);
        Task UpdateDataAsync(DataLoad load, IEnumerable<object> list);

        void ListItemAdd(BaseId value);
        Task ListItemChangeAsync(IEnumerable<BaseId> list);
        Task ListItemDeleteAsync(IEnumerable<BaseId> list);

        Task TrackChangedAsync(Track track);
        Task TrackChangedAsync(IEnumerable<Track> tracks);
        Task MarkerChangedAsync(Marker marker);

        void ShowChartTrackEle(object sender, Track value);

        void ShowMarkerPosition(object sender, PointLatLng value);

        List<int> Years { get; }
    }
}