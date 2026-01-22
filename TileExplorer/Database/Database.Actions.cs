using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class Database
    {
        internal static class Actions
        {
            private static async Task<bool> ListItemSaveAsync<T>(T value) where T : BaseId
            {
                var status = ProgramStatus.Default.Start(Status.SaveData);

                try
                {
                    await Default.ListItemSaveAsync(value);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
                finally
                {
                    ProgramStatus.Default.Stop(status);
                }
            }

            private static async Task<bool> ListItemDeleteAsync<T>(IEnumerable<T> values) where T : BaseId
            {
                var status = ProgramStatus.Default.Start(Status.SaveData);

                try
                {
                    await Default.ListItemDeleteAsync(values);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
                finally
                {
                    ProgramStatus.Default.Stop(status);
                }
            }

            public static async Task<bool> MarkerSaveAsync(Marker marker)
            {
                return await ListItemSaveAsync(marker);
            }

            public static async Task<bool> MarkerDeleteAsync(IEnumerable<Marker> markers)
            {
                return await ListItemDeleteAsync(markers);
            }

            public static async Task<bool> TrackDeleteAsync(IEnumerable<Track> tracks)
            {
                return await ListItemDeleteAsync(tracks);
            }

            public static async Task<bool> TagSaveAsync(TagModel tag)
            {
                return await ListItemSaveAsync(tag);
            }

            public static async Task<bool> TagDeleteAsync(IEnumerable<TagModel> tags)
            {
                return await ListItemDeleteAsync(tags);
            }

            public static async Task<bool> EquipmentSaveAsync(Equipment equipment)
            {
                return await ListItemSaveAsync(equipment);
            }

            public static async Task<bool> EquipmentDeleteAsync(IEnumerable<Equipment> equipments)
            {
                return await ListItemDeleteAsync(equipments);
            }
        }
    }
}