using GMap.NET;
using GMap.NET.WindowsForms;
using static TileExplorer.Database;

namespace TileExplorer
{
    internal class MapTrack : GMapRoute
    {
        public MapTrack(TrackModel track) : base(track.Text)
        {
            IsHitTestVisible = true;

            foreach (var trackPoint in track.TrackPoints)
            {
                Points.Add(new PointLatLng(trackPoint.Lat, trackPoint.Lng));
            }
        }
    }
}