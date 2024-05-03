using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public static partial class Utils
    {
        public static class Tracks
        {
            public static void UpdateTrackMinDistancePoint(Track track)
            {
                if (track.TrackPoints.Count < 2) return;

                double distance = 0;

                track.TrackPoints.First().ShowOnMap = true;

                for (var i = 1; i < track.TrackPoints.Count - 1; i++)
                {
                    if (distance >= AppSettings.Default.TrackMinDistancePoint)
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

                var trackGpx = new P3tr0viCh.Utils.Gpx.Track();

                try
                {
                    WriteDebug(path);

                    trackGpx.OpenFromFile(path);

                    WriteDebug("xml loaded");

                    track.Text = trackGpx.Text;

                    track.DateTimeStart = trackGpx.DateTimeStart;
                    track.DateTimeFinish = trackGpx.DateTimeFinish;

                    track.Distance = trackGpx.Distance;

                    track.EleAscent = trackGpx.EleAscent;

                    track.TrackPoints = new List<TrackPoint>();

                    foreach (var point in trackGpx.Points)
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

                    WriteDebug($"point count: {track.TrackPoints.Count}");

                    UpdateTrackMinDistancePoint(track);
                }
                catch (Exception e)
                {
                    WriteDebug($"error: {e.Message}");

                    Msg.Error(e.Message);
                }

                WriteDebug("end open file");

                return track;
            }
        }
    }
}