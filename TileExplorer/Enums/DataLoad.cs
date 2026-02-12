using System;

namespace TileExplorer
{
    [Flags]
    public enum DataLoad
    {
        None = 0,
        All = 1,
        Tiles = All << 1,
        Tracks = Tiles << 1,
        Markers = Tracks << 1,
        TracksInfo = Markers << 1,
    }
}