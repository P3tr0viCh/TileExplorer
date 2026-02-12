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
            public static async Task<bool> ListItemSaveAsync<T>(T value) where T : BaseId
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

            public static async Task<bool> ListItemDeleteAsync<T>(T value) where T : BaseId
            {
                var status = ProgramStatus.Default.Start(Status.SaveData);

                try
                {
                    await Default.ListItemDeleteAsync(value);

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

            public static async Task UpdateTracksAsync(IEnumerable<Track> tracks)
            {
                var status = ProgramStatus.Default.Start(Status.SaveData);

                try
                {
                    await Default.TrackSaveAsync(tracks);
                }
                catch (Exception e)
                {
                    DebugWrite.Error(e);

                    Msg.Error(e.Message);
                }
                finally
                {
                    ProgramStatus.Default.Stop(status);
                }
            }
        }
    }
}