using GMap.NET.WindowsForms;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class Main
    {
        private void UpdateSettings()
        {
            MapItemMarkerCircle.DefaultFill.Color = AppSettings.Roaming.Default.ColorMarkerPosition;

            MapItemMarker.DefaultFill.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerFillAlpha,
                AppSettings.Roaming.Default.ColorMarkerFill);
            MapItemMarker.DefaultSelectedFill.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerSelectedFillAlpha,
                AppSettings.Roaming.Default.ColorMarkerSelectedFill);

            MapItemMarker.DefaultStroke.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerLineAlpha,
                AppSettings.Roaming.Default.ColorMarkerLine);
            MapItemMarker.DefaultStroke.Width = AppSettings.Roaming.Default.WidthMarkerLine;

            MapItemMarker.DefaultSelectedStroke.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerSelectedLineAlpha,
                AppSettings.Roaming.Default.ColorMarkerSelectedLine);
            MapItemMarker.DefaultSelectedStroke.Width = AppSettings.Roaming.Default.WidthMarkerLineSelected;

            ((SolidBrush)GMapToolTip.DefaultFill).Color = Color.FromArgb(
                   AppSettings.Roaming.Default.ColorMarkerTextFillAlpha,
                   AppSettings.Roaming.Default.ColorMarkerTextFill);

            ((SolidBrush)GMapToolTip.DefaultForeground).Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorMarkerTextAlpha,
                AppSettings.Roaming.Default.ColorMarkerText);

            GMapRoute.DefaultStroke.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorTrackAlpha,
                AppSettings.Roaming.Default.ColorTrack);
            GMapRoute.DefaultStroke.Width = AppSettings.Roaming.Default.WidthTrack;

            MapItemTrack.DefaultSelectedStroke.Color = Color.FromArgb(
                AppSettings.Roaming.Default.ColorTrackSelectedAlpha,
                AppSettings.Roaming.Default.ColorTrackSelected);
            MapItemTrack.DefaultSelectedStroke.Width = AppSettings.Roaming.Default.WidthTrackSelected;

            DataGridViewCellStyles.UpdateSettings();
        }

        private async Task ShowSettingsAsync()
        {
            var frmSettings = new FrmSettings(new AppSettings());

            if (frmSettings.ShowDialog(this))
            {
                if (!SetDatabaseFileName())
                {
                    AbnormalExit = true;
                    Application.Exit();
                    return;
                }

                UpdateSettings();

                foreach (var frm in Application.OpenForms)
                {
                    if (frm is IChildForm form)
                    {
                        form.UpdateSettings();
                    }
                }

                await UpdateDataAsync();
            }
        }
    }
}