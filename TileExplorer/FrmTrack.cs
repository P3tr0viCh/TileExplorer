using System.Windows.Forms;
using static TileExplorer.Database;
using static TileExplorer.Database.Models;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmTrack : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        public FrmTrack()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, Track track)
        {
            bool Result;

            using (var frm = new FrmTrack()
            {
                Owner = owner,
            })
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