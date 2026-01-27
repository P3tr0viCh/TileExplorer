using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListMarkers : PresenterFrmListBase<Marker>
    {
        public override FrmListType ListType => FrmListType.MarkerList;

        public override ChildFormType FormType => ChildFormType.MarkerList;

        public PresenterFrmListMarkers(IFrmList frmList) : base(frmList)
        {
        }

        protected override string FormTitle => Resources.TitleListMarkers;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(Marker.Text);
        }

        protected override bool ShowItemChangeDialog(Marker value)
        {
            if (value.IsNew)
            {
                MainForm.ListItemAdd(value);
            }
            else
            {
                MainForm.ListItemChangeAsync(new List<Marker>() { value });
            }

            return true;
        }

        protected override bool ShowItemDeleteDialog(IEnumerable<Marker> list)
        {
            var count = list?.Count();

            if (count == 0) return false;

            var first = list.FirstOrDefault();

            var text = first.Text;

            if (text.IsEmpty())
            {
                text = $"{first.Lat}:{first.Lng}";
            }

            var question = count == 1 ? Resources.QuestionMarkerDelete : Resources.QuestionMarkerListDelete;

            return Msg.Question(question, text, count - 1);
        }

        protected override async Task ListItemSaveAsync(Marker value)
        {
            await Task.CompletedTask;
        }

        protected override async Task ListItemDeleteAsync(IEnumerable<Marker> list)
        {
            await Database.Actions.MarkerDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            DataGridView.Columns[nameof(Marker.Text)].DisplayIndex = 0;

            DataGridView.Columns[nameof(Marker.Text)].Visible = true;

            DataGridView.Columns[nameof(Marker.IsTextVisible)].Visible = false;
            DataGridView.Columns[nameof(Marker.OffsetX)].Visible = false;
            DataGridView.Columns[nameof(Marker.OffsetY)].Visible = false;

            DataGridView.Columns[nameof(Marker.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        public override void UpdateSettings()
        {
            DataGridView.Columns[nameof(Marker.Lat)].DefaultCellStyle = DataGridViewCellStyles.LatLng;
            DataGridView.Columns[nameof(Marker.Lng)].DefaultCellStyle = DataGridViewCellStyles.LatLng;
        }

        public override int Compare(Marker x, Marker y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(Marker.Text):
                    result = EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
                    break;
                case nameof(Marker.Lat):
                    result = SortOrderComparer.Default.Compare(x.Lat, y.Lat, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.Lng, y.Lng, ComparerSortOrder.Ascending);
                    break;
                case nameof(Marker.Lng):
                    result = SortOrderComparer.Default.Compare(x.Lng, y.Lng, sortOrder);
                    if (result == 0)
                        result = SortOrderComparer.Default.Compare(x.Lat, y.Lat, ComparerSortOrder.Ascending);
                    break;
            }

            return result;
        }
    }
}