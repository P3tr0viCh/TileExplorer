using P3tr0viCh.Utils;
using System;
using System.Diagnostics;
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
            get
            {
                return marker;
            }
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
                Debug.WriteLine(e);

                Msg.Error(e.Message);

                return false;
            }
        }

        private bool SaveData()
        {
            Task.Run(() => Database.Default.MarkerSaveAsync(Marker)).Wait();

            MainForm.MarkerChanged(Marker);

            return true;
        }

        private bool ApplyData()
        {
            return UpdateData() && SaveData();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (ApplyData())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            ApplyData();
        }
    }
}