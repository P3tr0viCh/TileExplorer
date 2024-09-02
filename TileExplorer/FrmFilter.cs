using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TileExplorer.Database;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class FrmFilter : Form, IChildForm
    {
        public IMainForm MainForm => Owner as IMainForm;

        public ChildFormType ChildFormType => ChildFormType.Filter;

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

        private void FrmFilter_Load(object sender, EventArgs e)
        {
            AppSettings.Local.LoadFormState(this, AppSettings.Local.Default.FormStateFilter);
 
            UpdateSettings();

            selfChange = true;

            rbtnFilterNone.Checked = true;

            dtpDay.Value = Filter.Default.Day != default ? Filter.Default.Day : DateTime.Now.Date;

            dtpDateFrom.Value = Filter.Default.DateFrom != default ? Filter.Default.DateFrom : DateTime.Now.Date;
            dtpDateTo.Value = Filter.Default.DateTo != default ? Filter.Default.DateTo : DateTime.Now.Date;

            tbYears.Text = Filter.Default.Years != default ? string.Join(", ", Filter.Default.Years) : string.Empty;

            selfChange = false;

            MainForm.ChildFormOpened(this);
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

        private int[] SplitInts(string text)
        {
            var s = "";

            var list = new List<int>();

            foreach (var c in text)
            {
                if (c >= '0' && c <= '9')
                {
                    s += c;
                }
                else
                {
                    if (s != "") list.Add(int.Parse(s));

                    s = "";
                }
                s.Split(' ');
            }

            if (s != "") list.Add(int.Parse(s));

            return list.ToArray();
        }

        private void FilterChanged()
        {
            timer.Stop();

            Filter.Default.Day = dtpDay.Value.Date;

            Filter.Default.DateFrom = dtpDateFrom.Value.Date;
            Filter.Default.DateTo = dtpDateTo.Value.Date;

            Filter.Default.Years = SplitInts(tbYears.Text);

            if (rbtnFilterNone.Checked)
                Filter.Default.Type = Filter.FilterType.None;
            else if (rbtnFilterDay.Checked)
                Filter.Default.Type = Filter.FilterType.Day;
            else if (rbtnFilterPeriod.Checked)
                Filter.Default.Type = Filter.FilterType.Period;
            else if (rbtnFilterYears.Checked)
                Filter.Default.Type = Filter.FilterType.Years;
        }

        private void FilterTypeChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            FilterChanged();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            FilterChanged();
        }

        private void DateTimeChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            timer.Stop();
            timer.Start();
        }

        private void YearsChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            timer.Stop();
            timer.Start();
        }

        public void UpdateSettings()
        {
            dtpDay.CustomFormat = AppSettings.Roaming.Default.FormatDate;

            dtpDateFrom.CustomFormat = AppSettings.Roaming.Default.FormatDate;
            dtpDateTo.CustomFormat = AppSettings.Roaming.Default.FormatDate;
        }

        public Task UpdateDataAsync()
        {
            return Task.CompletedTask;
        }
    }
}