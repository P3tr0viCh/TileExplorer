using GMap.NET;
using P3tr0viCh.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public class Interfaces
    {
        public interface IMainForm
        {
            void ChildFormOpened(object sender);
            void ChildFormClosed(object sender);

            Task SelectMapItemAsync(object sender, BaseId value);

            void ListItemAdd(object sender, BaseId value);
            Task ListItemChangeAsync(object sender, IEnumerable<BaseId> list);
            Task ListItemDeleteAsync(object sender, IEnumerable<BaseId> list);

            Task TrackChangedAsync(Track track);
            Task TrackChangedAsync(IEnumerable<Track> tracks);
            Task MarkerChangedAsync(Marker marker);
            Task EquipmentChangedAsync(Equipment equipment);

            void ShowChartTrackEle(object sender, Track value);

            void ShowMarkerPosition(object sender, PointLatLng value);

            ProgramStatus ProgramStatus { get; }

            List<int> Years { get; }
        }

        public interface IChildForm
        {
            IMainForm MainForm { get; }

            ChildFormType FormType { get; }

            void UpdateSettings();
        }

        public interface IUpdateDataForm : IChildForm
        {
            Task UpdateDataAsync();
        }

        public interface IListForm
        {
            void ListItemChange(BaseId value);
            void ListItemDelete(BaseId value);

            int Count { get; }

            void SetSelected(BaseId value);
        }

        public interface IMapItem
        {
            MapItemType Type { get; }

            BaseId Model { get; set; }

            bool Selected { get; set; }

            bool IsVisible { get; set; }

            void UpdateColors();
            void NotifyModelChanged();
        }
    }
}