using GMap.NET;
using TileExplorer.Properties;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    internal class StatusStripPresenter
    {
        private readonly IStatusStripView view;

        public StatusStripPresenter(IStatusStripView view)
        {
            this.view = view;

            Zoom = 0;
            TileId = PointLatLng.Empty;
            Position = PointLatLng.Empty;
            MousePosition = new PointLatLng(0, 0);

            Status = string.Empty;
            UpdateStatus = string.Empty;

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
                view.GetLabel(StatusLabel.Zoom).Text = string.Format(Resources.StatusZoom, value);
            }
        }

        public PointLatLng TileId
        {
            set
            {
                view.GetLabel(StatusLabel.TileId).Text = string.Format(Resources.StatusTileId,
                    Utils.Osm.LngToTileX(value), Utils.Osm.LatToTileY(value));
            }
        }

        public PointLatLng Position
        {
            set
            {
                view.GetLabel(StatusLabel.Position).Text = 
                    string.Format(Resources.StatusPosition, value.Lat, value.Lng);
            }
        }

        public PointLatLng MousePosition
        {
            set
            {
                view.GetLabel(StatusLabel.MousePosition).Text = 
                    string.Format(Resources.StatusMousePosition, value.Lat, value.Lng);
            }
        }

        public string Status
        {
            set
            {
                view.GetLabel(StatusLabel.Status).Text = value;
            }
        }

        public string UpdateStatus
        {
            set
            {
                view.GetLabel(StatusLabel.UpdateStatus).Text = value;
            }
        }

        public int TracksCount
        {
            set
            {
                view.GetLabel(StatusLabel.TracksCount).Text = string.Format(Resources.StatusTracksCount, value);
            }
        }

        public double TracksDistance
        {
            set
            {
                view.GetLabel(StatusLabel.TracksDistance).Text = string.Format(Resources.StatusTracksDistance, value);
            }
        }

        public int TilesVisited
        {
            set
            {
                view.GetLabel(StatusLabel.TilesVisited).Text = string.Format(Resources.StatusTilesVisited, value);
            }
        }

        public int TilesMaxCluster
        {
            set
            {
                view.GetLabel(StatusLabel.TilesMaxCluster).Text = string.Format(Resources.StatusTilesMaxCluster, value);
            }
        }

        public int TilesMaxSquare
        {
            set
            {
                view.GetLabel(StatusLabel.TilesMaxSquare).Text = string.Format(Resources.StatusTilesMaxSquare, value);
            }
        }
    }
}