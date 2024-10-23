﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

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
                };

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
                };

                FrmChartTracksByMonth.ShowFrm(owner, year, month);
            }
        }
    }
}