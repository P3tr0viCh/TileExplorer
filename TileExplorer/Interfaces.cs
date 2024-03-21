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

            void ListItemAdd(object sender, BaseId value);
            void ListItemChange(object sender, BaseId value);
            void ListItemDelete(object sender, BaseId value);

            void MarkerChanged(Marker marker);

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