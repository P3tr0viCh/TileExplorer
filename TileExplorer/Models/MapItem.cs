using TileExplorer.Interfaces;

namespace TileExplorer
{
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
            get => model;
            set
            {
                model = value;

                parent.NotifyModelChanged();
            }
        }

        private bool selected = false;
        public bool Selected
        {
            get => selected;
            set
            {
                if (selected == value) return;

                selected = value;

                parent.UpdateColors();
            }
        }
    }
}