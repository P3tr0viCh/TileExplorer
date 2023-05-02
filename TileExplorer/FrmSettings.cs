using Newtonsoft.Json.Linq;
using P3tr0viCh.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TileExplorer.Properties;

namespace TileExplorer
{
    public partial class FrmSettings : Form
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

        private class AppSettings
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

        private readonly AppSettings appSettings = new AppSettings();

        public FrmSettings()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(IWin32Window owner)
        {
            bool Result;

            using (var frm = new FrmSettings())
            {
                Result = frm.ShowDialog(owner) == DialogResult.OK;

                if (Result)
                {
                    Settings.Default.DatabaseHome = frm.appSettings.DatabaseHome;

                    Settings.Default.ColorMarkerFill = frm.appSettings.ColorMarkerFill;
                    Settings.Default.ColorMarkerFillAlpha = frm.appSettings.ColorMarkerFillAlpha;
                    Settings.Default.ColorMarkerText = frm.appSettings.ColorMarkerText;
                    Settings.Default.ColorMarkerTextAlpha = frm.appSettings.ColorMarkerTextAlpha;

                    Settings.Default.FontMarker = frm.appSettings.FontMarker;

                    Settings.Default.Save();
                }
            }

            return Result;
        }

        private void FrmMapDesign_Load(object sender, EventArgs e)
        {
            appSettings.DatabaseHome = Settings.Default.DatabaseHome;

            appSettings.ColorMarkerFill = Settings.Default.ColorMarkerFill;
            appSettings.ColorMarkerFillAlpha = Settings.Default.ColorMarkerFillAlpha;
            appSettings.ColorMarkerText = Settings.Default.ColorMarkerText;
            appSettings.ColorMarkerTextAlpha = Settings.Default.ColorMarkerTextAlpha;

            appSettings.FontMarker = Settings.Default.FontMarker;

            propertyGrid.SelectedObject = appSettings;
        }

        private bool CheckDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return true;

            if (Directory.Exists(path)) return true;

            Msg.Error(string.Format(Resources.ErrorDirectoryNotExists, path));

            return false;
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.PropertyDescriptor.PropertyType == appSettings.DatabaseHome.GetType())
            {
                CheckDirectory((string)e.ChangedItem.Value);
                return;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (CheckDirectory(appSettings.DatabaseHome))
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}