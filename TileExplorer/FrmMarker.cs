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

        public static bool ShowDlg(IWin32Window owner, MarkerModel markerModel)
        {
            bool Result;

            using (var frm = new FrmMarker())
            {
                frm.tbText.Text = markerModel.Text;

                frm.udPointLat.Value = (decimal)markerModel.Lat;
                frm.udPointLng.Value = (decimal)markerModel.Lng;

                frm.udOffsetX.Value = markerModel.OffsetX;
                frm.udOffsetY.Value = markerModel.OffsetY;

                frm.cboxTextVisible.Checked = markerModel.IsTextVisible;

                Result = frm.ShowDialog(owner) == DialogResult.OK;

                if (Result)
                {
                    markerModel.Text = frm.tbText.Text;

                    markerModel.Lat = (double)frm.udPointLat.Value;
                    markerModel.Lng = (double)frm.udPointLng.Value;

                    markerModel.OffsetX = (int)frm.udOffsetX.Value;
                    markerModel.OffsetY = (int)frm.udOffsetY.Value;

                    markerModel.IsTextVisible = frm.cboxTextVisible.Checked;
                }
            }

            return Result;
        }
    }
}