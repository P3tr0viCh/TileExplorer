using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System.Threading.Tasks;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private async Task PerformUpdateDataAsync(DataLoad load = default)
        {
            var childFormType = ChildFormType.None;

            if (load == default)
            {
                load = DataLoad.Tiles |
                       DataLoad.Tracks |
                       DataLoad.Markers | 
                       DataLoad.TrackListChanged;

                childFormType = default;
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

                childFormType = childFormType.AddFlag(ChildFormType.TrackList);
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

            await Utils.Forms.ChildFormsUpdateDataAsync(childFormType);
        }

        public async Task UpdateDataAsync(DataLoad load = default)
        {
            var selected = Selected?.Model;

            Selected = null;

            await PerformUpdateDataAsync(load);

            var item = FindMapItem(selected);

            Selected = item;
        }
    }
}