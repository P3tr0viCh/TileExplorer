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

            public static async Task<bool> MarkerDeleteAsync(Marker marker)
            {
                try
                {
                    await Default.MarkerDeleteAsync(marker);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
            }

            public static async Task<bool> TrackDeleteAsync(Track track)
            {
                try
                {
                    await Default.TrackDeleteAsync(track);

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

            public static async Task<bool> EquipmentDeleteAsync(Equipment equipment)
            {
                try
                {
                    await Default.EquipmentDeleteAsync(equipment);

                    return true;
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return false;
                }
            }

            public static async Task<List<T>> ListLoadAsync<T>(object filter = null)
            {
                DebugWrite.Line("start");

                try
                {
                    return await Default.ListLoadAsync<T>(filter);
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);

                    return null;
                }
                finally
                {
                    DebugWrite.Line("end");
                }
            }
        }
    }
}