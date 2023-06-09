﻿using GMap.NET.WindowsForms;
using System;
using System.Drawing;
using System.Linq;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class MapTrack : GMapRoute, IMapItem
    {
        public MapItemType Type => MapItemType.Track;

        public static Pen DefaultSelectedStroke;

        [NonSerialized]
        public Pen SelectedStroke = DefaultSelectedStroke;

        private readonly MapItem<Models.Track> item;

        public MapTrack(Models.Track track) : base(track.Text)
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

            double distance = 0;

            if (Model.TrackPoints.Count < 2) return;

            Points.Add(Utils.TrackPointToPointLatLng(Model.TrackPoints.First()));

            for (var i = 1; i < Model.TrackPoints.Count - 1; i++)
            {
                if (distance >= AppSettings.Default.TrackMinDistancePoint)
                {
                    distance = 0;

                    Points.Add(Utils.TrackPointToPointLatLng(Model.TrackPoints[i]));
                }
                else
                {
                    distance += Model.TrackPoints[i].Distance;
                }
            }

            Points.Add(Utils.TrackPointToPointLatLng(Model.TrackPoints.Last()));
        }

        public void UpdateColors()
        {
            Stroke = Selected ? DefaultSelectedStroke : DefaultStroke;
        }
    }
}