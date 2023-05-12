using Newtonsoft.Json;
using P3tr0viCh.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TileExplorer.Properties
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

    public class AppSettingsBase<T> where T : new()
    {
        public class FormState
        {
            public Rectangle Bounds { get; set; } = default;
            public bool Maximized { get; set; } = false;
        }

        private static T defaultInstance = new T();

        public static T Default
        {
            get
            {
                return defaultInstance;
            }
        }

        private static string directory = string.Empty;
        public static string Directory
        {
            get
            {
                return directory;
            }
            set
            {
                directory = value;
                filePath = string.Empty;
            }
        }

        private static string fileName = string.Empty;
        public static string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                filePath = string.Empty;
            }
        }

        private static string filePath = string.Empty;
        public static string FilePath
        {
            get
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    if (string.IsNullOrEmpty(Directory))
                    {
                        directory = Files.AppDataDirectory();
                    }

                    if (string.IsNullOrEmpty(FileName))
                    {
                        fileName = Files.SettingsFileName();
                    }

                    filePath = Path.Combine(Directory, FileName);
                }

                return filePath;
            }
        }

        public static void SaveFormState(Form form, FormState state)
        {
            if (state.Bounds == default)
            {
                state.Bounds = new Rectangle(
                    (Screen.FromControl(form).WorkingArea.Width - form.Width) / 2,
                    (Screen.FromControl(form).WorkingArea.Height - form.Height) / 2,
                    form.Width, form.Height);
            }

            switch (form.FormBorderStyle)
            {
                case FormBorderStyle.None:
                case FormBorderStyle.Sizable:
                    if (form.WindowState == FormWindowState.Maximized)
                    {
                        state.Maximized = true;
                    }
                    else
                    {
                        state.Maximized = false;

                        state.Bounds = form.Bounds;
                    }

                    break;
                case FormBorderStyle.Fixed3D:
                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.FixedDialog:
                case FormBorderStyle.FixedToolWindow:
                    state.Bounds = new Rectangle(form.Left, form.Top, 0, 0);

                    break;
                case FormBorderStyle.SizableToolWindow:
                    state.Bounds = form.Bounds;

                    break;
                default:
                    break;
            }
        }

        public static void LoadFormState(Form form, FormState state)
        {
            if (state.Bounds == default)
            {
                state.Bounds = new Rectangle(
                    (Screen.FromControl(form).WorkingArea.Width - form.Width) / 2,
                    (Screen.FromControl(form).WorkingArea.Height - form.Height) / 2,
                    form.Width, form.Height);
            }

            form.StartPosition = FormStartPosition.Manual;

            switch (form.FormBorderStyle)
            {
                case FormBorderStyle.None:
                case FormBorderStyle.Sizable:
                    form.Bounds = state.Bounds;

                    if (state.Maximized)
                    {
                        form.WindowState = FormWindowState.Maximized;
                    }

                    break;
                case FormBorderStyle.Fixed3D:
                case FormBorderStyle.FixedSingle:
                case FormBorderStyle.FixedDialog:
                case FormBorderStyle.FixedToolWindow:
                    form.Location = new Point(state.Bounds.Left, state.Bounds.Top);

                    break;
                case FormBorderStyle.SizableToolWindow:
                    form.Bounds = state.Bounds;

                    break;
                default:
                    break;
            }
        }

        public static void SaveDataGridColumns(DataGridView dataGridView, int[] columns)
        {
            if (columns == null)
            {
                columns = new int[dataGridView.Columns.Count];
            }

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                columns[column.Index] = column.Width;
            }
        }

        public static void LoadDataGridColumns(DataGridView dataGridView, int[] columns)
        {
            try
            {
                if (columns == null) return;
                if (columns.Length < dataGridView.Columns.Count) return;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    column.Width = columns[column.Index];
                }
            }
            catch (Exception)
            {
            }
        }

        public void Save()
        {
            try
            {
                if (!System.IO.Directory.Exists(Directory))
                {
                    System.IO.Directory.CreateDirectory(Directory);
                }

                using (var writer = File.CreateText(FilePath))
                {
                    var content = JsonConvert.SerializeObject(defaultInstance, Formatting.Indented);

                    writer.Write(content);
                }
            }
            catch
            {
                Debug.WriteLine("appsettings save");
            }
        }

        public void Load()
        {
            if (!File.Exists(FilePath)) return;

            try
            {
                using (var reader = File.OpenText(FilePath))
                {
                    defaultInstance = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                }
            }
            catch
            {
                Debug.WriteLine("appsettings load");
            }
        }
    }

    public class AppSettings : AppSettingsBase<AppSettings>
    {
        public readonly Database.Filter Filter = new Database.Filter();

        [LocalizedCategory("SettingsCategoryCommon")]
        [DisplayName("Расположение базы данных")]
        [Description("Расположение файла базы данных. Пустое значение – \\AppData\\Roaming\\TileExplorer\\")]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DatabaseHome { get; set; } = string.Empty;

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Фон")]
        [Description("Цвет фона маркера")]
        public Color ColorMarkerFill { get; set; } = Color.FromArgb(44, 165, 126);

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Прозрачность")]
        [Description("Прозрачность фона маркера")]
        public byte ColorMarkerFillAlpha { get; set; } = 220;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Цвет текста")]
        [Description("Цвет текста маркера")]
        public Color ColorMarkerText { get; set; } = Color.FromArgb(44, 165, 126);

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Прозрачность текста")]
        [Description("Прозрачность текста маркера")]
        public byte ColorMarkerTextAlpha { get; set; } = 255;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Цвет фона текста")]
        [Description("Цвет фона текста маркера")]
        public Color ColorMarkerTextFill { get; set; } = Color.LightGray;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Прозрачность фона текста")]
        [Description("Прозрачность фона текста маркера")]
        public byte ColorMarkerTextFillAlpha { get; set; } = 220;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Цвет линий")]
        [Description("Цвет линий маркера")]
        public Color ColorMarkerLine { get; set; } = Color.Gray;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Фон выделенного")]
        [Description("Цвет фона выделенного маркера")]
        public Color ColorMarkerSelectedFill { get; set; } = Color.DarkSlateBlue;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Прозрачность выделенного")]
        [Description("Прозрачность фона выделенного маркера")]
        public byte ColorMarkerSelectedFillAlpha { get; set; } = 220;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Прозрачность линий")]
        [Description("Прозрачность линий маркера")]
        public byte ColorMarkerLineAlpha { get; set; } = 160;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Цвет линий выделенного")]
        [Description("Цвет линий выделенного маркера")]
        public Color ColorMarkerSelectedLine { get; set; } = Color.DarkSlateBlue;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Прозрачность линий выделенного")]
        [Description("Прозрачность линий выделенного маркера")]
        public byte ColorMarkerSelectedLineAlpha { get; set; } = 220;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Толщина линий")]
        [Description("Толщина линий маркера")]
        public int WidthMarkerLine { get; set; } = 1;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Толщина линий выделенного")]
        [Description("Толщина линий выделенного маркера")]
        public int WidthMarkerLineSelected { get; set; } = 2;

        [LocalizedCategory("SettingsCategoryDesignMarkers")]
        [DisplayName("Шрифт")]
        [Description("Шрифт маркера")]
        public Font FontMarker { get; set; } = new Font("Arial", 10);

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Кластер: цвет")]
        [Description("Цвет плитки кластера")]
        public Color ColorTileCluster { get; set; } = Color.FromArgb(220, 220, 20);

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Кластер: прозрачность")]
        [Description("Прозрачность плитки кластера")]
        public byte ColorTileClusterAlpha { get; set; } = 40;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Кластер: прозрачность линии")]
        [Description("Прозрачность границы плитки кластера")]
        public byte ColorTileClusterLineAlpha { get; set; } = 50;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Максимальный кластер: цвет")]
        [Description("Цвет плиток максимального кластера")]
        public Color ColorTileMaxCluster { get; set; } = Color.FromArgb(44, 165, 126);

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Максимальный кластер: прозрачность")]
        [Description("Прозрачность плиток максимального кластера")]
        public byte ColorTileMaxClusterAlpha { get; set; } = 40;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Максимальный кластер: прозрачность линии")]
        [Description("Прозрачность границ плиток максимального кластера")]
        public byte ColorTileMaxClusterLineAlpha { get; set; } = 50;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Квадрат: цвет")]
        [Description("Цвет плиток квадрата")]
        public Color ColorTileMaxSquare { get; set; } = Color.FromArgb(66, 140, 244);

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Квадрат: прозрачность")]
        [Description("Прозрачность плиток квадрата")]
        public byte ColorTileMaxSquareAlpha { get; set; } = 25;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Квадрат: прозрачность линии")]
        [Description("Прозрачность границ плиток квадрата")]
        public byte ColorTileMaxSquareLineAlpha { get; set; } = 50;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Открытая: цвет")]
        [Description("Цвет открытых плиток")]
        public Color ColorTileVisited { get; set; } = Color.Red;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Открытая: прозрачность")]
        [Description("Прозрачность открытых плиток")]
        public byte ColorTileVisitedAlpha { get; set; } = 25;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Открытая: прозрачность линии")]
        [Description("Прозрачность границ открытых плиток")]
        public byte ColorTileVisitedLineAlpha { get; set; } = 25;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Выбранный трек: цвет")]
        [Description("Цвет плитки выбранного трека")]
        public Color ColorTileTrackSelected { get; set; } = Color.DarkSlateBlue;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Выбранный трек: прозрачность")]
        [Description("Прозрачность плитки выбранного трека")]
        public byte ColorTileTrackSelectedAlpha { get; set; } = 144;

        [LocalizedCategory("SettingsCategoryDesignTiles")]
        [DisplayName("Выбранный трек: прозрачность границы")]
        [Description("Прозрачность границы плитки выбранного трека")]
        public byte ColorTileTrackSelectedLineAlpha { get; set; } = 88;

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("SettingsCategoryDesignTracks")]
        [DisplayName("Цвет")]
        [Description("Цвет трека")]
        public Color ColorTrack { get; set; } = Color.Red;

        [LocalizedCategory("SettingsCategoryDesignTracks")]
        [DisplayName("Прозрачность")]
        [Description("Прозрачность трека")]
        public byte ColorTrackAlpha { get; set; } = 144;

        [LocalizedCategory("SettingsCategoryDesignTracks")]
        [DisplayName("Цвет выбранного")]
        [Description("Цвет выбранного трека")]
        public Color ColorTrackSelected { get; set; } = Color.DarkSlateBlue;

        [LocalizedCategory("SettingsCategoryDesignTracks")]
        [DisplayName("Прозрачность выбранного")]
        [Description("Прозрачность выбранного трека")]
        public byte ColorTrackSelectedAlpha { get; set; } = 220;

        [LocalizedCategory("SettingsCategoryDesignTracks")]
        [DisplayName("Толщина")]
        [Description("Толщина трека")]
        public int WidthTrack { get; set; } = 2;

        [LocalizedCategory("SettingsCategoryDesignTracks")]
        [DisplayName("Толщина выделенного")]
        [Description("Толщина выделенного трека")]
        public int WidthTrackSelected { get; set; } = 4;

        [LocalizedCategory("SettingsCategoryDesignTracks")]
        [DisplayName("Расстояние между точками")]
        [Description("Минимальное расстояние между точками трека в метрах")]
        public int TrackMinDistancePoint { get; set; } = 100;

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("SettingsCategoryFormat")]
        [DisplayName("Дата")]
        [Description("Формат даты")]
        public string FormatDate { get; set; } = "yyyy.MM.dd";

        [LocalizedCategory("SettingsCategoryFormat")]
        [DisplayName("Дата и время")]
        [Description("Формат даты и времени")]
        public string FormatDateTime { get; set; } = "yyyy.MM.dd HH:mm";

        [LocalizedCategory("SettingsCategoryFormat")]
        [DisplayName("Расстояние")]
        [Description("Формат расстояний")]
        public string FormatDistance { get; set; } = "0.00";

        [LocalizedCategory("SettingsCategoryFormat")]
        [DisplayName("Координаты")]
        [Description("Формат координат")]
        public string FormatLatLng { get; set; } = "0.000000";

        // ------------------------------------------------------------------------------------------------------------
        [LocalizedCategory("SettingsCategoryOsm")]
        [DisplayName("Ключ")]
        [Description("Ключ линии сетки")]
        public string OsmTileKey { get; set; } = "boundary";

        [LocalizedCategory("SettingsCategoryOsm")]
        [DisplayName("Значение")]
        [Description("Значение ключа линии сетки")]
        public string OsmTileValue { get; set; } = "tile";

        [LocalizedCategory("SettingsCategoryOsm")]
        [DisplayName("Минимальный масштаб")]
        [Description("Минимальный масштаб сетки")]
        public int SaveOsmTileMinZoom { get; set; } = 10;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public double HomeLat { get; set; } = 51.196369;
        [Browsable(false)]
        public double HomeLng { get; set; } = 58.298527;
        [Browsable(false)]
        public int HomeZoom { get; set; } = 11;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public bool MapGrayScale { get; set; } = false;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public byte MouseWheelZoomType { get; set; } = 1;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public string MapRefererUrl { get; set; } = "https://github.com/P3tr0viCh/TileExplorer";

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public bool VisibleGrid { get; set; } = true;
        [Browsable(false)]
        public bool VisibleMarkers { get; set; } = true;
        [Browsable(false)]
        public bool VisibleTracks { get; set; } = true;
        [Browsable(false)]
        public bool VisibleFilter { get; set; } = true;
        [Browsable(false)]
        public bool VisibleMarkerList { get; set; } = true;
        [Browsable(false)]
        public bool VisibleTrackList { get; set; } = true;

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public FormState FormStateMain { get; set; } = new FormState();
        [Browsable(false)]
        public FormState FormStateTrackList { get; set; } = new FormState();
        [Browsable(false)]
        public FormState FormStateMarkerList { get; set; } = new FormState();
        [Browsable(false)]
        public FormState FormStateFilter { get; set; } = new FormState();
        [Browsable(false)]
        public FormState FormStateSettings { get; set; } = new FormState();

        // ------------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public int[] TrackListColumns { get; set; } = null;
        [Browsable(false)]
        public int[] MarkerListColumns { get; set; } = null;
    }
}