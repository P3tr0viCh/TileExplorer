using Newtonsoft.Json.Linq;
using P3tr0viCh.Database;
using P3tr0viCh.Database.Extensions;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using static TileExplorer.Enums;

namespace TileExplorer.Presenters
{
    internal abstract partial class PresenterFrmListBase<T> :
        IPresenterFrmList,
        IPresenterDataGridViewCompare<T> where T : BaseId, new()
    {
        public Form Form => FrmList as Form;

        public IMainForm MainForm => Form.Owner as IMainForm;

        public IFrmList FrmList { get; private set; }

        public object Value { get; set; }

        public abstract ChildFormType FormType { get; }

        public abstract FrmListType ListType { get; }

        public bool Changed { get; set; } = false;

        public DataGridView DataGridView => FrmList.DataGridView;
        public StatusStrip StatusStrip => FrmList.StatusStrip;


        internal readonly BindingSource bindingSource;

        internal readonly PresenterDataGridViewFrmList<T> presenterDataGridView;

        public PresenterFrmListBase(IFrmList frmList)
        {
            FrmList = frmList;

            Form.Load += new EventHandler(Form_Load);
            Form.FormClosing += new FormClosingEventHandler(FrmList_FormClosing);

            bindingSource = new BindingSource();

            presenterDataGridView = new PresenterDataGridViewFrmList<T>(this);

            bindingSource.PositionChanged += new EventHandler(BindingSource_PositionChanged);

            DataGridView.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView_CellDoubleClick);

            DataGridView.CellMouseDown += new DataGridViewCellMouseEventHandler(DataGridView_CellMouseDown);
        }

        private async void Form_Load(object sender, System.EventArgs e)
        {
            await FormLoadAsync();
        }

        private void FrmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClosing();
        }

        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            PerformOnPositionChanged();
        }

        private FrmListGrant grants = FrmListGrant.All;
        protected FrmListGrant Grants
        {
            get => grants;
            set
            {
                grants = value;

                presenterDataGridView.CanSort = CanSort;

                foreach (ToolStripItem item in FrmList.ToolStrip.Items)
                {
                    if (item.Name == "tsbtnClose")
                    {
                        continue;
                    }

                    if (item.Name == "tsbtnAdd")
                    {
                        item.Visible = CanAdd;
                        continue;
                    }

                    if (item.Name == "tsbtnChange")
                    {
                        item.Visible = CanChange;
                        continue;
                    }

                    if (item.Name == "tsbtnDelete")
                    {
                        item.Visible = CanDelete;
                        continue;
                    }

                    if (item.Name == "toolStripSeparator1")
                    {
                        item.Visible = CanAdd || CanChange || CanDelete;
                        continue;
                    }
                }
            }
        }

        private bool CanAdd => grants.HasFlag(FrmListGrant.Add);
        private bool CanChange => grants.HasFlag(FrmListGrant.Change);
        private bool CanDelete => grants.HasFlag(FrmListGrant.Delete);
        private bool CanSort => grants.HasFlag(FrmListGrant.Sort);

        private void SetDataSource()
        {
            bindingSource.DataSource = Enumerable.Empty<T>();

            DataGridView.DataSource = bindingSource;
        }

        protected abstract string FormTitle { get; }

        protected virtual void LoadFormState()
        {
            AppSettings.Local.LoadFormState(Form, ListType.ToString(), AppSettings.Local.Default.FormStates);
            AppSettings.Local.LoadDataGridColumns(FrmList.DataGridView, ListType.ToString(), AppSettings.Local.Default.ColumnStates);
        }

        protected virtual void SaveFormState()
        {
            AppSettings.Local.SaveFormState(Form, ListType.ToString(), AppSettings.Local.Default.FormStates);
            AppSettings.Local.SaveDataGridColumns(FrmList.DataGridView, ListType.ToString(), AppSettings.Local.Default.ColumnStates);
        }

        protected abstract void UpdateColumns();

        public virtual void UpdateSettings()
        {
        }

        private async Task FormLoadAsync()
        {
            Form.Text = FormTitle;

            DataGridView.MultiSelect = true;

            SetDataSource();

            LoadFormState();

            UpdateCommonColumns();

            UpdateColumns();

            UpdateSettings();

            await UpdateDataAsync();
        }

        private void UpdateCommonColumns()
        {
            DataGridView.Columns[nameof(BaseId.Id)].Visible = false;
            DataGridView.Columns[nameof(BaseId.IsNew)].Visible = false;
        }

        public void FormClosing()
        {
            SaveFormState();

            AppSettings.LocalSave();
        }

        public T Find(T value)
        {
            return bindingSource.Cast<T>().Where(item => item.Id == value.Id).FirstOrDefault();
        }

        public IBaseId Find(IBaseId value)
        {
            return Find(value as T);
        }

        public T Selected
        {
            get => presenterDataGridView.Selected;
            set => presenterDataGridView.Selected = Find(value);
        }

        public IEnumerable<T> SelectedList
        {
            get => DataGridView.GetSelectedList<T>();
        }

        public event ListChanged OnListChanged;

        public event PositionChanged OnPositionChanged;

        private void PerformOnListChanged()
        {
            Changed = true;

            OnListChanged?.Invoke();
        }

        private void PerformOnPositionChanged()
        {
            Changed = true;

            OnPositionChanged?.Invoke();
        }

        private void InternalListItemChange(IBaseId value)
        {
            var item = Find(value);

            if (item == default)
            {
                bindingSource.Add(value);
            }
            else
            {
                var index = bindingSource.IndexOf(item);

                bindingSource.List[index] = value;

                bindingSource.ResetItem(index);
            }
        }

        public void ListItemChange(IBaseId value)
        {
            InternalListItemChange(value);

            DataGridView.SetSelectedRows(value);

            presenterDataGridView.Sort();

            PerformOnListChanged();
        }

        public void ListItemChange(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                InternalListItemChange(item);
            }

            DataGridView.SetSelectedRows(list);

            presenterDataGridView.Sort();

            PerformOnListChanged();
        }

        private void InternalListItemDelete(IBaseId value)
        {
            bindingSource.Remove(value);
        }

        public void ListItemDelete(IBaseId value)
        {
            InternalListItemDelete(value);

            PerformOnListChanged();
        }

        private void ListItemDelete(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                InternalListItemDelete(item);
            }

            PerformOnListChanged();
        }

        protected virtual T GetNewItem() => new T();

        public class OkEventArgs : EventArgs
        {
            public bool Ok { get; set; } = false;

            public OkEventArgs()
            {
            }
        }

        public class ItemDialogEventArgs : OkEventArgs
        {
            public T Value { get; set; } = null;

            public ItemDialogEventArgs()
            {
            }
        }

        public class ItemListDialogEventArgs : OkEventArgs
        {
            public IEnumerable<T> Values { get; set; } = null;

            public ItemListDialogEventArgs()
            {
            }
        }

        public delegate void ItemChangeDialogEventHandler(object sender, ItemDialogEventArgs e);

        public delegate void ItemListChangeDialogEventHandler(object sender, ItemListDialogEventArgs e);

        public delegate void ItemListDeleteDialogEventHandler(object sender, ItemListDialogEventArgs e);

        public event ItemChangeDialogEventHandler ItemChangeDialog;

        public event ItemListChangeDialogEventHandler ItemListChangeDialog;

        public event ItemListDeleteDialogEventHandler ItemListDeleteDialog;

        internal void OnItemChangeDialogEvent(ItemDialogEventArgs e)
        {
            ItemChangeDialog?.Invoke(this, e);
        }

        internal void OnItemListChangeDialogEvent(ItemListDialogEventArgs e)
        {
            ItemListChangeDialog?.Invoke(this, e);
        }

        internal void OnItemListDeleteDialogEvent(ItemListDialogEventArgs e)
        {
            ItemListDeleteDialog?.Invoke(this, e);
        }

        private bool ShowItemChangeDialog(T value)
        {
            var itemDialogEventArgs = new ItemDialogEventArgs()
            {
                Value = value
            };

            OnItemChangeDialogEvent(itemDialogEventArgs);

            return itemDialogEventArgs.Ok;
        }

        private bool ShowItemListChangeDialog(IEnumerable<T> values)
        {
            var itemListDialogEventArgs = new ItemListDialogEventArgs()
            {
                Values = values
            };

            OnItemListChangeDialogEvent(itemListDialogEventArgs);

            return itemListDialogEventArgs.Ok;
        }

        private bool ShowItemDeleteDialog(IEnumerable<T> values)
        {
            var itemListDialogEventArgs = new ItemListDialogEventArgs()
            {
                Values = values
            };

            OnItemListDeleteDialogEvent(itemListDialogEventArgs);

            return itemListDialogEventArgs.Ok;
        }

        public async Task UpdateDataAsync()
        {
            await PerformListLoadAsync();
        }

        private async Task ListItemChangeAsync(T value)
        {
            try
            {
                if (!ShowItemChangeDialog(value)) return;

                await PerformListItemSaveAsync(value);

                ListItemChange(value);

                await MainForm.UpdateDataAsync(DataLoad.ObjectChange, value);
            }
            catch (Exception e)
            {
                DebugWrite.Line(e.GetQuery());

                DebugWrite.Error(e);

                Msg.Error(e.Message);
            }
        }

        private async Task ListItemChangeAsync(IEnumerable<T> list)
        {
            try
            {
                if (!ShowItemListChangeDialog(list)) return;

                await PerformListItemSaveAsync(list);

                ListItemChange(list);

                await MainForm.UpdateDataAsync(DataLoad.ObjectChange, list);
            }
            catch (Exception e)
            {
                DebugWrite.Line(e.GetQuery());

                DebugWrite.Error(e);

                Msg.Error(e.Message);
            }
        }

        public async Task ListItemAddNewAsync()
        {
            if (!CanAdd) return;

            var item = GetNewItem();

            await ListItemChangeAsync(item);
        }

        public async Task ListItemChangeSelectedAsync()
        {
            if (!CanChange) return;

            if (Grants.HasFlag(FrmListGrant.MultiChange))
            {
                var list = SelectedList;

                DataGridView.SetSelectedRows(list);

                await ListItemChangeAsync(list);
            }
            else
            {
                var item = Selected;

                DataGridView.SetSelectedRows(item);

                await ListItemChangeAsync(item);
            }
        }

        public async Task ListItemDeleteSelectedAsync()
        {
            if (!CanDelete) return;

            try
            {
                var list = SelectedList;

                DataGridView.SetSelectedRows(list);

                if (!ShowItemDeleteDialog(list)) return;

                await PerformListItemDeleteAsync(list);

                ListItemDelete(list);

                await MainForm.UpdateDataAsync(DataLoad.ObjectDelete, list);
            }
            catch (Exception e)
            {
                DebugWrite.Line(e.GetQuery());

                DebugWrite.Error(e);

                Msg.Error(e.Message);
            }
        }

        private async void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            await ListItemChangeSelectedAsync();
        }

        private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            Utils.Forms.SelectCellOnCellMouseDown(DataGridView, e);
        }

        public abstract int Compare(T x, T y, string dataPropertyName, ComparerSortOrder sortOrder);
    }
}