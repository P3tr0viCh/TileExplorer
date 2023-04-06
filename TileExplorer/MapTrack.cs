using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh;
using System.Drawing;
using TileExplorer.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static TileExplorer.Database;

namespace TileExplorer
{
    internal class MapTrack : GMapRoute
    {
        private TrackModel trackModel;

        public MapTrack(TrackModel track) : base(track.Text)
        {
            IsHitTestVisible = true;

            Stroke = new Pen(Color.FromArgb(Settings.Default.ColorTrackAlpha, Settings.Default.ColorTrack),
                Settings.Default.WidthTrack);

            TrackModel = track;
        }

        public TrackModel TrackModel
        {
            get
            {
                return trackModel;
            }
            set
            {
                trackModel = value;

                NotifyModelChanged();
            }
        }

        public void NotifyModelChanged()
        {
            Name = trackModel.Text;

            Points.Clear();

            double lat1 = 0, lng1 = 0, lat2, lng2;

            foreach (var trackPoint in trackModel.TrackPoints)
            {
                lat2 = trackPoint.Lat;
                lng2 = trackPoint.Lng;

//                if (Geo.Haversine(lat1, lng1, lat2, lng2) < Settings.Default.TrackMinDistancePoint) continue;
                if (!trackPoint.IsUsedForDraw) continue;

                Points.Add(new PointLatLng(trackPoint.Lat, trackPoint.Lng));

                lat1 = lat2;
                lng1 = lng2;
            }
        }
    }
}