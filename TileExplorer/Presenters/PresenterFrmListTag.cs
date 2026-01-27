using P3tr0viCh.Utils;
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
        }

        protected override string FormTitle => Resources.TitleListTags;

        protected override void LoadFormState()
        {
            bindingSource.DataSource = Enumerable.Empty<TagModel>();

            DataGridView.DataSource = bindingSource;

            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(TagModel.Text);
        }

        protected override bool ShowItemChangeDialog(TagModel value)
        {
            return FrmTag.ShowDlg(Form, value);
        }

        protected override bool ShowItemDeleteDialog(IEnumerable<TagModel> list)
        {
            var count = list?.Count();

            if (count == 0) return false;

            var first = list.FirstOrDefault();

            var text = first.Text;

            var question = count == 1 ? Resources.QuestionTagDelete : Resources.QuestionTagListDelete;

            return Msg.Question(question, text, count - 1);
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