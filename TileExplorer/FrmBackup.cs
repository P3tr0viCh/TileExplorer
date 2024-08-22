using P3tr0viCh.Utils;
using System;
using System.IO;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Backup;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmBackup : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly BackupSettings settings = new BackupSettings();
        private BackupSettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings.Assign(value);

                if (settings.Directory.IsEmpty())
                {
                    settings.Directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                folderBrowserDialog.SelectedPath = settings.Directory;

                tbDirectory.Text = settings.Directory;

                cboxMarkersExcelXml.Checked = settings.FileTypeMarkers.HasFlag(Backup.FileType.ExcelXml);
                cboxMarkersGpx.Checked = settings.FileTypeMarkers.HasFlag(Backup.FileType.Gpx);

                cboxEquipmentsExcelXml.Checked = settings.FileTypeEquipments.HasFlag(Backup.FileType.ExcelXml);
            }
        }

        public FrmBackup()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner)
        {
            using (var frm = new FrmBackup()
            {
                Owner = owner,
            })
            {
                frm.Settings = AppSettings.Default.BackupSettings;

                return frm.ShowDialog(owner) == DialogResult.OK;
            }
        }

        private void BtnDirectory_Click(object sender, System.EventArgs e)
        {
            folderBrowserDialog.SelectedPath = tbDirectory.Text;

            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                tbDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private bool CheckData()
        {
            if (!Directory.Exists(tbDirectory.Text))
            {
                tbDirectory.Focus();

                Msg.Error(Resources.ErrorDirectoryNotExists, tbDirectory.Text);

                return false;
            }

            if (!(cboxMarkersExcelXml.Checked || cboxMarkersGpx.Checked || cboxEquipmentsExcelXml.Checked))
            {
                Msg.Error(Resources.BackupErrorNothingSave);

                return false;
            }

            return true;
        }

        private bool UpdateData()
        {
            try
            {
                settings.Directory = tbDirectory.Text;

                settings.FileTypeMarkers = default;

                if (cboxMarkersExcelXml.Checked)
                {
                    settings.FileTypeMarkers |= Backup.FileType.ExcelXml;
                }
                if (cboxMarkersGpx.Checked)
                {
                    settings.FileTypeMarkers |= Backup.FileType.Gpx;
                }

                settings.FileTypeEquipments = default;

                if (cboxEquipmentsExcelXml.Checked)
                {
                    settings.FileTypeEquipments |= Backup.FileType.ExcelXml;
                }

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);

                return false;
            }
        }

        private bool SaveData()
        {
            try
            {
                AppSettings.Default.BackupSettings = Settings;

                AppSettings.Save();

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);

                return false;
            }
        }

        private bool ApplyData()
        {
            return CheckData() && UpdateData() && SaveData();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (ApplyData())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}