using System.Windows.Forms;
using static TileExplorer.Database;

namespace TileExplorer
{
    public partial class FrmMarker : Form
    {
        public FrmMarker()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(IWin32Window owner, MarkerModel marker)
        {
            bool Result;

            using (var frm = new FrmMarker())
            {
                frm.tbText.Text = marker.Text;

                frm.udPointLat.Value = (decimal)marker.Lat;
                frm.udPointLng.Value = (decimal)marker.Lng;

                frm.udOffsetX.Value = marker.OffsetX;
                frm.udOffsetY.Value = marker.OffsetY;

                frm.cboxTextVisible.Checked = marker.IsTextVisible;

                Result = frm.ShowDialog(owner) == DialogResult.OK;

                if (Result)
                {
                    marker.Text = frm.tbText.Text;

                    marker.Lat = (double)frm.udPointLat.Value;
                    marker.Lng = (double)frm.udPointLng.Value;

                    marker.OffsetX = (int)frm.udOffsetX.Value;
                    marker.OffsetY = (int)frm.udOffsetY.Value;

                    marker.IsTextVisible = frm.cboxTextVisible.Checked;
                }
            }

            return Result;
        }
    }
}