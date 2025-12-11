using P3tr0viCh.Utils;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;
using System;
using P3tr0viCh.Database;

namespace TileExplorer
{
    public partial class Main
    {
        public async Task UpdateDataAsync(DataLoad load = default, object value = null)
        {
            if (load == default)
            {
                load = DataLoad.Tiles |
                       DataLoad.Tracks |
                       DataLoad.Markers;
            }

            DebugWrite.Line($"Loading data {load}");

            var savedSelected = Selected;

            Selected = null;

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

            await UpdateDataChildFormsAsync(load, value);

            Selected = FindMapItem(savedSelected?.Model);
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

            foreach (var frm in Utils.Forms.GetChildForms<IUpdateDataForm>())
            {
                dataUpdate = DataUpdate.None;

                switch (frm.FormType)
                {
                    case ChildFormType.Filter:
                        if (load.HasFlag(DataLoad.Tracks) ||
                            IsObjectChangedOrDeleted(load, value, typeof(Track)) ||
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
                        await frm.UpdateDataAsync();

                        break;
                    case DataUpdate.ObjectChange:
                        ((FrmList)frm).ListItemChange((BaseId)value);

                        break;
                    case DataUpdate.ObjectDelete:
                        ((FrmList)frm).ListItemDelete((BaseId)value);

                        if (frm.FormType == ChildFormType.TileInfo)
                        {
                            if (((FrmList)frm).Count == 0)
                            {
                                ((FrmList)frm).Close();
                            }
                        }

                        break;
                }
            };
        }
    }
}