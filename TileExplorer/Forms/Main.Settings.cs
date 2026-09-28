using GMap.NET.WindowsForms;
using P3tr0viCh.Utils.Interfaces;
using System.Drawing;
using System.Threading.Tasks;

namespace TileExplorer
{
    public partial class Main
    {
        private void UpdateSettings()
        {
            MapItemMarkerCircle.DefaultFill.Color = AppSettings.Roaming.ColorMarkerPosition;

            MapItemMarker.DefaultFill.Color = Color.FromArgb(
                AppSettings.Roaming.ColorMarkerFillAlpha,
                AppSettings.Roaming.ColorMarkerFill);
            MapItemMarker.DefaultSelectedFill.Color = Color.FromArgb(
                AppSettings.Roaming.ColorMarkerSelectedFillAlpha,
                AppSettings.Roaming.ColorMarkerSelectedFill);

            MapItemMarker.DefaultStroke.Color = Color.FromArgb(
                AppSettings.Roaming.ColorMarkerLineAlpha,
                AppSettings.Roaming.ColorMarkerLine);
            MapItemMarker.DefaultStroke.Width = AppSettings.Roaming.WidthMarkerLine;

            MapItemMarker.DefaultSelectedStroke.Color = Color.FromArgb(
                AppSettings.Roaming.ColorMarkerSelectedLineAlpha,
                AppSettings.Roaming.ColorMarkerSelectedLine);
            MapItemMarker.DefaultSelectedStroke.Width = AppSettings.Roaming.WidthMarkerLineSelected;

            ((SolidBrush)GMapToolTip.DefaultFill).Color = Color.FromArgb(
                   AppSettings.Roaming.ColorMarkerTextFillAlpha,
                   AppSettings.Roaming.ColorMarkerTextFill);

            ((SolidBrush)GMapToolTip.DefaultForeground).Color = Color.FromArgb(
                AppSettings.Roaming.ColorMarkerTextAlpha,
                AppSettings.Roaming.ColorMarkerText);

            GMapRoute.DefaultStroke.Color = Color.FromArgb(
                AppSettings.Roaming.ColorTrackAlpha,
                AppSettings.Roaming.ColorTrack);
            GMapRoute.DefaultStroke.Width = AppSettings.Roaming.WidthTrack;

            MapItemTrack.DefaultSelectedStroke.Color = Color.FromArgb(
                AppSettings.Roaming.ColorTrackSelectedAlpha,
                AppSettings.Roaming.ColorTrackSelected);
            MapItemTrack.DefaultSelectedStroke.Width = AppSettings.Roaming.WidthTrackSelected;

            DataGridViewCellStyles.UpdateSettings();
        }

        private async Task ShowSettingsAsync()
        {
            if (FrmSettings.ShowDlg(this))
            {
                SetDatabase();

                UpdateSettings();

                Utils.Forms.GetChildForms<IUpdateSettings>(ChildFormType.All).ForEach(frm => frm.UpdateSettings());

                await UpdateDataAsync(DataLoad.All, ChildFormType.All);
            }
        }
    }
}