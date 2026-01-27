using System;

namespace TileExplorer
{
    [Flags]
    public enum FrmListType
    {
        None = 0,
        TagList = ChildFormType.TagList,
        TrackList = ChildFormType.TrackList,
        MarkerList = ChildFormType.MarkerList,
        EquipmentList = ChildFormType.EquipmentList,
        TileInfo = ChildFormType.TileInfo,
        ResultYears = ChildFormType.ResultYears,
        ResultEquipments = ChildFormType.ResultEquipments,
    }
}