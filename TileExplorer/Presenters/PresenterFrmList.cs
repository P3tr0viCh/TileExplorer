using P3tr0viCh.Database;
using P3tr0viCh.Database.Extensions;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Presenters;
using System;
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

        public new IChildFormList FrmList => base.FrmList as IChildFormList;

        public object Value { get; set; }

        public abstract ChildFormType FormType { get; }

        public PresenterFrmList(IChildFormList frmList) : base(frmList)
        {
            Grants = Grants.AddFlag(FrmListGrant.MultiDelete);

            FormOpened += PresenterFrmList_FormOpened;
            FormClosed += PresenterFrmList_FormClosed;

            StatusStart += PresenterFrmList_StatusStart;
            StatusStop += PresenterFrmList_StatusStop;

            ItemsExceptionLoad += PresenterFrmList_Exception;
            ItemsExceptionChange += PresenterFrmList_Exception;
            ItemsExceptionDelete += PresenterFrmList_Exception;
        }

        private void FormOpen()
        {
            DebugWrite.Line($"{FormType}");
        }

        private void FormClose()
        {
            DebugWrite.Line($"{FormType}");
        }

        private void PresenterFrmList_FormOpened(object sender, EventArgs e)
        {
            FormOpen();
        }

        private void PresenterFrmList_FormClosed(object sender, EventArgs e)
        {
            FormClose();
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

        private void PresenterFrmList_StatusStart(object sender, StatusEventArgs e)
        {
            var status = ProgramStatus.Status.Idle;

            switch (e.Status)
            {
                case FrmListDatabaseActionStatus.Load:
                    status = ProgramStatus.Status.LoadData;

                    FrmList.Loading = true;

                    break;
                case FrmListDatabaseActionStatus.Save:
                case FrmListDatabaseActionStatus.Delete:
                    status = ProgramStatus.Status.SaveData;
                    break;
            }

            e.Object = ProgramStatus.Default.Start(status);
        }

        private void PresenterFrmList_StatusStop(object sender, StatusEventArgs e)
        {
            if (e.Status == FrmListDatabaseActionStatus.Load)
            {
                FrmList.Loading = false;
            }

            ProgramStatus.Default.Stop((ProgramStatus<ProgramStatus.Status>.Status)e.Object);
        }

        private void PresenterFrmList_Exception(object sender, ExceptionEventArgs e)
        {
            DebugWrite.Line(e.Exception.GetQuery());

            DebugWrite.Error(e.Exception);

            Msg.Error(e.Exception.Message);
        }

        protected override async Task<IEnumerable<T>> DatabaseListLoadAsync(CancellationToken cancellationToken)
        {
            return await Database.Default.ListLoadAsync<T>();
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