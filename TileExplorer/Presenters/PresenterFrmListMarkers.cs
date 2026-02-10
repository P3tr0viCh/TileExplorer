using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer.Presenters
{
    internal class PresenterFrmListMarkers : PresenterFrmList<Marker>
    {
        public override ChildFormType FormType => ChildFormType.MarkerList;

        public PresenterFrmListMarkers(IFrmList frmList) : base(frmList)
        {
            BindingSource.PositionChanged += BindingSource_PositionChanged;

            ItemsChangeDialog += PresenterFrmListMarkers_ItemsChangeDialog;
            ItemsDeleteDialog += PresenterFrmListMarkers_ItemsDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListMarkers;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(Marker.Text);
        }

        private async void PresenterFrmListMarkers_ItemsChangeDialog(object sender, ItemsDialogEventArgs<Marker> e)
        {
            if (e.Values.First().IsNew)
            {
                e.Ok = await MainForm.ListItemAddAsync(e.Values.First());
            }
            else
            {
                e.Ok = await MainForm.ListItemChangeAsync(e.Values);
            }
        }

        private void PresenterFrmListMarkers_ItemsDeleteDialog(object sender, ItemsDialogEventArgs<Marker> e)
        {
            e.Ok = Utils.ShowItemDeleteDialog(e.Values,
                Resources.QuestionMarkerDelete, Resources.QuestionMarkerListDelete);
        }

        protected override async Task DatabaseListItemsSaveAsync(IEnumerable<Marker> list)
        {
            await Task.CompletedTask;
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

            FrmList.DataGridView.Columns[nameof(Marker.Text)].DisplayIndex = 0;

            FrmList.DataGridView.Columns[nameof(Marker.Text)].Visible = true;

            FrmList.DataGridView.Columns[nameof(Marker.IsTextVisible)].Visible = false;
            FrmList.DataGridView.Columns[nameof(Marker.OffsetX)].Visible = false;
            FrmList.DataGridView.Columns[nameof(Marker.OffsetY)].Visible = false;

            FrmList.DataGridView.Columns[nameof(Marker.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        public override void UpdateSettings()
        {
            FrmList.DataGridView.Columns[nameof(Marker.Lat)].DefaultCellStyle = DataGridViewCellStyles.LatLng;
            FrmList.DataGridView.Columns[nameof(Marker.Lng)].DefaultCellStyle = DataGridViewCellStyles.LatLng;
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

        private async void BindingSource_PositionChanged(object sender, EventArgs e)
        {
            await MainForm.SelectMapItemAsync(this, Selected);
        }
    }
}