using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System.Threading.Tasks;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private async Task PerformUpdateDataAsync(DataLoad load)
        {
            var childFormType = load == DataLoad.All ? ChildFormType.All : ChildFormType.None;

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

            if (load.HasFlag(DataLoad.TracksInfo))
            {
                await LoadYearsAsync();

                await LoadTracksInfoAsync();
            }

            await Utils.Forms.ChildFormsUpdateDataAsync(childFormType);
        }

        public async Task UpdateDataAsync(DataLoad load)
        {
            var selected = Selected?.Model;

            Selected = null;

            await PerformUpdateDataAsync(load);

            var item = FindMapItem(selected);

            Selected = item;
        }
    }
}