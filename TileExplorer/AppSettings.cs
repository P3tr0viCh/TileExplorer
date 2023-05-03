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

        [LocalizedCategory("SettingsCategoryDesign")]
        [DisplayName("Маркер: фон")]
        [Description("Цвет фона маркера")]
        public Color ColorMarkerFill { get; set; }

        [LocalizedCategory("SettingsCategoryDesign")]
        [DisplayName("Маркер: прозрачность")]
        [Description("Прозрачность фона маркера")]
        public byte ColorMarkerFillAlpha { get; set; }

        [LocalizedCategory("SettingsCategoryDesign")]
        [DisplayName("Маркер: цвет текста")]
        [Description("Цвет текста маркера")]
        public Color ColorMarkerText { get; set; }

        [LocalizedCategory("SettingsCategoryDesign")]
        [DisplayName("Маркер: прозрачность текста")]
        [Description("Прозрачность текста маркера")]
        public byte ColorMarkerTextAlpha { get; set; }

        [LocalizedCategory("SettingsCategoryDesign")]
        [DisplayName("Маркер: шрифт")]
        [Description("Шрифт маркера")]
        public Font FontMarker { get; set; }
    }
}