using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database;
using static TileExplorer.Database.Filter;
using static TileExplorer.Main;

namespace TileExplorer
{
    public partial class FrmFilter : Form, IFrmChild
    {
        private Filter Filter => (Owner as IMainForm).Filter;

        public FrmFilter(Form owner)
        {
            Owner = owner;

            InitializeComponent();
        }

        private void FrmFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public bool Updating { set { UseWaitCursor = value; } }

        private bool selfChange = false;

        private void FrmFilter_Load(object sender, EventArgs e)
        {
            dtpDay.CustomFormat = Settings.Default.FormatDate;

            dtpDateFrom.CustomFormat = Settings.Default.FormatDate;
            dtpDateTo.CustomFormat = Settings.Default.FormatDate;

            selfChange = true;

            rbtnFilterNone.Checked = true;

            dtpDay.Value = Filter.Day != default ? Filter.Day : DateTime.Now;
            
            dtpDateFrom.Value = Filter.DateFrom != default ? Filter.DateFrom : DateTime.Now;
            dtpDateTo.Value = Filter.DateTo != default ? Filter.DateTo : DateTime.Now;
            
            tbYears.Text = Filter.Years != default ? string.Join(", ", Filter.Years) : string.Empty;

            selfChange = false;
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

            Debug.WriteLine("FilterChanged");

            Filter.Day = dtpDay.Value;

            Filter.DateFrom = dtpDateFrom.Value;
            Filter.DateTo = dtpDateTo.Value;

            Filter.Years = SplitInts(tbYears.Text);

            if (rbtnFilterNone.Checked)
                Filter.Type = FilterType.None;
            else if (rbtnFilterDay.Checked)
                Filter.Type = FilterType.Day;
            else if (rbtnFilterPeriod.Checked)
                Filter.Type = FilterType.Period;
            else if (rbtnFilterYears.Checked)
                Filter.Type = FilterType.Years;
        }

        private void FilterTypeChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            FilterChanged();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("Timer_Tick");
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
    }
}