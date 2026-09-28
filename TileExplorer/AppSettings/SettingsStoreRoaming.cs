using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Settings;

namespace TileExplorer
{
    internal class SettingsStoreRoaming : SettingsStore<SettingsRoaming>
    {
        public new bool Load()
        {
            DebugWrite.Line($"Settings Roaming: {FilePath}");

            if (base.Load()) return true;

            DebugWrite.Error(LastError);

            return false;
        }

        public new bool Save()
        {
            if (base.Save()) return true;

            DebugWrite.Error(LastError);

            return false;
        }
    }
}