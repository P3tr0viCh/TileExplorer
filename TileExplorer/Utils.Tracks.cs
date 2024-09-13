using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
            }

            public static Track OpenTrackFromFile(string path)
            {
                var track = new Track();

                try
                {
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

                    UpdateTrackMinDistancePoint(track);
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);
                }

                DebugWrite.Line("end open file");

                return track;
            }
        }
    }
}