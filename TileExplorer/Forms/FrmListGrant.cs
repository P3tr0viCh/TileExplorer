using System;

namespace TileExplorer
{
    [Flags]
    public enum FrmListGrant
    {
        None = 0,
        Add = 1,
        Change = 2,
        Delete = 4,
        Sort = 8,
        All = Add | Change | Delete | Sort,
    }
}