using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Interfaces;
using System.Linq;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListTags : PresenterFrmList<TagModel>
    {
        public override ChildFormType FormType => ChildFormType.TagList;

        public PresenterFrmListTags(IFrmList frmList) : base(frmList)
        {
            ItemsChangeDialog += PresenterFrmListTags_ItemsChangeDialog;
            ItemsDeleteDialog += PresenterFrmListTags_ItemsDeleteDialog;

            ItemsChanged += PresenterFrmListTags_ItemsChanged;
            ItemsDeleted += PresenterFrmListTags_ItemsDeleted;
        }

        protected override string FormTitle => Resources.TitleListTags;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(TagModel.Text);
        }

        private void PresenterFrmListTags_ItemsChangeDialog(object sender, ItemsDialogEventArgs<TagModel> e)
        {
            e.Ok = FrmTag.ShowDlg(Form, e.Values.First());
        }

        private void PresenterFrmListTags_ItemsDeleteDialog(object sender, ItemsDialogEventArgs<TagModel> e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values, Resources.QuestionTagDelete, Resources.QuestionTagListDelete);
        }

        private async void PresenterFrmListTags_ItemsChanged(object sender, ItemsEventArgs<TagModel> e)
        {
            Utils.Forms.ChildFormsListItemsChange(ChildFormType.TrackList, e.Values);

            await Utils.Forms.ChildFormsUpdateDataAsync(ChildFormType.Filter);
        }

        private async void PresenterFrmListTags_ItemsDeleted(object sender, ItemsEventArgs<TagModel> e)
        {
            Utils.Forms.ChildFormsListItemsDelete(ChildFormType.TrackList, e.Values);

            await Utils.Forms.ChildFormsUpdateDataAsync(ChildFormType.Filter);
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

            FrmList.DataGridView.Columns[nameof(TagModel.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        public override int Compare(TagModel x, TagModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            return EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
        }
    }
}