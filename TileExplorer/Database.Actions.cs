using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class Database
    {
        internal static class Actions
        {
            public static async Task<bool> MarkerSaveAsync(Marker marker)
            {
                try
                {
                    await Default.MarkerSaveAsync(marker);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
            }

            public static async Task<bool> MarkerDeleteAsync(List<Marker> markers)
            {
                try
                {
                    await Default.MarkerDeleteAsync(markers);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
            }

            public static async Task<bool> TrackDeleteAsync(List<Track> tracks)
            {
                try
                {
                    await Default.TrackDeleteAsync(tracks);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
            }

            public static async Task<bool> EquipmentSaveAsync(Equipment equipment)
            {
                try
                {
                    await Default.EquipmentSaveAsync(equipment);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
            }

            public static async Task<bool> EquipmentDeleteAsync(List<Equipment> equipments)
            {
                try
                {
                    await Default.EquipmentDeleteAsync(equipments);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
            }
        }
    }
}