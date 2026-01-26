using P3tr0viCh.Database;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TileExplorer.Database;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class FrmFilter : Form, IChildForm, IUpdateDataForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType FormType => ChildFormType.Filter;

        public FrmFilter()
        {
            InitializeComponent();
        }

        public static FrmFilter ShowFrm(Form owner)
        {
            var frm = new FrmFilter()
            {
                Owner = owner
            };

            frm.Show(owner);

            return frm;
        }

        private bool selfChange = false;

        private async void FrmFilter_Load(object sender, EventArgs e)
        {
            AppSettings.Local.LoadFormState(this, AppSettings.Local.Default.FormStates);

            UpdateSettings();

            selfChange = true;

            rbtnFilterAllDate.Checked = true;

            dtpDay.Value = Filter.Default.Day != default ? Filter.Default.Day : DateTime.Now.Date;

            dtpDateFrom.Value = Filter.Default.DateFrom != default ? Filter.Default.DateFrom : DateTime.Now.Date;
            dtpDateTo.Value = Filter.Default.DateTo != default ? Filter.Default.DateTo : DateTime.Now.Date;

            clbYears.ColumnWidth = clbYears.Width / 2 - 16;

            cboxUseEquipments.Checked = Filter.Default.UseEquipments;

            cboxUseTags.Checked = Filter.Default.UseTags;

            selfChange = false;

            MainForm.ChildFormOpened(this);

            await UpdateDataAsync();
        }

        private void FrmFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppSettings.Local.SaveFormState(this, AppSettings.Local.Default.FormStates);

            AppSettings.LocalSave();
        }

        private void FrmFilter_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.ChildFormClosed(this);
        }

        private void FrmFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void FilterChanged()
        {
            timer.Stop();

            selfChange = true;

            Filter.Default.Day = dtpDay.Value.Date;

            Filter.Default.DateFrom = dtpDateFrom.Value.Date;
            Filter.Default.DateTo = dtpDateTo.Value.Date;

            Filter.Default.Years = clbYears.CheckedItems.Cast<int>().ToArray();

            if (rbtnFilterAllDate.Checked)
                Filter.Default.DateType = Filter.FilterDateType.AllDate;
            else if (rbtnFilterDay.Checked)
                Filter.Default.DateType = Filter.FilterDateType.Day;
            else if (rbtnFilterPeriod.Checked)
                Filter.Default.DateType = Filter.FilterDateType.Period;
            else if (rbtnFilterYears.Checked)
                Filter.Default.DateType = Filter.FilterDateType.Years;

            Filter.Default.UseEquipments = cboxUseEquipments.Checked;

            Filter.Default.Equipments = GetCheckedEquipments();

            Filter.Default.UseTags = cboxUseTags.Checked;

            Filter.Default.Tags = GetCheckedTags();

            selfChange = false;
        }

        private void FilterTypeChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            FilterChanged();
        }

        private void DateTimeChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            timer.Restart();
        }

        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (selfChange) return;

            timer.Restart();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            timer.Restart();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            FilterChanged();
        }

        public void UpdateSettings()
        {
            dtpDay.CustomFormat = AppSettings.Roaming.Default.FormatDate;

            dtpDateFrom.CustomFormat = AppSettings.Roaming.Default.FormatDate;
            dtpDateTo.CustomFormat = AppSettings.Roaming.Default.FormatDate;
        }

        private void UpdateDataYears()
        {
            clbYears.DataSource = MainForm.Years;

            if (Filter.Default.Years is null) return;

            for (var i = 0; i < clbYears.Items.Count; i++)
            {
                clbYears.SetItemChecked(i, Filter.Default.Years.Contains(
                    int.Parse(clbYears.Items[i].ToString())));
            }
        }

        private class CheckedListBoxItem : BaseText
        {
            public CheckedListBoxItem(BaseText value)
            {
                Assign(value);
            }

            public override string ToString() => Text;
        }

        private async Task UpdateDataEquipments()
        {
            clbEquipments.Items.Clear();

            var equipments = await Database.Default.ListLoadAsync<Equipment>();

            clbEquipments.Items.Add(new CheckedListBoxItem(
                new Equipment()
                {
                    Text = "(не указано)"
                }));

            foreach (var equipment in equipments)
            {
                clbEquipments.Items.Add(new CheckedListBoxItem(equipment));
            }

            if (Filter.Default.Equipments is null) return;

            for (var i = 0; i < clbEquipments.Items.Count; i++)
            {
                clbEquipments.SetItemChecked(i, Filter.Default.Equipments.Contains(
                    clbEquipments.Items.Cast<CheckedListBoxItem>().Select(e => e.Id).ElementAt(i)));
            }
        }

        private long[] GetCheckedEquipments()
        {
            if (clbEquipments.CheckedItems.Count == 0) return default;

            if (clbEquipments.CheckedItems.Count == clbEquipments.Items.Count) return default;

            return clbEquipments.CheckedItems.Cast<CheckedListBoxItem>().Select(e => e.Id).ToArray();
        }

        private async Task UpdateDataTags()
        {
            clbTags.Items.Clear();

            var tags = await Database.Default.ListLoadAsync<TagModel>();

            foreach (var tag in tags)
            {
                clbTags.Items.Add(new CheckedListBoxItem(tag));
            }

            if (Filter.Default.Tags is null) return;

            for (var i = 0; i < clbTags.Items.Count; i++)
            {
                clbTags.SetItemChecked(i, Filter.Default.Tags.Contains(
                    clbTags.Items.Cast<CheckedListBoxItem>().Select(t => t.Id).ElementAt(i)));
            }
        }

        private long[] GetCheckedTags()
        {
            if (clbTags.CheckedItems.Count == 0) return default;

            if (clbTags.CheckedItems.Count == clbTags.Items.Count) return default;

            return clbTags.CheckedItems.Cast<CheckedListBoxItem>().Select(e => e.Id).ToArray();
        }

        public async Task UpdateDataAsync()
        {
            if (selfChange) return;

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                UpdateDataYears();

                await UpdateDataTags();

                await UpdateDataEquipments();
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }
        }
    }
}