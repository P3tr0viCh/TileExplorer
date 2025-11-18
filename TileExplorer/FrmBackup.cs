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

        private BackupSettings Settings { get; set; }

        public enum BackupAction
        {
            Save,
            Load
        }

        private BackupAction Action { get; set; }

        private bool IsActionLoad => Action == BackupAction.Load;

        public FrmBackup()
        {
            InitializeComponent();
        }

        public FrmBackup(Form owner) : this() => Owner = owner;

        public static bool ShowDlg(Form owner, BackupSettings settings, BackupAction action)
        {
            using (var frm = new FrmBackup(owner))
            {
                frm.Settings = settings;

                frm.Action = action;

                frm.Text = frm.IsActionLoad ? Resources.TitleBackupLoad : Resources.TitleBackupSave;

                frm.SetData();

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    AppSettings.Local.Default.DirectoryBackups = frm.Settings.Directory;
                }

                return result;
            }
        }

        private void SetData()
        {
            folderBrowserDialog.SelectedPath = Settings.Directory;

            tbDirectory.Text = Settings.Directory;

            cboxMarkersExcelXml.Checked = Settings.FileNames.HasFlag(FileName.MarkersExcelXml);
            cboxMarkersGpx.Checked = Settings.FileNames.HasFlag(FileName.MarkersGpx);

            cboxEquipmentsExcelXml.Checked = Settings.FileNames.HasFlag(FileName.EquipmentsExcelXml);

            cboxLocalSettings.Checked = Settings.FileNames.HasFlag(FileName.LocalSettings);
            cboxRoamingSettings.Checked = Settings.FileNames.HasFlag(FileName.RoamingSettings);

            cboxTrackExts.Checked = Settings.FileNames.HasFlag(FileName.TrackExts);

            if (Action == BackupAction.Save)
            {
                cboxNameIsDate.Checked = Settings.NameUseDate;

                if (!cboxNameIsDate.Checked)
                {
                    tbName.Text = Settings.Name;
                }
            }
            else
            {
                folderBrowserDialog.ShowNewFolderButton = false;

                tbName.Text = Settings.Name;

                cboxNameIsDate.Visible = false;

                cboxMarkersGpx.Enabled = false;

                cboxLocalSettings.Enabled = false;
                cboxRoamingSettings.Enabled = false;
            }
        }

        private void UpdateFileNames()
        {
            Settings.FileNames = default;

            if (cboxMarkersExcelXml.Checked) Settings.FileNames |= FileName.MarkersExcelXml;
            if (cboxMarkersGpx.Checked) Settings.FileNames |= FileName.MarkersGpx;

            if (cboxEquipmentsExcelXml.Checked) Settings.FileNames |= FileName.EquipmentsExcelXml;

            if (cboxLocalSettings.Checked) Settings.FileNames |= FileName.LocalSettings;

            if (cboxRoamingSettings.Checked) Settings.FileNames |= FileName.RoamingSettings;

            if (cboxTrackExts.Checked) Settings.FileNames |= FileName.TrackExts;
        }

        private bool CheckData()
        {
            var directory = tbDirectory.Text;

            if (!Directory.Exists(directory))
            {
                Utils.Forms.TextBoxWrongValue(tbDirectory, Resources.ErrorDirectoryNotExists, directory);

                return false;
            }

            if (Utils.Forms.CheckTextBoxIsEmpty(tbName, Resources.BackupErrorNameEmpty))
            {
                return false;
            }

            var name = tbName.Text;

            var backupDirectory = Path.Combine(directory, name);

            if (IsActionLoad && !Directory.Exists(backupDirectory))
            {
                Utils.Forms.TextBoxWrongValue(tbName, Resources.ErrorDirectoryNotExists, backupDirectory);

                return false;
            }

            UpdateFileNames();

            if (Settings.FileNames == default)
            {
                Msg.Error(IsActionLoad ? Resources.BackupErrorNothingLoad : Resources.BackupErrorNothingSave);

                return false;
            }

            if (IsActionLoad)
            {
                if (cboxMarkersExcelXml.Checked)
                {
                    if (!File.Exists(GetFullFileName(backupDirectory, FileName.MarkersExcelXml)))
                    {
                        Msg.Error(Resources.BackupErrorFileMarkersNotExists);
                        return false;
                    }
                }

                if (cboxEquipmentsExcelXml.Checked)
                {
                    if (!File.Exists(GetFullFileName(backupDirectory, FileName.EquipmentsExcelXml)))
                    {
                        Msg.Error(Resources.BackupErrorFileEquipmentsNotExists);
                        return false;
                    }
                }

                if (cboxTrackExts.Checked)
                {
                    if (!File.Exists(GetFullFileName(backupDirectory, FileName.TrackExts)))
                    {
                        Msg.Error(Resources.BackupErrorFileTrackExtsNotExists);
                        return false;
                    }
                }
            }

            return true;
        }

        private bool UpdateData()
        {
            Settings.Directory = tbDirectory.Text;
            Settings.Name = tbName.Text;

            Settings.NameUseDate = cboxNameIsDate.Checked;

            return true;
        }

        private bool ApplyData() => CheckData() && UpdateData();

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (ApplyData())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void CboxNameIsDate_CheckedChanged(object sender, EventArgs e)
        {
            if (cboxNameIsDate.Checked)
            {
                tbName.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

            tbName.ReadOnly = cboxNameIsDate.Checked;
            btnName.Enabled = !cboxNameIsDate.Checked;
        }

        private void BtnDirectory_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = tbDirectory.Text;

            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                tbDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void BtnName_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = tbDirectory.Text;

            if (folderBrowserDialog.ShowDialog(this) != DialogResult.OK) return;

            var path = folderBrowserDialog.SelectedPath;

            tbDirectory.Text = Path.GetDirectoryName(path);

            tbName.Text = Path.GetFileName(path);

            cboxMarkersExcelXml.Checked = File.Exists(GetFullFileName(path, FileName.MarkersExcelXml));
            cboxEquipmentsExcelXml.Checked = File.Exists(GetFullFileName(path, FileName.EquipmentsExcelXml));

            cboxTrackExts.Checked = File.Exists(GetFullFileName(path, FileName.TrackExts));
        }
    }
}