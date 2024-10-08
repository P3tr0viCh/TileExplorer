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
            [Description("Поиск файлов gpx...")]
            CheckDirectoryTracks,
        }

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
            TracksTree = 128,
            ChartTrackEle = 256,
        }

        [Flags]
        public enum DataLoad
        {
            Tiles = 1,
            Tracks = 2,
            Markers = 4,
            TracksTree = 8,
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