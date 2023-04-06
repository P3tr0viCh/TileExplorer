using System.Collections.Generic;
using System.Windows.Forms;
using static TileExplorer.Database;

namespace TileExplorer
{
    public partial class FrmMarkerList : Form
    {
        public FrmMarkerList()
        {
            InitializeComponent();
        }

        private void FrmMarkerList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public List<MarkerModel> Markers
        {
            set
            {
                dataGridView.Rows.Clear();

                foreach (var marker in value)
                {
                    dataGridView.Rows.Add(
                        marker.Text, string.Format("{0:F6}", marker.Lat), string.Format("{0:F6}", marker.Lng)
                    );
                }
            }
        }
    }
}