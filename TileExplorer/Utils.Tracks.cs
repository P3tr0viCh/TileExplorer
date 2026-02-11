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
            public static void UpdateTrackMinDistancePoint(Track track)
            {
                if (track.TrackPoints.Count < 2) return;

                var distance = 0D;

                track.TrackPoints.First().ShowOnMap = true;

                for (var i = 1; i < track.TrackPoints.Count - 1; i++)
                {
                    if (distance >= Const.TrackMinDistancePoint)
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

            private static Track OpenTrackFromFile(string path)
            {
                var track = new Track(path);

                UpdateTrackMinDistancePoint(track);

                DebugWrite.Line("end open file");

                return track;
            }

            public static async Task<Track> OpenTrackFromFileAsync(string path)
            {
                return await Task.Factory.StartNew(() =>
                {
                    return OpenTrackFromFile(path);
                });
            }
        }
    }
}