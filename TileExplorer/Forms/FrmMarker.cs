using GMap.NET;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public partial class FrmMarker : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly Marker marker = new Marker();

        private Marker Marker
        {
            get => marker;
            set
            {
                marker.Assign(value);

                tbText.SetText(marker.Text);

                udPointLat.Value = (decimal)marker.Lat;
                udPointLng.Value = (decimal)marker.Lng;

                udOffsetX.Value = marker.OffsetX;
                udOffsetY.Value = marker.OffsetY;

                cboxTextVisible.SetBool(marker.IsTextVisible);
            }
        }

        public FrmMarker()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, Marker marker)
        {
#if DEBUG
            if (marker.IsNew)
            {
                marker.Text = DateTime.Now.ToString();
            }
#endif
            using (var frm = new FrmMarker()
            {
                Owner = owner,

                Marker = marker
            })
            {
                if (marker.IsNew)
                {
                    frm.MainForm.ShowMarkerNewPosition(marker.LatLng);
                }

                var result = frm.ShowDialog(owner);

                frm.MainForm.ShowMarkerNewPosition(default);

                if (result != DialogResult.OK) return false;

                marker.Assign(frm.Marker);

                return true;
            }
        }

        private bool UpdateData()
        {
            try
            {
                marker.Text = tbText.GetTrimText();

                marker.Lat = (double)udPointLat.Value;
                marker.Lng = (double)udPointLng.Value;

                marker.OffsetX = (int)udOffsetX.Value;
                marker.OffsetY = (int)udOffsetY.Value;

                marker.IsTextVisible = cboxTextVisible.GetBool();

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);

                return false;
            }
        }

        private async Task<bool> SaveDataAsync()
        {
            var result = await Database.Actions.ListItemSaveAsync(Marker);

            if (result)
            {
                MainForm.ShowMarkerNewPosition(default);

                await MainForm.MarkerChangedAsync(Marker);
            }

            return result;
        }

        private async Task<bool> ApplyDataAsync()
        {
            return UpdateData() && await SaveDataAsync();
        }

        private async void BtnOk_Click(object sender, EventArgs e)
        {
            if (await ApplyDataAsync())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private async void BtnApply_Click(object sender, EventArgs e)
        {
            await ApplyDataAsync();
        }
    }
}