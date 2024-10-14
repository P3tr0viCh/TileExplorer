using GMap.NET;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            Task ListItemChangeAsync(object sender, BaseId value);
            Task ListItemDeleteAsync(object sender, BaseId value);

            Task TrackChangedAsync(Track track);
            Task MarkerChangedAsync(Marker marker);

            void ShowChartTrackEle(object sender, Track value);

            void ShowMarkerPosition(object sender, PointLatLng value);

            ProgramStatus ProgramStatus { get; }
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