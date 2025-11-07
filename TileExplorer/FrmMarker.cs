using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TileExplorer.Database.Models;
using static TileExplorer.Interfaces;

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

                tbText.Text = marker.Text;

                udPointLat.Value = (decimal)marker.Lat;
                udPointLng.Value = (decimal)marker.Lng;

                udOffsetX.Value = marker.OffsetX;
                udOffsetY.Value = marker.OffsetY;

                cboxTextVisible.Checked = marker.IsTextVisible;
            }
        }

        public FrmMarker()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, Marker marker)
        {
            using (var frm = new FrmMarker()
            {
                Owner = owner,

                Marker = marker
            })
            {
                return frm.ShowDialog(owner) == DialogResult.OK;
            }
        }

        private bool UpdateData()
        {
            try
            {
                marker.Text = tbText.Text;

                marker.Lat = (double)udPointLat.Value;
                marker.Lng = (double)udPointLng.Value;

                marker.OffsetX = (int)udOffsetX.Value;
                marker.OffsetY = (int)udOffsetY.Value;

                marker.IsTextVisible = cboxTextVisible.Checked;

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
            if (await Database.Actions.MarkerSaveAsync(Marker))
            {
                await MainForm.MarkerChangedAsync(Marker);

                return true;
            }

            return false;
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