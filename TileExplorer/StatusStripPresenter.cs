using GMap.NET;
using P3tr0viCh;
using System.Windows.Forms;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal class StatusStripPresenter
    {
        public interface IStatusStripView
        {
            ToolStripStatusLabel StatusLabelZoom { get; }
            ToolStripStatusLabel StatusLabelTileId { get; }
            ToolStripStatusLabel StatusLabelPosition { get; }
            ToolStripStatusLabel StatusLabelMousePosition { get; }

            ToolStripStatusLabel StatusLabelTilesVisited { get; }
            ToolStripStatusLabel StatusLabelTilesMaxCluster { get; }
            ToolStripStatusLabel StatusLabelTilesMaxSquare { get; }
        }

        private readonly IStatusStripView view;

        public StatusStripPresenter(IStatusStripView view)
        {
            this.view = view;

            Zoom = 0;
            TileId = PointLatLng.Empty;
            Position = PointLatLng.Empty;
            MousePosition = new PointLatLng(0, 0);

            TilesVisited = 0;
            TilesMaxCluster = 0;
            TilesMaxSquare = 0;
        }

        public double Zoom
        {
            set
            {
                view.StatusLabelZoom.Text = string.Format(Resources.StatusZoom, value);
            }
        }

        public PointLatLng TileId
        {
            set
            {
                view.StatusLabelTileId.Text = string.Format(Resources.StatusTileId,
                    Osm.LngToTileX(value.Lng, Const.TILE_ZOOM), Osm.LatToTileY(value.Lat, Const.TILE_ZOOM));
            }
        }

        public PointLatLng Position
        {
            set
            {
                view.StatusLabelPosition.Text = string.Format(Resources.StatusPosition, value.Lat, value.Lng);
            }
        }

        public PointLatLng MousePosition
        {
            set
            {
                view.StatusLabelMousePosition.Text = string.Format(Resources.StatusMousePosition, value.Lat, value.Lng);
            }
        }

        public int TilesVisited
        {
            set
            {
                view.StatusLabelTilesVisited.Text = string.Format(Resources.StatusTilesVisited, value);
            }
        }

        public int TilesMaxCluster
        {
            set
            {
                view.StatusLabelTilesMaxCluster.Text = string.Format(Resources.StatusTilesMaxCluster, value);
            }
        }

        public int TilesMaxSquare
        {
            set
            {
                view.StatusLabelTilesMaxSquare.Text = string.Format(Resources.StatusTilesMaxSquare, value);
            }
        }
    }
}
