using P3tr0viCh.Database;
using P3tr0viCh.Database.Extensions;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Presenters;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using static TileExplorer.Enums;

namespace TileExplorer.Presenters
{
    internal abstract class PresenterFrmListBase<T> : PresenterFrmList<T>, IPresenterFrmListBase where T : BaseId, new()
    {
        public IMainForm MainForm => Form.Owner as IMainForm;

        public object Value { get; set; }

        public abstract ChildFormType FormType { get; }

        public abstract FrmListType ListType { get; }

        public PresenterFrmListBase(IFrmListBase frmList) : base(frmList)
        {
            ItemChanged += PresenterFrmListBase_ItemChanged;
            ItemListChanged += PresenterFrmListBase_ItemListChanged;
            ItemListDeleted += PresenterFrmListBase_ItemListDeleted;
        }

        protected override void FormOpened()
        {
            DebugWrite.Line($"ListType = {ListType}");
        }

        protected override void FormClosed()
        {
            DebugWrite.Line("closed");
        }

        private void FrmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClosing();
        }

        private void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            PerformOnPositionChanged();
        }

        protected override void LoadFormState()
        {
            AppSettings.Local.LoadFormState(Form, ListType.ToString(), AppSettings.Local.Default.FormStates);
            AppSettings.Local.LoadDataGridColumns(FrmList.DataGridView, ListType.ToString(), AppSettings.Local.Default.ColumnStates);
        }

        protected override void SaveFormState()
        {
            AppSettings.Local.SaveFormState(Form, ListType.ToString(), AppSettings.Local.Default.FormStates);
            AppSettings.Local.SaveDataGridColumns(FrmList.DataGridView, ListType.ToString(), AppSettings.Local.Default.ColumnStates);
        }

        protected override void UpdateColumns()
        {
            FrmList.DataGridView.Columns[nameof(BaseId.Id)].Visible = false;
            FrmList.DataGridView.Columns[nameof(BaseId.IsNew)].Visible = false;
        }

        public void FormClosing()
        {
            SaveFormState();

            AppSettings.LocalSave();
        }

        public IBaseId Find(IBaseId value)
        {
            return Find(value as T);
        }

        public event PositionChanged OnPositionChanged;

        private void PerformOnPositionChanged()
        {
            OnPositionChanged?.Invoke();
        }

        private async void PresenterFrmListBase_ItemChanged(object sender, ItemChangedEventArgs<T> e)
        {
            await MainForm.UpdateDataAsync(DataLoad.ObjectChange, e.Value);
        }

        private async void PresenterFrmListBase_ItemListChanged(object sender, ItemListChangedEventArgs<T> e)
        {
            await MainForm.UpdateDataAsync(DataLoad.ObjectChange, e.Values);
        }

        private async void PresenterFrmListBase_ItemListDeleted(object sender, ItemListDeletedEventArgs<T> e)
        {
            await MainForm.UpdateDataAsync(DataLoad.ObjectDelete, e.Values);
        }

        private void PerformListLoadException(Exception e)
        {
            DebugWrite.Line(e.GetQuery());

            DebugWrite.Error(e);

            Msg.Error(e.Message);
        }

        protected override void ListLoadException(Exception e)
        {
            PerformListLoadException(e);
        }

        protected override void ListItemChangeException(Exception e)
        {
            PerformListLoadException(e);
        }

        protected override void ListItemDeleteException(Exception e)
        {
            PerformListLoadException(e);
        }

        public abstract void ListItemChange(IBaseId value);

        public abstract void ListItemDelete(IBaseId value);

        public abstract Task UpdateDataAsync();
    }
}