using System;
using System.ComponentModel;
using static P3tr0viCh.Utils.Converters;

namespace TileExplorer
{
    public static class Enums
    {
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
            ChartTrackEle,
        }

        [Flags]
        public enum DataLoad
        {
            Tiles = 1,
            Tracks = 2,
            Markers = 4,
            Equipments = 64,
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