﻿using GMap.NET.WindowsForms;
using System;
using System.Drawing;
using static TileExplorer.Database;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public class MapItemTrack : GMapRoute, IMapItem
    {
        public MapItemType Type => MapItemType.Track;

        private const int DefaultWidth = 2;
        private const int DefaultWidthSelected = 4;

        public static Pen DefaultSelectedStroke = new Pen(Color.DarkMagenta, DefaultWidthSelected);

        [NonSerialized]
        public Pen SelectedStroke = DefaultSelectedStroke;

        private readonly MapItem<Models.Track> item;
        
        static MapItemTrack()
        {
            DefaultStroke.Width = DefaultWidth;
        }

        public MapItemTrack(Models.Track track) : base(track.Text)
        {
            item = new MapItem<Models.Track>(this, track);

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