using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TileExplorer.Database;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

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
            AppSettings.Local.LoadFormState(this, AppSettings.Local.Default.FormStateFilter);

            UpdateSettings();

            selfChange = true;

            rbtnFilterNone.Checked = true;

            dtpDay.Value = Filter.Default.Day != default ? Filter.Default.Day : DateTime.Now.Date;

            dtpDateFrom.Value = Filter.Default.DateFrom != default ? Filter.Default.DateFrom : DateTime.Now.Date;
            dtpDateTo.Value = Filter.Default.DateTo != default ? Filter.Default.DateTo : DateTime.Now.Date;

            clbYears.ColumnWidth = clbYears.Width / 2 - 16;

            selfChange = false;

            MainForm.ChildFormOpened(this);

            await UpdateDataAsync();
        }

        private void FrmFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppSettings.Local.Default.FormStateFilter = AppSettings.Local.SaveFormState(this);

            AppSettings.LocalSave();
        }

        private void FrmFilter_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.ChildFormClosed(this);
        }

        private void FilterChanged()
        {
            timer.Stop();

            selfChange = true;

            Filter.Default.Day = dtpDay.Value.Date;

            Filter.Default.DateFrom = dtpDateFrom.Value.Date;
            Filter.Default.DateTo = dtpDateTo.Value.Date;

            Filter.Default.Years = clbYears.CheckedItems.Cast<int>().ToArray();

            if (rbtnFilterNone.Checked)
                Filter.Default.Type = Filter.FilterType.None;
            else if (rbtnFilterDay.Checked)
                Filter.Default.Type = Filter.FilterType.Day;
            else if (rbtnFilterPeriod.Checked)
                Filter.Default.Type = Filter.FilterType.Period;
            else if (rbtnFilterYears.Checked)
                Filter.Default.Type = Filter.FilterType.Years;

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

        private void ClbYears_ItemCheck(object sender, ItemCheckEventArgs e)
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

        public Task UpdateDataAsync()
        {
            if (selfChange) return Task.CompletedTask;

            clbYears.DataSource = MainForm.Years;

            for (var i = 0; i < clbYears.Items.Count; i++)
            {
                clbYears.SetItemChecked(i, Filter.Default.Years.Contains(
                    int.Parse(clbYears.Items[i].ToString())));
            }

            return Task.CompletedTask;
        }
    }
}