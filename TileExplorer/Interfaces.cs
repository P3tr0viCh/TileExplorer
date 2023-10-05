using System.Windows.Forms;
using static TileExplorer.Database;
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

            void ChangeMapItem(object sender, BaseId value);

            void MarkerChanged(Marker marker);

            ProgramStatus Status { set; }
        }

        public interface IChildForm
        {
            IMainForm MainForm { get; }

            ChildFormType ListType { get; }
        }

        public interface IMapItem
        {
            MapItemType Type { get; }

            Models.BaseId Model { get; set; }

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