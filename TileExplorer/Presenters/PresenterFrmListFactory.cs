using System;
using TileExplorer.Interfaces;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListFactory
    {
        public static IPresenterFrmListBase PresenterFrmListInstance(IFrmListBase frmList, ChildFormType frmType)
        {
            switch (frmType)
            {
                case ChildFormType.TagList:
                    return new PresenterFrmListTags(frmList);
                case ChildFormType.TrackList:
                    return new PresenterFrmListTracks(frmList);
                case ChildFormType.MarkerList:
                    return new PresenterFrmListMarkers(frmList);
                case ChildFormType.EquipmentList:
                    return new PresenterFrmListEquipments(frmList);
                case ChildFormType.TileInfo:
                    return new PresenterFrmListTileInfo(frmList);
                case ChildFormType.ResultYears:
                    return new PresenterFrmListResultYears(frmList);
                case ChildFormType.ResultEquipments:
                    return new PresenterFrmListResultEquipments(frmList);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}