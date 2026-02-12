using P3tr0viCh.Database;

namespace TileExplorer.Interfaces
{
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