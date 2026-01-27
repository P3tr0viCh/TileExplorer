using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal static partial class Utils
    {
        public static class Forms
        {
            public static List<T> GetChildForms<T>(ChildFormType type = default)
            {
                var forms = new List<T>();

                foreach (var frm in Application.OpenForms)
                {
                    if (frm is IChildForm childFrm && frm is T childFrmT &&
                        (type == default || type.HasFlag(childFrm.FormType)))
                    {
                        forms.Add(childFrmT);
                    }
                }

                return forms;
            }

            public static FrmList GetFrmList(ChildFormType type)
            {
                return GetChildForms<FrmList>(type).FirstOrDefault();
            }

            public static void OpenChartTracksByYear(Form owner, int year)
            {
                foreach (var frm in GetChildForms<FrmChartTracksByYear>(ChildFormType.ChartTracksByYear))
                {
                    if (frm.Year == year)
                    {
                        frm.BringToFront();
                        return;
                    }
                }
                ;

                FrmChartTracksByYear.ShowFrm(owner, year);
            }

            public static void OpenChartTracksByMonth(Form owner, int year, int month)
            {
                foreach (var frm in GetChildForms<FrmChartTracksByMonth>(ChildFormType.ChartTracksByMonth))
                {
                    if (frm.Year == year && frm.Month == month)
                    {
                        frm.BringToFront();
                        return;
                    }
                }
                ;

                FrmChartTracksByMonth.ShowFrm(owner, year, month);
            }

            public static void TextBoxWrongValue(TextBox textBox, string error)
            {
                textBox.Focus();
                textBox.SelectAll();

                Msg.Error(error);
            }

            public static void TextBoxWrongValue(TextBox textBox, string error, object arg0)
            {
                TextBoxWrongValue(textBox, string.Format(error, arg0));
            }

            public static bool TextBoxIsWrongValue(Func<bool> check, TextBox textBox, string error)
            {
                if (check())
                {
                    TextBoxWrongValue(textBox, error);

                    return true;
                }

                return false;
            }

            public static bool TextBoxIsWrongValue(Func<bool> check, TextBox textBox, string error, object arg0)
            {
                 return TextBoxIsWrongValue(check, textBox, string.Format(error, arg0));
            }

            public static bool TextBoxIsEmpty(TextBox textBox, string error)
            {
                return TextBoxIsWrongValue(() => textBox.IsEmpty(), textBox, error);
            }

            public static bool TextBoxIsWrongFloat(TextBox textBox)
            {
                if (textBox.IsEmpty())
                {
                    return true;
                }

                if (Misc.FloatCheck(textBox.Text))
                {
                    return true;
                }

                TextBoxWrongValue(textBox, Resources.ErrorNeedDigit);

                return false;
            }

            public static void SelectCellOnCellMouseDown(DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                    {
                        dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];
                    }
                }
            }
        }
    }
}