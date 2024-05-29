using System;
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
        public enum Status
        {
            [Description("")]
            Idle,
            [Description("")]
            Starting,
            [Description("Загрузка...")]
            LoadData,
            [Description("Чтение файла gpx...")]
            LoadGpx,
            [Description("Сохранение...")]
            SaveData,
            [Description("Сохранение в архив...")]
            BackupSave,
        }

        public enum ChildFormType
        {
            Filter,
            TrackList,
            MarkerList,
            EquipmentList,
            ResultYears,
            ResultEquipments,
            TileInfo,
            TracksTree,
        }

        [Flags]
        public enum DataLoad
        {
            Tiles = 1,
            Tracks = 2,
            Markers = 4,
            TracksInfo = 8,
            TrackList = 16,
            MarkerList = 32,
            EquipmentList = 64,
            TracksTree = 128
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

        public enum StatusLabel
        {
            Zoom,
            TileId,
            Position,
            MousePosition,

            Status,
            UpdateStatus,

            TracksCount,
            TracksDistance,

            TilesVisited,
            TilesMaxCluster,
            TilesMaxSquare,
        }
    }
}