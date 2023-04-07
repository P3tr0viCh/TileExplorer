using Newtonsoft.Json.Linq;
using P3tr0viCh;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;
using static TileExplorer.Main;

namespace TileExplorer
{
    public partial class FrmMarkerList : Form
    {
        readonly IMainForm MainForm;

        public FrmMarkerList(IMainForm mainForm)
        {
            InitializeComponent();

            MainForm = mainForm;
        }

        private void FrmMarkerList_Load(object sender, System.EventArgs e)
        {
#if !DEBUG
            ColumnId.Visible = false;
#endif
            SettingsExt.LoadFormPosition(this);
        }

        private void FrmMarkerList_FormClosed(object sender, FormClosedEventArgs e)
        {
            SettingsExt.SaveFormPosition(this);

            Settings.Default.Save();
        }

        private void FrmMarkerList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private int FindMarker(MarkerModel marker)
        {
            if (marker == null) return -1;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if ((long)row.Cells[ColumnId.Name].Value == marker.Id)
                {
                    return row.Index;
                }
            }

            return -1;
        }

        private void SetMarker(int rowIndex, MarkerModel marker)
        {
            dataGridView.Rows[rowIndex].Cells[ColumnId.Name].Value = marker.Id;

            dataGridView.Rows[rowIndex].Cells[ColumnText.Name].Value = marker.Text;

            dataGridView.Rows[rowIndex].Cells[ColumnLat.Name].Value = string.Format("{0:F6}", marker.Lat);
            dataGridView.Rows[rowIndex].Cells[ColumnLng.Name].Value = string.Format("{0:F6}", marker.Lng);
        }

        public void AddMarker(MarkerModel marker)
        {
            SetMarker(dataGridView.Rows.Add(), marker);
        }

        public void UpdateMarker(MarkerModel marker)
        {
            int rowIndex = FindMarker(marker);

            if (rowIndex != -1) return;

            SetMarker(rowIndex, marker);
        }

        public List<MarkerModel> Markers
        {
            set
            {
                dataGridView.Rows.Clear();

                foreach (var marker in value)
                {
                    AddMarker(marker);
                }
            }
        }

        public MarkerModel SelectedMarker
        {
            set
            {
                int rowIndex = FindMarker(value);

                if (rowIndex == -1) return;

                if (rowIndex == dataGridView.CurrentCell.RowIndex) return;

                dataGridView.CurrentCell = dataGridView[ColumnText.DisplayIndex, rowIndex];
            }
        }

        private void DataGridView_SelectionChanged(object sender, System.EventArgs e)
        {
            if (dataGridView.SelectedCells.Count == 0) return;

            var cellValue = dataGridView[ColumnId.Index, dataGridView.SelectedCells[0].RowIndex].Value;

            if (cellValue == null) return;

            MainForm.SelectMarkerById((long)cellValue);
        }
    }
}