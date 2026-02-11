using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using TileExplorer.Interfaces;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private async Task InternalUpdateDataAsync2(DataLoad load = default, object value = null)
        {
            if (load == default)
            {
                load = DataLoad.Tiles |
                       DataLoad.Tracks |
                       DataLoad.Markers;
            }

            DebugWrite.Line($"Loading data {load}");

            if (load.HasFlag(DataLoad.Tiles))
            {
                tilesLoaded = false;

                if (miMainShowTiles.Checked)
                {
                    await LoadTilesAsync();
                }
            }

            if (load.HasFlag(DataLoad.Tracks))
            {
                tracksLoaded = false;

                if (miMainShowTracks.Checked)
                {
                    await LoadTracksAsync();
                }
            }

            if (load.HasFlag(DataLoad.Markers))
            {
                markersLoaded = false;

                if (miMainShowMarkers.Checked)
                {
                    await LoadMarkersAsync();
                }
            }

            if (load.HasFlag(DataLoad.Tracks) ||
                IsObjectChangedOrDeleted(load, value, typeof(Track)))
            {
                await LoadYearsAsync();

                await LoadTracksInfoAsync();
            }

            //await UpdateDataChildFormsAsync(load, value);

            if (load.HasFlag(DataLoad.ObjectDelete))
            {
                if (value?.GetType() == typeof(Track))
                {
                    var track = (Track)value;

                    overlayTracks.Routes.Remove(
                        overlayTracks.Routes.Cast<IMapItem>().Where(i => i.Model.Id == track.Id)?
                        .Cast<MapItemTrack>().FirstOrDefault());
                }

                if (value?.GetType() == typeof(Marker))
                {
                    var marker = (Marker)value;

                    overlayMarkers.Markers.Remove(
                        overlayMarkers.Markers.Cast<IMapItem>().Where(i => i.Model.Id == marker.Id)?
                        .Cast<MapItemMarker>().FirstOrDefault());
                }
            }
        }
        
        private async Task InternalUpdateDataAsync(DataLoad load = default)
        {
            if (load == default)
            {
                load = DataLoad.Tiles |
                       DataLoad.Tracks |
                       DataLoad.Markers | 
                       DataLoad.TrackListChanged;
            }

            DebugWrite.Line($"Loading data {load}");

            if (load.HasFlag(DataLoad.Tiles))
            {
                tilesLoaded = false;

                if (miMainShowTiles.Checked)
                {
                    await LoadTilesAsync();
                }
            }

            if (load.HasFlag(DataLoad.Tracks))
            {
                tracksLoaded = false;

                if (miMainShowTracks.Checked)
                {
                    await LoadTracksAsync();
                }
            }

            if (load.HasFlag(DataLoad.Markers))
            {
                markersLoaded = false;

                if (miMainShowMarkers.Checked)
                {
                    await LoadMarkersAsync();
                }
            }

            if (load.HasFlag(DataLoad.TrackListChanged))
            {
                await LoadYearsAsync();

                await LoadTracksInfoAsync();
            }

            await Utils.Forms.ChildFormsUpdateDataAsync();
        }

        public async Task UpdateDataAsync(DataLoad load = default)
        {
            var selected = Selected?.Model;

            Selected = null;

            await InternalUpdateDataAsync(load);

            var item = FindMapItem(selected);

            Selected = item;
        }

        private bool IsObjectChanged(DataLoad load, object value, Type type)
        {
            if (!load.HasFlag(DataLoad.ObjectChange)) return false;

            return value?.GetType() == type;
        }

        private bool IsObjectDeleted(DataLoad load, object value, Type type)
        {
            if (!load.HasFlag(DataLoad.ObjectDelete)) return false;

            return value?.GetType() == type;
        }

        private bool IsObjectChangedOrDeleted(DataLoad load, object value, Type type)
        {
            if (value?.GetType() != type) return false;

            return load.HasFlag(DataLoad.ObjectChange) || load.HasFlag(DataLoad.ObjectDelete);
        }

        private async Task UpdateDataChildFormsAsync(DataLoad load, object value = null)
        {
            DataUpdate dataUpdate;

            foreach (var frm in Utils.Forms.GetChildForms<IChildForm>())
            {
                dataUpdate = DataUpdate.None;

                var frmType = ((IChildForm)frm).FormType;

                switch (frmType)
                {
                    case ChildFormType.Filter:
                        if (load.HasFlag(DataLoad.Tracks) ||
                            IsObjectChangedOrDeleted(load, value, typeof(Track)) ||
                            IsObjectChangedOrDeleted(load, value, typeof(TagModel)) ||
                            IsObjectChangedOrDeleted(load, value, typeof(Equipment)))
                        {
                            dataUpdate = DataUpdate.Full;
                        }

                        break;
                    case ChildFormType.TileInfo:
                        if (load.HasFlag(DataLoad.Tracks))
                        {
                            dataUpdate = DataUpdate.Full;
                            break;
                        }

                        if (IsObjectChanged(load, value, typeof(Track)))
                        {
                            dataUpdate = DataUpdate.ObjectChange;
                            break;
                        }

                        if (IsObjectDeleted(load, value, typeof(Track)))
                        {
                            dataUpdate = DataUpdate.ObjectDelete;
                            break;
                        }

                        break;
                    case ChildFormType.TrackList:
                        if (load.HasFlag(DataLoad.Tracks) ||
                            IsObjectChangedOrDeleted(load, value, typeof(TagModel)) ||
                            IsObjectChangedOrDeleted(load, value, typeof(Equipment)))
                        {
                            dataUpdate = DataUpdate.Full;
                            break;
                        }

                        if (IsObjectChanged(load, value, typeof(Track)))
                        {
                            dataUpdate = DataUpdate.ObjectChange;
                            break;
                        }

                        if (IsObjectDeleted(load, value, typeof(Track)))
                        {
                            dataUpdate = DataUpdate.ObjectDelete;
                            break;
                        }

                        break;
                    case ChildFormType.ResultYears:
                        if (load.HasFlag(DataLoad.Tracks) ||
                            IsObjectChangedOrDeleted(load, value, typeof(Track)))
                        {
                            dataUpdate = DataUpdate.Full;
                        }

                        break;
                    case ChildFormType.ResultEquipments:
                        if (load.HasFlag(DataLoad.Tracks) ||
                            IsObjectChangedOrDeleted(load, value, typeof(Track)) ||
                            IsObjectChangedOrDeleted(load, value, typeof(Equipment)))
                        {
                            dataUpdate = DataUpdate.Full;
                        }

                        break;
                    case ChildFormType.MarkerList:
                        if (load.HasFlag(DataLoad.Markers))
                        {
                            dataUpdate = DataUpdate.Full;
                            break;
                        }

                        if (IsObjectChanged(load, value, typeof(Marker)))
                        {
                            dataUpdate = DataUpdate.ObjectChange;
                            break;
                        }

                        if (IsObjectDeleted(load, value, typeof(Marker)))
                        {
                            dataUpdate = DataUpdate.ObjectDelete;
                            break;
                        }

                        break;
                    case ChildFormType.TagList:
                        if (load.HasFlag(DataLoad.Tracks))
                        {
                            dataUpdate = DataUpdate.Full;
                            break;
                        }

                        break;
                    case ChildFormType.EquipmentList:
                        if (load.HasFlag(DataLoad.Tracks))
                        {
                            dataUpdate = DataUpdate.Full;
                            break;
                        }

                        if (IsObjectChanged(load, value, typeof(Equipment)))
                        {
                            dataUpdate = DataUpdate.ObjectChange;
                            break;
                        }

                        if (IsObjectDeleted(load, value, typeof(Equipment)))
                        {
                            dataUpdate = DataUpdate.ObjectDelete;
                            break;
                        }

                        break;
                    case ChildFormType.ChartTracksByYear:
                    case ChildFormType.ChartTracksByMonth:
                        if (load.HasFlag(DataLoad.Tracks) ||
                            IsObjectChangedOrDeleted(load, value, typeof(Track)))
                        {
                            dataUpdate = DataUpdate.Full;
                        }

                        break;
                    case ChildFormType.ChartTrackEle:
                        if (IsObjectDeleted(load, value, typeof(Track)))
                        {
                            if (((FrmChartTrackEle)frm).Track.Id == ((Track)value).Id)
                            {
                                ((FrmChartTrackEle)frm).Close();
                            }
                        }

                        break;
                }

                switch (dataUpdate)
                {
                    case DataUpdate.Full:
                        //await frm.UpdateDataAsync();

                        break;
                    case DataUpdate.ObjectChange:
                        //frm.ListItemChange((BaseId)value);

                        break;
                    case DataUpdate.ObjectDelete:
                        //((IFrmListBase)frm).ListItemDelete((BaseId)value);

                        if (frmType == ChildFormType.TileInfo)
                        {
                            if (((FrmList)frm).Count == 0)
                            {
                                ((FrmList)frm).Close();
                            }
                        }

                        break;
                }
            }
            ;
        }
    }
}