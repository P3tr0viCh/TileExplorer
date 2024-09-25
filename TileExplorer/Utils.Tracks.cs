using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    internal static partial class Utils
    {
        public static class Tracks
        {
            public static async Task UpdateTrackMinDistancePointAsync(Track track)
            {
                await Task.Factory.StartNew(() =>
                {
                    if (track.TrackPoints.Count < 2) return;

                    var distance = 0D;

                    track.TrackPoints.First().ShowOnMap = true;

                    for (var i = 1; i < track.TrackPoints.Count - 1; i++)
                    {
                        if (distance >= AppSettings.Roaming.Default.TrackMinDistancePoint)
                        {
                            distance = 0;

                            track.TrackPoints[i].ShowOnMap = true;
                        }
                        else
                        {
                            distance += track.TrackPoints[i].Distance;

                            track.TrackPoints[i].ShowOnMap = false;
                        }
                    }

                    track.TrackPoints.Last().ShowOnMap = true;
                });
            }

            public static async Task CalcTrackTilesAsync(Track track)
            {
                await Task.Factory.StartNew(() =>
                {
                    track.TrackTiles = new List<Tile>();

                    int x, y;

                    track.TrackPoints.ForEach(point =>
                    {
                        x = Osm.LngToTileX(point.Lng);
                        y = Osm.LatToTileY(point.Lat);

                        if (track.TrackTiles.FindIndex(tile => tile.X == x && tile.Y == y) == -1)
                        {
                            track.TrackTiles.Add(new Tile(x, y));
                        }
                    });

                    DebugWrite.Line($"{track.TrackTiles.Count()}");
                });
            }

            public static async Task<Track> OpenTrackFromFileAsync(string path)
            {
                var result = await Task.Factory.StartNew(() =>
                {
                    var track = new Track();

                    DebugWrite.Line(path);

                    track.Gpx.OpenFromFile(path);

                    DebugWrite.Line("xml loaded");

                    track.TrackPoints = new List<TrackPoint>();

                    foreach (var point in track.Gpx.Points)
                    {
                        track.TrackPoints.Add(new TrackPoint()
                        {
                            Num = point.Num,

                            Lat = point.Lat,
                            Lng = point.Lng,

                            DateTime = point.DateTime,

                            Ele = point.Ele,

                            Distance = point.Distance,
                        });
                    }

                    DebugWrite.Line($"point count: {track.TrackPoints.Count}");

                    return track;
                });

                await UpdateTrackMinDistancePointAsync(result);

                DebugWrite.Line("end open file");

                return result;
            }
        }
    }
}