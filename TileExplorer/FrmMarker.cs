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

        public static bool Show(IWin32Window owner, MarkerModel markerModel)
        {
            bool Result;

            using (var frmMarker = new FrmMarker())
            {
                frmMarker.tbText.Text = markerModel.Text;

                frmMarker.udPointLat.Value = (decimal)markerModel.Lat;
                frmMarker.udPointLng.Value = (decimal)markerModel.Lng;

                frmMarker.udOffsetX.Value = markerModel.OffsetX;
                frmMarker.udOffsetY.Value = markerModel.OffsetY;

                frmMarker.cboxTextVisible.Checked = markerModel.IsTextVisible;

                Result = frmMarker.ShowDialog(owner) == DialogResult.OK;

                if (Result)
                {
                    markerModel.Text = frmMarker.tbText.Text;

                    markerModel.Lat = (double)frmMarker.udPointLat.Value;
                    markerModel.Lng = (double)frmMarker.udPointLng.Value;

                    markerModel.OffsetX = (int)frmMarker.udOffsetX.Value;
                    markerModel.OffsetY = (int)frmMarker.udOffsetY.Value;

                    markerModel.IsTextVisible = frmMarker.cboxTextVisible.Checked;
                }
            }

            return Result;
        }
    }
}