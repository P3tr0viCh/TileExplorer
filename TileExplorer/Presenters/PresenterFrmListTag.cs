using P3tr0viCh.Utils.Comparers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListTags : PresenterFrmListBase<TagModel>
    {
        public override FrmListType ListType => FrmListType.TagList;

        public override ChildFormType FormType => ChildFormType.TagList;

        public PresenterFrmListTags(IFrmList frmList) : base(frmList)
        {
            ItemChangeDialog += PresenterFrmListTags_ItemChangeDialog;

            ItemListDeleteDialog += PresenterFrmListTags_ItemListDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListTags;

        protected override void LoadFormState()
        {
            bindingSource.DataSource = Enumerable.Empty<TagModel>();

            DataGridView.DataSource = bindingSource;

            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(TagModel.Text);
        }

        private void PresenterFrmListTags_ItemChangeDialog(object sender, ItemDialogEventArgs e)
        {
            e.Ok = FrmTag.ShowDlg(Form, e.Value);
        }

        private void PresenterFrmListTags_ItemListDeleteDialog(object sender, ItemListDialogEventArgs e)
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
            DataGridView.Columns[nameof(TagModel.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        public override int Compare(TagModel x, TagModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            return EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
        }
    }
}