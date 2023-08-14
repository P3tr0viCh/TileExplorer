﻿using System;
using System.ComponentModel;
using System.Linq;
using static P3tr0viCh.Utils.Converters;

namespace TileExplorer
{
    public static class Enums
    {
        public static string Description(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes
                && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        [TypeConverter(typeof(EnumDescriptionConverter))]
        public enum ProgramStatus
        {
            [Description("")]
            Idle,
            [Description("Загрузка...")]
            LoadData,
            [Description("Чтение файла gpx...")]
            LoadGpx,
            [Description("Сохранение...")]
            SaveData
        }

        public enum ChildFormType
        {
            Filter,
            Tracks,
            Markers,
            Results,
        }

        [Flags]
        public enum DataLoad
        {
            Tiles = 1,
            Tracks = 2,
            Markers = 4,
            TracksInfo = 8,
            TracksList = 16,
            MarkersList = 32,
        }

        public enum MapItemType
        {
            Marker,
            Track,
            Tile
        }
        
        public enum SaveFileDialogType
        {
            Png, Osm, Gpx
        }

        [TypeConverter(typeof(EnumDescriptionConverter))]
        public enum OsmandIconBackgroud
        {
            [Description("Круг")]
            Circle,
            [Description("Восьмиугольник")]
            Octagon,
            [Description("Квадрат")]
            Square
        }
    }
}