using GMap.NET.WindowsForms;
using System;
using System.Drawing;
using System.Linq;
using TileExplorer.Properties;
using static TileExplorer.Database;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public class MapItemTrack : GMapRoute, IMapItem
    {
        public MapItemType Type => MapItemType.Track;

        public static Pen DefaultSelectedStroke;

        [NonSerialized]
        public Pen SelectedStroke = DefaultSelectedStroke;

        private readonly MapItem<Models.Track> item;

        public MapItemTrack(Models.Track track) : base(track.Text)
        {
            item = new MapItem<Models.Track>(this, track);

            DefaultStroke.Color = Color.FromArgb(AppSettings.Default.ColorTrackAlpha, AppSettings.Default.ColorTrack);
            DefaultStroke.Width = AppSettings.Default.WidthTrack;

            DefaultSelectedStroke = new Pen(Color.FromArgb(AppSettings.Default.ColorTrackSelectedAlpha,
                AppSettings.Default.ColorTrackSelected), AppSettings.Default.WidthTrackSelected);

            IsHitTestVisible = true;

            NotifyModelChanged();
            UpdateColors();
        }

        public Models.Track Model { get => item.Model; set => item.Model = value; }
        Models.BaseId IMapItem.Model { get => Model; set => Model = (Models.Track)value; }

        public bool Selected { get => item.Selected; set => item.Selected = value; }

        public void NotifyModelChanged()
        {
            Name = Model.Text;

            Points.Clear();

            if (Model.TrackPoints == null) return;

            Model.TrackPoints.ForEach(p => { Points.Add(Utils.TrackPointToPointLatLng(p)); });
        }

        public void UpdateColors()
        {
            Stroke = Selected ? DefaultSelectedStroke : DefaultStroke;
        }
    }
}