using Newtonsoft.Json.Linq;
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
            OnPositionChanged += PresenterFrmListMarkers_OnPositionChanged;

            ItemChangeDialog += PresenterFrmListMarkers_ItemChangeDialog;

            ItemListDeleteDialog += PresenterFrmListMarkers_ItemListDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListMarkers;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(Marker.Text);
        }

        private async void PresenterFrmListMarkers_ItemChangeDialog(object sender, ItemDialogEventArgs e)
        {
            if (e.Value.IsNew)
            {
                e.Ok = await MainForm.ListItemAddAsync(e.Value);
            }
            else
            {
                e.Ok = await MainForm.ListItemChangeAsync(new List<Marker>() { e.Value });
            }
        }

        private void PresenterFrmListMarkers_ItemListDeleteDialog(object sender, ItemListDialogEventArgs e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values,
                Resources.QuestionMarkerDelete, Resources.QuestionMarkerListDelete);
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

        private void PresenterFrmListMarkers_OnPositionChanged()
        {
            MainForm.SelectMapItemAsync(this, Selected);
        }
    }
}