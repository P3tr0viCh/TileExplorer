using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using TileExplorer.Properties;

namespace TileExplorer
{
    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        static string Localize(string key)
        {
            return Resources.ResourceManager.GetString(key);
        }

        public LocalizedCategoryAttribute(string key) : base(Localize(key))
        {
        }
    }

    public class AppSettings
    {
        [LocalizedCategory("SettingsCategoryCommon")]
        [DisplayName("Расположение базы данных")]
        [Description("Расположение файла базы данных. Пустое значение – \\AppData\\Roaming\\TileExplorer\\")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DatabaseHome { get; set; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Фон")]
        [Description("Цвет фона маркера")]
        public Color ColorMarkerFill { get; set; }

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Прозрачность")]
        [Description("Прозрачность фона маркера")]
        public byte ColorMarkerFillAlpha { get; set; }

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Цвет текста")]
        [Description("Цвет текста маркера")]
        public Color ColorMarkerText { get; set; }

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Прозрачность текста")]
        [Description("Прозрачность текста маркера")]
        public byte ColorMarkerTextAlpha { get; set; }

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Шрифт")]
        [Description("Шрифт маркера")]
        public Font FontMarker { get; set; }

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Выбранный трек: цвет")]
        [Description("Цвет плитки выбранного трека")]
        public Color ColorTileTrackSelected { get; set; }

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Выбранный трек: прозрачность")]
        [Description("Прозрачность плитки выбранного трека")]
        public byte ColorTileTrackSelectedAlpha { get; set; }

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Выбранный трек: прозрачность границы")]
        [Description("Прозрачность границы плитки выбранного трека")]
        public byte ColorTileTrackSelectedLineAlpha { get; set; }
    }
}