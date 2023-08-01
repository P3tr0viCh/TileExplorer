using System;
using System.ComponentModel;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;

namespace TileExplorer
{
    public class FrmTrackList : FrmListBase<Models.Track>
    {
        public override FrmListType Type => FrmListType.Tracks;

        private readonly DataGridViewTextBoxColumn ColumnText = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnDateTimeStart = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnDateTimeFinish = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnDuration = new DataGridViewTextBoxColumn();
        private readonly DataGridViewTextBoxColumn ColumnDistance = new DataGridViewTextBoxColumn();

        public FrmTrackList(Form owner) : base(owner)
        {
            Name = "FrmTrackList";
        }

        public override void Set(int rowIndex, Models.Track model)
        {
            DataGridView.Rows[rowIndex].Cells[ColumnId.Name].Value = model.Id;

            DataGridView.Rows[rowIndex].Cells[ColumnText.Name].Value = model.Text;

            DataGridView.Rows[rowIndex].Cells[ColumnDateTimeStart.Name].Value = model.DateTimeStart;
            DataGridView.Rows[rowIndex].Cells[ColumnDateTimeFinish.Name].Value = model.DateTimeFinish;

            DataGridView.Rows[rowIndex].Cells[ColumnDuration.Name].Value = new DateTime(model.Duration.Ticks);

            DataGridView.Rows[rowIndex].Cells[ColumnDistance.Name].Value = model.Distance / 1000.0;
        }

        public override void InitializeComponent()
        {
            Text = "Треки";

            ColumnText.HeaderText = "Название";
            ColumnText.Name = "ColumnText";
            ColumnText.ReadOnly = true;
            ColumnText.Width = 144;

            ColumnDateTimeStart.DefaultCellStyle = new DataGridViewCellStyle()
            {
                NullValue = null,
                Format = AppSettings.Default.FormatDateTime
            };
            ColumnDateTimeStart.HeaderText = "Начало";
            ColumnDateTimeStart.Name = "ColumnDateTimeStart";
            ColumnDateTimeStart.ReadOnly = true;
            ColumnDateTimeStart.Width = 144;

            ColumnDateTimeFinish.DefaultCellStyle = new DataGridViewCellStyle()
            {
                NullValue = null,
                Format = AppSettings.Default.FormatDateTime
            };
            ColumnDateTimeFinish.HeaderText = "Окончание";
            ColumnDateTimeFinish.Name = "ColumnDateTimeFinish";
            ColumnDateTimeFinish.ReadOnly = true;
            ColumnDateTimeFinish.Width = 144;

            ColumnDuration.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.TopRight,
                NullValue = null,
                Format = AppSettings.Default.FormatTime
            };
            ColumnDuration.HeaderText = "Время";
            ColumnDuration.Name = "ColumnDuration";
            ColumnDuration.ReadOnly = true;

            ColumnDistance.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.TopRight,
                NullValue = null,
                Format = AppSettings.Default.FormatDistance
            };
            ColumnDistance.HeaderText = "Расстояние";
            ColumnDistance.Name = "ColumnDistance";
            ColumnDistance.ReadOnly = true;

            DataGridView.Columns.AddRange(
                new DataGridViewColumn[] { ColumnText, ColumnDateTimeStart, ColumnDateTimeFinish, ColumnDuration, ColumnDistance });

            DataGridView.Sort(ColumnDateTimeStart, ListSortDirection.Ascending);
        }
    }
}