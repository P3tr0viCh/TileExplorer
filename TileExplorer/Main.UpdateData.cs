using P3tr0viCh.Utils;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;
using System.Threading.Tasks;

namespace TileExplorer
{
    public partial class Main
    {
        public async Task UpdateDataAsync(DataLoad load = default)
        {
            if (load == default)
            {
                load = DataLoad.Tiles |
                       DataLoad.Tracks |
                       DataLoad.Markers |
                       DataLoad.TracksTree |
                       DataLoad.TrackList;
            }

            if (load.HasFlag(DataLoad.Tracks)) load |= DataLoad.Summary;

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

            if (load.HasFlag(DataLoad.Summary))
            {
                await LoadYearsAsync();

                await LoadTracksInfoAsync();
            }

            await UpdateDataChildFormsAsync(load);

            Selected = FindMapItem(savedSelected?.Model);
        }

        private async Task UpdateDataChildFormsAsync(DataLoad load)
        {
            bool updateData;

            foreach (var frm in Utils.Forms.GetChildForms<IUpdateDataForm>())
            {
                switch (frm.FormType)
                {
                    case ChildFormType.Filter:
                        updateData = load.HasFlag(DataLoad.TrackList);

                        break;
                    case ChildFormType.TileInfo:
                    case ChildFormType.TrackList:
                    case ChildFormType.ResultYears:
                    case ChildFormType.ResultEquipments:
                        updateData = load.HasFlag(DataLoad.Tracks) |
                                     load.HasFlag(DataLoad.TrackList);

                        break;
                    case ChildFormType.MarkerList:
                        updateData = load.HasFlag(DataLoad.Markers);

                        break;
                    case ChildFormType.EquipmentList:
                        updateData = load.HasFlag(DataLoad.Tracks) |
                                     load.HasFlag(DataLoad.TrackList);

                        break;
                    case ChildFormType.TracksTree:
                        updateData = load.HasFlag(DataLoad.TracksTree);

                        break;
                    case ChildFormType.ChartTracksByYear:
                    case ChildFormType.ChartTracksByMonth:
                        updateData = load.HasFlag(DataLoad.Tracks) |
                                     load.HasFlag(DataLoad.TrackList);

                        break;
                    default:
                        updateData = false;

                        break;
                }

                if (updateData)
                {
                    await frm.UpdateDataAsync();
                }
            };
        }
    }
}