using System;
using TileExplorer.Interfaces;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListFactory
    {
        public static IPresenterFrmList PresenterFrmListInstance(IFrmList frmList, FrmListType listType)
        {
            switch (listType)
            {
                case FrmListType.TagList:
                    return new PresenterFrmListTags(frmList);
                case FrmListType.TrackList:
                    return new PresenterFrmListTracks(frmList);
                case FrmListType.MarkerList:
                    return new PresenterFrmListMarkers(frmList);
                case FrmListType.EquipmentList:
                    return new PresenterFrmListEquipments(frmList);
                case FrmListType.TileInfo:
                    return new PresenterFrmListTileInfo(frmList);
                case FrmListType.ResultYears:
                    return new PresenterFrmListResultYears(frmList);
                case FrmListType.ResultEquipments:
                    return new PresenterFrmListResultEquipments(frmList);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}