using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Settings;

namespace TileExplorer
{
    internal class SettingsStorageLocal : SettingsStorage<SettingsLocal>
    {
        public new bool Load()
        {
            DebugWrite.Line($"Settings Local: {FilePath}");

            if (!base.Load())
            {
                DebugWrite.Error(LastError);

                return false;
            }

            AppSettings.Default.UpdateDirectoryRoaming();

            return true;
        }

        public new bool Save()
        {
            if (base.Save()) return true;

            DebugWrite.Error(LastError);

            return false;
        }
    }
}