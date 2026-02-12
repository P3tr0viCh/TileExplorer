using System.ComponentModel;
using P3tr0viCh.Utils.Converters;

namespace TileExplorer
{
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