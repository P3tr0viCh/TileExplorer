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
            ToolStripStatusLabel LabelZoom { get; }
            ToolStripStatusLabel LabelTileId { get; }
            ToolStripStatusLabel LabelPosition { get; }
            ToolStripStatusLabel LabelMousePosition { get; }

            ToolStripStatusLabel LabelStatus { get; }

            ToolStripStatusLabel LabelTracksCount { get; }
            ToolStripStatusLabel LabelTracksDistance { get; }

            ToolStripStatusLabel LabelTilesVisited { get; }
            ToolStripStatusLabel LabelTilesMaxCluster { get; }
            ToolStripStatusLabel LabelTilesMaxSquare { get; }
        }

        private readonly IStatusStripView view;

        public StatusStripPresenter(IStatusStripView view)
        {
            this.view = view;

            Zoom = 0;
            TileId = PointLatLng.Empty;
            Position = PointLatLng.Empty;
            MousePosition = new PointLatLng(0, 0);

            Status = string.Empty;

            TracksCount = 0;
            TracksDistance = 0;

            TilesVisited = 0;
            TilesMaxCluster = 0;
            TilesMaxSquare = 0;
        }

        public double Zoom
        {
            set
            {
                view.LabelZoom.Text = string.Format(Resources.StatusZoom, value);
            }
        }

        public PointLatLng TileId
        {
            set
            {
                view.LabelTileId.Text = string.Format(Resources.StatusTileId,
                    Osm.LngToTileX(value.Lng, Const.TILE_ZOOM), Osm.LatToTileY(value.Lat, Const.TILE_ZOOM));
            }
        }

        public PointLatLng Position
        {
            set
            {
                view.LabelPosition.Text = string.Format(Resources.StatusPosition, value.Lat, value.Lng);
            }
        }

        public PointLatLng MousePosition
        {
            set
            {
                view.LabelMousePosition.Text = string.Format(Resources.StatusMousePosition, value.Lat, value.Lng);
            }
        }

        public string Status
        {
            set
            {
                view.LabelStatus.Text = value;
            }
        }

        public int TracksCount
        {
            set
            {
                view.LabelTracksCount.Text = string.Format(Resources.StatusTracksCount, value);
            }
        }

        public double TracksDistance
        {
            set
            {
                view.LabelTracksDistance.Text = string.Format(Resources.StatusTracksDistance, value);
            }
        }

        public int TilesVisited
        {
            set
            {
                view.LabelTilesVisited.Text = string.Format(Resources.StatusTilesVisited, value);
            }
        }

        public int TilesMaxCluster
        {
            set
            {
                view.LabelTilesMaxCluster.Text = string.Format(Resources.StatusTilesMaxCluster, value);
            }
        }

        public int TilesMaxSquare
        {
            set
            {
                view.LabelTilesMaxSquare.Text = string.Format(Resources.StatusTilesMaxSquare, value);
            }
        }
    }
}
