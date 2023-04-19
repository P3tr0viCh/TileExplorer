using static TileExplorer.Database;

namespace TileExplorer
{
    public enum MapItemType
    {
        Marker,
        Track,
        Tile
    }

    public interface IMapItem
    {
        MapItemType Type { get; }

        BaseModelId Model { get; set; }

        bool Selected { get; set; }

        void UpdateColors();
        void NotifyModelChanged();
    }

    public class MapItem<T>
    {
        private readonly IMapItem parent;

        public MapItem(IMapItem parent, T model)
        {
            this.parent = parent;
            this.model = model;
        }

        private T model;
        public T Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;

                parent.NotifyModelChanged();
            }
        }

        private bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == value) return;

                selected = value;

                parent.UpdateColors();
            }
        }
    }
}