using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public static partial class Utils
    {
        public static class Tracks
        {
            private static string XmlGetText(XmlNode node)
            {
                return node != null ? node.InnerText : string.Empty;
            }

            public static Track OpenTrackFromFile(string path)
            {
                var trackXml = new XmlDocument();

                var track = new Track();

                try
                {
                    WriteDebug(path);

                    trackXml.Load(path);

                    WriteDebug("xml loaded");

                    track.TrackPoints = new List<TrackPoint>();

                    var trkptList = trackXml.GetElementsByTagName("trkpt");

                    WriteDebug("trkptList count: " + trkptList.Count);

                    var num = 0;

                    foreach (XmlNode trkpt in trkptList)
                    {
                        if (trkpt.Attributes["lat"] != null && trkpt.Attributes["lon"] != null)
                        {
                            track.TrackPoints.Add(new TrackPoint()
                            {
                                Num = num++,

                                Lat = DoubleParse(trkpt.Attributes["lat"].Value),
                                Lng = DoubleParse(trkpt.Attributes["lon"].Value),

                                DateTime = DateTimeParse(XmlGetText(trkpt["time"])),

                                Ele = DoubleParse(XmlGetText(trkpt["ele"]))
                            });
                        }
                    }

                    if (track.TrackPoints.Count < 2)
                    {
                        throw new Exception("empty track");
                    }

                    track.Distance = 0;

                    var pointPrev = track.TrackPoints.First();

                    foreach (var point in track.TrackPoints)
                    {
                        point.Distance = Geo.Haversine(pointPrev.Lat, pointPrev.Lng, point.Lat, point.Lng);
                        
                        track.Distance += point.Distance;

                        pointPrev = point;
                    }
                                       

                    var trkname = XmlGetText(trackXml.DocumentElement["trk"]?["name"]);

                    if (trkname == string.Empty)
                    {
                        trkname = XmlGetText(trackXml.DocumentElement["metadata"]?["name"]);

                        if (trkname == string.Empty)
                        {
                            trkname = Path.GetFileNameWithoutExtension(path);
                        }
                    }

                    track.Text = trkname;

                    track.DateTimeStart = track.TrackPoints.First().DateTime;
                    track.DateTimeFinish = track.TrackPoints.Last().DateTime;

                    if (track.DateTimeStart == default)
                    {
                        track.DateTimeStart = DateTimeParse(XmlGetText(trackXml.DocumentElement["metadata"]?["time"]), DateTime.Now);
                    }
                    if (track.DateTimeFinish == default)
                    {
                        track.DateTimeFinish = track.DateTimeStart;
                    }
                }
                catch (Exception e)
                {
                    WriteDebug("error: " + e.Message);

                    Msg.Error(e.Message);
                }

                WriteDebug("end open xml");

                return track;
            }
        }
    }
}