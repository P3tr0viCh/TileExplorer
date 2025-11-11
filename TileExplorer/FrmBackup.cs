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
                AppSettings.LocalSave();

                frm.SetData();

                if (frm.ShowDialog(owner) == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    AppSettings.LocalLoad();

                    return false;
                }
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

        private void SetData()
        {
            var settings = AppSettings.Local.Default.BackupSettings;

            folderBrowserDialog.SelectedPath = settings.Directory;

            tbDirectory.Text = settings.Directory;

            cboxMarkersExcelXml.Checked = settings.Markers.HasFlag(FileType.ExcelXml);
            cboxMarkersGpx.Checked = settings.Markers.HasFlag(FileType.Gpx);

            cboxEquipmentsExcelXml.Checked = settings.Equipments.HasFlag(FileType.ExcelXml);

            cboxLocalSettings.Checked = settings.LocalSettings;
            cboxRoamingSettings.Checked = settings.RoamingSettings;
        }

        private bool CheckData()
        {
            if (!Directory.Exists(tbDirectory.Text))
            {
                tbDirectory.Focus();

                Msg.Error(Resources.ErrorDirectoryNotExists, tbDirectory.Text);

                return false;
            }

            if (!(cboxMarkersExcelXml.Checked || cboxMarkersGpx.Checked ||
                cboxEquipmentsExcelXml.Checked ||
                cboxLocalSettings.Checked || cboxRoamingSettings.Checked))
            {
                Msg.Error(Resources.BackupErrorNothingSave);

                return false;
            }

            return true;
        }

        private bool UpdateData()
        {
            var settings = AppSettings.Local.Default.BackupSettings;

            settings.Directory = tbDirectory.Text;

            settings.Markers = default;

            if (cboxMarkersExcelXml.Checked)
            {
                settings.Markers |= FileType.ExcelXml;
            }
            if (cboxMarkersGpx.Checked)
            {
                settings.Markers |= FileType.Gpx;
            }

            settings.Equipments = default;

            if (cboxEquipmentsExcelXml.Checked)
            {
                settings.Equipments |= FileType.ExcelXml;
            }

            settings.LocalSettings = cboxLocalSettings.Checked;
            settings.RoamingSettings = cboxRoamingSettings.Checked;

            return true;
        }

        private bool SaveData()
        {
            if (!AppSettings.LocalSave())
            {
                Msg.Error(AppSettings.LastError.Message);

                return false;
            }

            return true;
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