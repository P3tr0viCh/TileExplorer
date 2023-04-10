using System;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;
using static TileExplorer.Main;

namespace TileExplorer
{
    public partial class FrmMarkerList : BaseFrmMarkerList
    {
        public FrmMarkerList(IMainForm mainForm) : base(mainForm)
        {
            InitializeComponent();
        }

        public override DataGridView DataGridView => dataGridView;
        public override DataGridViewColumn ColumnFind => ColumnId;
        public override DataGridViewColumn ColumnSelect => ColumnText;

        private void FrmMarkerList_Load(object sender, EventArgs e)
        {
#if !DEBUG
            ColumnId.Visible = false;
#endif
        }

        public override void Set(int rowIndex, MarkerModel marker)
        {
            dataGridView.Rows[rowIndex].Cells[ColumnId.Name].Value = marker.Id;

            dataGridView.Rows[rowIndex].Cells[ColumnText.Name].Value = marker.Text;

            dataGridView.Rows[rowIndex].Cells[ColumnLat.Name].Value = string.Format(Resources.CoordFmt, marker.Lat);
            dataGridView.Rows[rowIndex].Cells[ColumnLng.Name].Value = string.Format(Resources.CoordFmt, marker.Lng);
        }
    }
}