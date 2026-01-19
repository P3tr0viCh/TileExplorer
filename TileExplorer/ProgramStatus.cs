using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Converters;
using System.ComponentModel;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public class ProgramStatus : DefaultInstance<ProgramStatus<Status>>
    {
        [TypeConverter(typeof(EnumDescriptionConverter))]
        public enum Status
        {
            [Description("")]
            Idle,
            [Description("")]
            Starting,
            [Description("Загрузка...")]
            LoadData,
            [Description("Чтение файла gpx...")]
            LoadGpx,
            [Description("Сохранение...")]
            SaveData,
            [Description("Сохранение в архив...")]
            BackupSave,
            [Description("Чтение из архива...")]
            BackupLoad,
            [Description("Поиск файлов gpx...")]
            CheckDirectoryTracks,
        }
    }
}