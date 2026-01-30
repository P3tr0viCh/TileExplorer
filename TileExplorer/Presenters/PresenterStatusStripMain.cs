using GMap.NET;
using P3tr0viCh.AppUpdate;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Presenters;
using TileExplorer.Properties;
using static TileExplorer.ProgramStatus;

namespace TileExplorer.Presenters
{
    internal class PresenterStatusStripMain : PresenterStatusStrip<PresenterStatusStripMain.StatusLabel>
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
            TilesArea,
        }

        public PresenterStatusStripMain(IPresenterStatusStrip view) : base(view)
        {
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
            TilesArea = 0;
        }

        public double Zoom
        {
            set => View.GetLabel(StatusLabel.Zoom).Text = string.Format(Resources.StatusZoom, value);
        }

        public PointLatLng TileId
        {
            set => View.GetLabel(StatusLabel.TileId).Text = string.Format(Resources.StatusTileId,
                  Utils.Osm.LngToTileX(value), Utils.Osm.LatToTileY(value));
        }

        public PointLatLng Position
        {
            set => View.GetLabel(StatusLabel.Position).Text =
                  string.Format(Resources.StatusPosition, value.Lat, value.Lng);
        }

        public PointLatLng MousePosition
        {
            set => View.GetLabel(StatusLabel.MousePosition).Text =
                  string.Format(Resources.StatusMousePosition, value.Lat, value.Lng);
        }

        public Status Status
        {
            set => View.GetLabel(StatusLabel.Status).Text = value.Description();
        }

        public UpdateStatus UpdateStatus
        {
            set
            {
                switch (value)
                {
                    case UpdateStatus.CheckLatest:
                        View.GetLabel(StatusLabel.UpdateStatus).Text = Resources.AppUpdateInfoStatusCheckLatest;

                        break;
                    case UpdateStatus.Download:
                        View.GetLabel(StatusLabel.UpdateStatus).Text = Resources.AppUpdateInfoStatusDownload;

                        break;
                    case UpdateStatus.ArchiveExtract:
                        View.GetLabel(StatusLabel.UpdateStatus).Text = Resources.AppUpdateInfoStatusExtract;

                        break;
                    case UpdateStatus.Check:
                    case UpdateStatus.CheckLocal:
                    case UpdateStatus.Update:
                    case UpdateStatus.Idle:
                    default:
                        View.GetLabel(StatusLabel.UpdateStatus).Text = string.Empty;

                        break;
                }
            }
        }

        public int TracksCount
        {
            set => View.GetLabel(StatusLabel.TracksCount).Text = string.Format(Resources.StatusTracksCount, value);
        }

        public double TracksDistance
        {
            set => View.GetLabel(StatusLabel.TracksDistance).Text = string.Format(Resources.StatusTracksDistance, value);
        }

        public int TilesVisited
        {
            set => View.GetLabel(StatusLabel.TilesVisited).Text = string.Format(Resources.StatusTilesVisited, value);
        }

        public int TilesMaxCluster
        {
            set => View.GetLabel(StatusLabel.TilesMaxCluster).Text = string.Format(Resources.StatusTilesMaxCluster, value);
        }

        public int TilesMaxSquare
        {
            set => View.GetLabel(StatusLabel.TilesMaxSquare).Text = string.Format(Resources.StatusTilesMaxSquare, value);
        }

        public double TilesArea
        {
            set => View.GetLabel(StatusLabel.TilesArea).Text = string.Format(Resources.StatusTilesArea, value);
        }
    }
}