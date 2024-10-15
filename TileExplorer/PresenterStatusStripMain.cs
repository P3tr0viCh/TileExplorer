using GMap.NET;
using P3tr0viCh.AppUpdate;
using P3tr0viCh.Utils;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Enums;

namespace TileExplorer
{
    internal class PresenterStatusStripMain
    {
        public enum StatusLabel
        {
            Zoom,
            TileId,
            Position,
            MousePosition,

            Status,
            UpdateStatus,

            TracksCount,
            TracksDistance,

            TilesVisited,
            TilesMaxCluster,
            TilesMaxSquare,
        }

        public interface IPresenterStatusStripMain
        {
            ToolStripStatusLabel GetLabel(StatusLabel label);
        }

        private readonly IPresenterStatusStripMain view;

        public PresenterStatusStripMain(IPresenterStatusStripMain view)
        {
            this.view = view;

            Zoom = 0;
            TileId = PointLatLng.Empty;
            Position = PointLatLng.Empty;
            MousePosition = new PointLatLng(0, 0);

            Status = Status.Idle;
            UpdateStatus = UpdateStatus.Idle;

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

        public Status Status
        {
            set
            {
                view.GetLabel(StatusLabel.Status).Text = value.Description();
            }
        }

        public UpdateStatus UpdateStatus
        {
            set
            {
                switch (value)
                {
                    case UpdateStatus.CheckLatest:
                        view.GetLabel(StatusLabel.UpdateStatus).Text = Resources.AppUpdateInfoStatusCheckLatest;

                        break;
                    case UpdateStatus.Download:
                        view.GetLabel(StatusLabel.UpdateStatus).Text  = Resources.AppUpdateInfoStatusDownload;

                        break;
                    case UpdateStatus.ArchiveExtract:
                        view.GetLabel(StatusLabel.UpdateStatus).Text  = Resources.AppUpdateInfoStatusExtract;

                        break;
                    case UpdateStatus.Check:
                    case UpdateStatus.CheckLocal:
                    case UpdateStatus.Update:
                    case UpdateStatus.Idle:
                    default:
                        view.GetLabel(StatusLabel.UpdateStatus).Text  = string.Empty;

                        break;
                }
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