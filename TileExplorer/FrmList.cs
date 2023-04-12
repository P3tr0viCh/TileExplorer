using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using static TileExplorer.Database;
using static TileExplorer.Main;

namespace TileExplorer
{
    public class BaseFrmTrackList : FrmList<TrackModel>
    {
        public override DataGridView DataGridView => null;
        public override DataGridViewColumn ColumnFind => null;

        public BaseFrmTrackList() : base(null)
        {
        }

        public BaseFrmTrackList(IMainForm mainForm) : base(mainForm)
        {
        }

        public override void Set(int rowIndex, TrackModel model)
        {
            throw new NotImplementedException();
        }
    }

    public class BaseFrmMarkerList : FrmList<MarkerModel>
    {
        public override DataGridView DataGridView => null;
        public override DataGridViewColumn ColumnFind => null;

        public BaseFrmMarkerList() : base(null)
        {
        }

        public BaseFrmMarkerList(IMainForm mainForm) : base(mainForm)
        {
        }

        public override void Set(int rowIndex, MarkerModel model)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class FrmList<T> : Form where T : BaseModelId
    {
        public IMainForm MainForm;

        public abstract DataGridView DataGridView { get; }
        public abstract DataGridViewColumn ColumnFind { get; }

        protected FrmList(IMainForm mainForm)
        {
            MainForm = mainForm;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Load += new EventHandler(FrmList_Load);
            FormClosing += new FormClosingEventHandler(FrmList_FormClosing);
        }

        public int Find(T value)
        {
            if (value == null) return -1;

            foreach (DataGridViewRow row in DataGridView.Rows)
            {
                if ((long)row.Cells[ColumnFind.Name].Value == value.Id)
                {
                    return row.Index;
                }
            }

            return -1;
        }

        public T Selected
        {
            set
            {
                if (!Visible) return;

                int rowIndex = Find(value);

                if (rowIndex == -1) return;

                if (rowIndex == DataGridView.CurrentCell.RowIndex) return;

                DataGridView.CurrentCell = DataGridView[
                    DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Displayed).Index, rowIndex];
            }
        }

        public abstract void Set(int rowIndex, T model);

        public void Add(T model)
        {
            Set(DataGridView.Rows.Add(), model);
        }
        
        public void Update(T model)
        {
            Set(Find(model), model);

            DataGridView.Sort(DataGridView.SortedColumn, 
                DataGridView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending: ListSortDirection.Descending);
        }
        
        public void Delete(T model)
        {
            int rowIndex = Find(model);

            if (rowIndex == -1) return;

            DataGridView.Rows.RemoveAt(rowIndex);
        }

        public List<T> List
        {
            set
            {
                DataGridView.Rows.Clear();

                foreach (var t in value)
                {
                    Add(t);
                }
            }
        }

        private void FrmList_Load(object sender, EventArgs e)
        {
            if (DataGridView != null)
            {
                DataGridView.SelectionChanged += new EventHandler(DataGridView_SelectionChanged);
                DataGridView.MouseDoubleClick += new MouseEventHandler(DataGridView_MouseDoubleClick);
            }
        }

        private void FrmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private long GetSelectedId()
        {
            if (DataGridView.SelectedCells.Count == 0) return -1;

            var cellValue = DataGridView[ColumnFind.Index, DataGridView.SelectedCells[0].RowIndex].Value;

            if (cellValue == null) return -1;

            return (long)cellValue;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var id = GetSelectedId();

            if (id == -1) return;

            MainForm.SelectById(this, id);
        }

        private void DataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var id = GetSelectedId();

            if (id == -1) return;

            MainForm.ChangeById(this, id);
        }
    }
}