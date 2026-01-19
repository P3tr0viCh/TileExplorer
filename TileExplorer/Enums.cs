using System;
using System.ComponentModel;
using P3tr0viCh.Utils.Converters;

namespace TileExplorer
{
    public static class Enums
    {
        [Flags]
        public enum ChildFormType
        {
            Filter = 1,
            TrackList = 2,
            MarkerList = 4,
            EquipmentList = 8,
            ResultYears = 16,
            ResultEquipments = 32,
            TileInfo = 64,
            ChartTrackEle = 256,
            ChartTracksByYear = 512,
            ChartTracksByMonth = 1024,
        }

        [Flags]
        public enum DataLoad
        {
            Tiles = 1,
            Tracks = 2,
            Markers = 4,
            ObjectChange = 64,
            ObjectDelete = 128,
        }
        
        public enum DataUpdate
        {
            None,
            Full,
            ObjectChange,
            ObjectDelete,
        }

        public enum TileStatus
        {
            Unknown = 0,
            Visited = 1,
            Cluster = 2,
            MaxCluster = 3,
            MaxSquare = 4,
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