﻿using P3tr0viCh.Utils;
using System;
using System.Linq;

namespace TileExplorer
{
    public partial class Database
    {
        public class Filter
        {
            private static readonly Filter defaultInstance = new Filter();

            public static Filter Default
            {
                get
                {
                    return defaultInstance;
                }
            }

            public delegate void Changed();

            public event Changed OnChanged;

            public enum FilterType
            {
                None,
                Day,
                Period,
                Years
            }

            private FilterType type = FilterType.None;
            public FilterType Type
            {
                get
                {
                    return type;
                }
                set
                {
                    if (type == value) return;

                    type = value;

                    PerformOnChanged();
                }
            }

            private DateTime day = default;
            public DateTime Day
            {
                get
                {
                    return day;
                }
                set
                {
                    if (day == value) return;

                    day = value;

                    if (Type == FilterType.Day) PerformOnChanged();
                }
            }

            private DateTime dateFrom = default;
            public DateTime DateFrom
            {
                get
                {
                    return dateFrom;
                }
                set
                {
                    if (dateFrom == value) return;

                    dateFrom = value;

                    if (Type == FilterType.Period) PerformOnChanged();
                }
            }

            private DateTime dateTo = default;
            public DateTime DateTo
            {
                get
                {
                    return dateTo;
                }
                set
                {
                    if (dateTo == value) return;

                    dateTo = value;

                    if (Type == FilterType.Period) PerformOnChanged();
                }
            }

            public int[] years = default;
            public int[] Years
            {
                get
                {
                    return years;
                }
                set
                {
                    if (years == default && value == default) return;

                    if (value != default && years != default && value.SequenceEqual(years)) return;

                    years = value;

                    if (Type == FilterType.Years) PerformOnChanged();
                }
            }

            private void PerformOnChanged()
            {
                OnChanged?.Invoke();
            }

            public string ToSql()
            {
                var sql = string.Empty;

                switch (Type)
                {
                    case FilterType.Day:
                        if (Day != default)
                        {
                            sql = string.Format("datetime BETWEEN '{0}' AND '{1}'",
                                Day.ToString("yyyy-MM-dd"), Day.AddDays(1).ToString("yyyy-MM-dd"));
                        }

                        break;
                    case FilterType.Period:
                        if (DateFrom != default && DateTo != default)
                        {
                            sql = string.Format("datetime BETWEEN '{0}' AND '{1}'",
                                DateFrom.ToString("yyyy-MM-dd"), DateTo.AddDays(1).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            if (DateFrom != default)
                            {
                                sql = string.Format("datetime >= '{0}'", DateFrom.ToString("yyyy-MM-dd 00:00:00"));
                            }

                            if (DateTo != default)
                            {
                                sql = sql.JoinExcludeEmpty(" AND ",
                                    string.Format("datetime < '{0}'", DateTo.AddDays(1).ToString("yyyy-MM-dd 00:00:00")));
                            }
                        }

                        break;
                    case FilterType.Years:
                        if (Years != default && Years.Length > 0)
                        {
                            sql = string.Format("CAST(strftime('%Y', datetime) AS INTEGER) IN ({0})",
                                string.Join(", ", Years));
                        }

                        break;
                    default:
                    case FilterType.None:
                        break;
                }

                if (!string.IsNullOrEmpty(sql)) sql = " WHERE " + sql;

                return sql;
            }
        }
    }
}