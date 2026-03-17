using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Interfaces;
using System.Threading.Tasks;

namespace TileExplorer
{
    public partial class Main
    {
        private async Task MainUpdateDataAsync(DataLoad load)
        {
            if (load.HasFlag(DataLoad.All))
            {
                load = EnumExtensions.All<DataLoad>();
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

            if (load.HasFlag(DataLoad.TracksInfo))
            {
                await LoadYearsAsync();

                await LoadTracksInfoAsync();
            }
        }

        private static async Task ChildFormsUpdateDataAsync(ChildFormType type)
        {
            if (type.HasFlag(ChildFormType.All))
            {
                type = EnumExtensions.All<ChildFormType>();
            }

            DebugWrite.Line($"Loading data {type}");

            var forms = Utils.Forms.GetChildForms<IUpdateData>(type);

            foreach (var form in forms)
            {
                await form.UpdateDataAsync();
            }
        }

        public async Task UpdateDataAsync(DataLoad load, ChildFormType childFormType)
        {
            var selected = Selected?.Model;

            Selected = null;

            await MainUpdateDataAsync(load);

            await ChildFormsUpdateDataAsync(childFormType);

            var item = FindMapItem(selected);

            Selected = item;
        }
    }
}