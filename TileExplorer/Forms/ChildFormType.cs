using System;

namespace TileExplorer
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
}