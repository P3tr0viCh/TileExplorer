using System.Windows.Forms;
using static TileExplorer.Database;

namespace TileExplorer
{
    public partial class FrmTrack : Form
    {
        public FrmTrack()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(IWin32Window owner, TrackModel track)
        {
            bool Result;

            using (var frm = new FrmTrack())
            {
                frm.tbText.Text = track.Text;

                Result = frm.ShowDialog(owner) == DialogResult.OK;

                if (Result)
                {
                    track.Text = frm.tbText.Text;
                }
            }

            return Result;
        }
    }
}
