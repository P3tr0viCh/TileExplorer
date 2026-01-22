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
            None = 0,
            Filter = 1,
            TrackList = Filter << 1,
            MarkerList = TrackList << 1,
            TagList = MarkerList << 1,
            EquipmentList = TagList << 1,
            ResultYears = EquipmentList << 1,
            ResultEquipments = ResultYears << 1,
            TileInfo = ResultEquipments << 1,
            ChartTrackEle = TileInfo << 1,
            ChartTracksByYear = ChartTrackEle << 1,
            ChartTracksByMonth = ChartTracksByYear << 1,
        }

        [Flags]
        public enum DataLoad
        {
            None = 0,
            Tiles = 1,
            Tracks = Tiles << 1,
            Markers = Tracks << 1,
            ObjectChange = Markers << 1,
            ObjectDelete = ObjectChange << 1,
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