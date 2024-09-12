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

            void SelectMapItem(object sender, BaseId value);

            Task ListItemAdd(object sender, BaseId value);
            Task ListItemChange(object sender, BaseId value);
            Task ListItemDelete(object sender, BaseId value);

            Task TrackChanged(Track track);
            Task MarkerChanged(Marker marker);

            void ShowChartTrackEle(object sender, Track value);

            void ShowMarkerPosition(object sender, PointLatLng value);

            ProgramStatus ProgramStatus { get; }
        }

        public interface IChildForm
        {
            IMainForm MainForm { get; }

            ChildFormType ChildFormType { get; }

            void UpdateSettings();
        }

        public interface IUpdateDataForm
        {
            void UpdateData();
        }

        public interface IListForm
        {
            void SetSelected(BaseId value);
        }

        public interface IMapItem
        {
            MapItemType Type { get; }

            BaseId Model { get; set; }

            bool Selected { get; set; }

            void UpdateColors();
            void NotifyModelChanged();
        }

        public interface IStatusStripView
        {
            ToolStripStatusLabel GetLabel(StatusLabel label);
        }
    }
}