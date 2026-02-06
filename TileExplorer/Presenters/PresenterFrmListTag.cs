using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using System.Collections.Generic;
using System.Threading.Tasks;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListTags : PresenterFrmListBase<TagModel>
    {
        public override FrmListType ListType => FrmListType.TagList;

        public override ChildFormType FormType => ChildFormType.TagList;

        public PresenterFrmListTags(IFrmListBase frmList) : base(frmList)
        {
            ItemChangeDialog += PresenterFrmListTags_ItemChangeDialog;

            ItemListDeleteDialog += PresenterFrmListTags_ItemListDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListTags;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(TagModel.Text);
        }

        private void PresenterFrmListTags_ItemChangeDialog(object sender, ItemDialogEventArgs<TagModel> e)
        {
            e.Ok = FrmTag.ShowDlg(Form, e.Value);
        }

        private void PresenterFrmListTags_ItemListDeleteDialog(object sender, ItemListDialogEventArgs<TagModel> e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values,
                Resources.QuestionTagDelete, Resources.QuestionTagListDelete);
        }

        protected override async Task ListItemDeleteAsync(IEnumerable<TagModel> list)
        {
            await Database.Actions.TagDeleteAsync(list);
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