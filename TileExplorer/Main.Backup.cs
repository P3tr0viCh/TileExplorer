using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using TileExplorer.Properties;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private async Task BackupSaveAsync()
        {
            if (ProgramStatus.Contains(Status.BackupSave) || ProgramStatus.Contains(Status.BackupLoad))
            {
                Msg.Info(Resources.BackupInfoInProgress);

                return;
            }

            var settings = new Backup.BackupSettings()
            {
                Directory = AppSettings.Local.Default.DirectoryBackups,
                NameUseDate = true,
                FileNames = Backup.FileName.MarkersExcelXml | Backup.FileName.MarkersGpx |
                    Backup.FileName.EquipmentsExcelXml |
                    Backup.FileName.LocalSettings |
                    Backup.FileName.RoamingSettings |
                    Backup.FileName.TrackExts,
            };

            if (!FrmBackup.ShowDlg(this, settings, FrmBackup.BackupAction.Save))
            {
                return;
            }

            var status = ProgramStatus.Start(Status.BackupSave);

            bool result;
            string resultMessage;

            try
            {
                var backup = new Backup(settings);

                await backup.SaveAsync();

                result = true;
                resultMessage = string.Format(Resources.BackupSaveOk, backup.FullPath);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                result = false;
                resultMessage = string.Format(Resources.BackupSaveFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }

            Utils.MsgResult(result, resultMessage);
        }

        private async Task BackupLoadAsync()
        {
            if (ProgramStatus.Contains(Status.BackupSave) || ProgramStatus.Contains(Status.BackupLoad))
            {
                Msg.Info(Resources.BackupInfoInProgress);

                return;
            }

            var settings = new Backup.BackupSettings()
            {
#if DEBUG
                Name = "2025-11-11",
#endif
                Directory = AppSettings.Local.Default.DirectoryBackups,
                FileNames = Backup.FileName.MarkersExcelXml | Backup.FileName.EquipmentsExcelXml,
            };

            if (!FrmBackup.ShowDlg(this, settings, FrmBackup.BackupAction.Load))
            {
                return;
            }

            var status = ProgramStatus.Start(Status.BackupLoad);

            bool result;
            string resultMessage;

            try
            {
                var backup = new Backup(settings);

                await backup.LoadAsync();

                result = true;
                resultMessage = string.Format(Resources.BackupLoadOk, backup.FullPath);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                result = false;
                resultMessage = string.Format(Resources.BackupLoadFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }

            await UpdateDataAsync();

            Utils.MsgResult(result, resultMessage);
        }
    }
}