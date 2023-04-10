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
        private TrackModel trackModel;

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

            double distance = 0;

            if (trackModel.TrackPoints.Count < 2) return;

            Points.Add(Utils.TrackPointToPointLatLng(trackModel.TrackPoints.First()));

            for (var i = 1; i < trackModel.TrackPoints.Count - 1; i++)
            {
                if (distance >= Settings.Default.TrackMinDistancePoint)
                {
                    distance = 0;

                    Points.Add(Utils.TrackPointToPointLatLng(trackModel.TrackPoints[i]));
                }
                else {
                    distance += trackModel.TrackPoints[i].Distance;
                }
            }

            Points.Add(Utils.TrackPointToPointLatLng(trackModel.TrackPoints.Last()));
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