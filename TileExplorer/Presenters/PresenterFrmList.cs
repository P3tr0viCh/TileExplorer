using P3tr0viCh.Database;
using P3tr0viCh.Database.Extensions;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Interfaces;
using P3tr0viCh.Utils.Presenters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;

namespace TileExplorer.Presenters
{
    internal abstract class PresenterFrmList<T> : PresenterFrmListBase<T>, IPresenterFrmList where T : BaseId, new()
    {
        public IMainForm MainForm => Form.Owner as IMainForm;

        public object Value { get; set; }

        public abstract ChildFormType FormType { get; }

        public PresenterFrmList(IFrmList frmList) : base(frmList)
        {
            Grants = Grants.AddFlag(FrmListGrant.MultiDelete);

            ItemsExceptionLoad += PresenterFrmList_Exception;
            ItemsExceptionChange += PresenterFrmList_Exception;
            ItemsExceptionDelete += PresenterFrmList_Exception;
        }

        protected override void FormOpened()
        {
            DebugWrite.Line($"{FormType}");
        }

        protected override void FormClosed()
        {
            DebugWrite.Line($"{FormType}");
        }

        private void FrmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClosing();
        }

        protected override void LoadFormState()
        {
            AppSettings.Local.LoadFormState(Form, FormType.ToString(), AppSettings.Local.Default.FormStates);
            AppSettings.Local.LoadDataGridColumns(FrmList.DataGridView, FormType.ToString(), AppSettings.Local.Default.ColumnStates);
        }

        protected override void SaveFormState()
        {
            AppSettings.Local.SaveFormState(Form, FormType.ToString(), AppSettings.Local.Default.FormStates);
            AppSettings.Local.SaveDataGridColumns(FrmList.DataGridView, FormType.ToString(), AppSettings.Local.Default.ColumnStates);

            AppSettings.LocalSave();
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

        private void PresenterFrmList_Exception(object sender, ExceptionEventArgs e)
        {
            DebugWrite.Line(e.Exception.GetQuery());

            DebugWrite.Error(e.Exception);

            Msg.Error(e.Exception.Message);
        }

        protected override Task<IEnumerable<T>> DatabaseListLoadAsync(CancellationToken cancellationToken)
        {
            return Database.Default.ListLoadAsync<T>();
        }

        protected override async Task DatabaseListItemsSaveAsync(IEnumerable<T> list)
        {
            await Database.Default.ListItemSaveAsync(list);
        }

        protected override async Task DatabaseListItemsDeleteAsync(IEnumerable<T> list)
        {
            await Database.Default.ListItemDeleteAsync(list);
        }
    }
}