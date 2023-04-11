using GMap.NET;
using GMap.NET.WindowsForms;
using P3tr0viCh;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using TileExplorer.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class MapTrack : GMapRoute
    {
        private TrackModel track;

        public static readonly Pen DefaultSelectedStroke;

        [NonSerialized]
        public Pen SelectedStroke = DefaultSelectedStroke;

        static MapTrack()
        {
            DefaultStroke.Color = Color.FromArgb(Settings.Default.ColorTrackAlpha, Settings.Default.ColorTrack);
            DefaultStroke.Width = Settings.Default.WidthTrack;

            DefaultSelectedStroke = new Pen(Color.FromArgb(Settings.Default.ColorTrackAlpha,
                Settings.Default.ColorTrackSelected), Settings.Default.WidthTrackSelected);
        }

        public MapTrack(TrackModel track) : base(track.Text)
        {
            IsHitTestVisible = true;

            UpdateColors();

            Track = track;
        }

        public TrackModel Track
        {
            get
            {
                return track;
            }
            set
            {
                track = value;

                NotifyModelChanged();
            }
        }

        public void NotifyModelChanged()
        {
            Name = track.Text;

            Points.Clear();

            double distance = 0;

            if (track.TrackPoints.Count < 2) return;

            Points.Add(Utils.TrackPointToPointLatLng(track.TrackPoints.First()));

            for (var i = 1; i < track.TrackPoints.Count - 1; i++)
            {
                if (distance >= Settings.Default.TrackMinDistancePoint)
                {
                    distance = 0;

                    Points.Add(Utils.TrackPointToPointLatLng(track.TrackPoints[i]));
                }
                else {
                    distance += track.TrackPoints[i].Distance;
                }
            }

            Points.Add(Utils.TrackPointToPointLatLng(track.TrackPoints.Last()));
        }

        private void UpdateColors()
        {
            Stroke = Selected ? DefaultSelectedStroke : DefaultStroke;
        }

        private bool selected = false;

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == value) return;

                selected = value;

                UpdateColors();
            }
        }
    }
}