using System;
using System.Windows.Forms;
using static TileExplorer.Database;
using static TileExplorer.Main;

namespace TileExplorer
{
    public partial class FrmTrackList : BaseFrmTrackList
    {
        public FrmTrackList(IMainForm mainForm) : base(mainForm)
        {
            InitializeComponent();
        }

        public override DataGridView DataGridView => dataGridView;
        public override DataGridViewColumn ColumnFind => ColumnId;

        private void FrmTrackList_Load(object sender, EventArgs e)
        {
#if !DEBUG
            ColumnId.Visible = false;
#endif
        }

        public override void Set(int rowIndex, TrackModel track)
        {
            dataGridView.Rows[rowIndex].Cells[ColumnId.Name].Value = track.Id;

            dataGridView.Rows[rowIndex].Cells[ColumnText.Name].Value = track.Text;

            dataGridView.Rows[rowIndex].Cells[ColumnDateTime.Name].Value = track.DateTime;

            dataGridView.Rows[rowIndex].Cells[ColumnDistance.Name].Value = track.Distance / 1000.0;
        }
    }
}