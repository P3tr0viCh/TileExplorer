using System.Collections.Generic;
using System.Windows.Forms;
using static TileExplorer.Database;

namespace TileExplorer
{
    public partial class FrmTrackList : Form
    {
        public FrmTrackList()
        {
            InitializeComponent();
        }

        private void FrmTrackList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public List<TrackModel> Tracks
        {
            set
            {
                dataGridView.Rows.Clear();

                foreach (var track in value)
                {
                    dataGridView.Rows.Add(
                        track.Text, track.DateTime, track.Distance / 1000.0
                    );
                }
            }
        }
    }
}
