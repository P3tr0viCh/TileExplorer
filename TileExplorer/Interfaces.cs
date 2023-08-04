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

            void SelectMapItemById(object sender, long id);

            void ChangeMapItemById(object sender, long id);

            void MarkerChanged(MapMarker marker);

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
    }
}