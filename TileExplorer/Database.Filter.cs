using P3tr0viCh.Utils;
using System;
using System.Linq;
using TileExplorer.Properties;

namespace TileExplorer
{
    public partial class Database
    {
        public class Filter
        {
            private static readonly Filter defaultInstance = new Filter();

            public static Filter Default => defaultInstance;

            public delegate void Changed();

            public event Changed OnChanged;

            public enum FilterType
            {
                None,
                Day,
                Period,
                Years,
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

            public void SetDates(DateTime dateFrom, DateTime dateTo)
            {
                if (this.dateFrom == dateFrom && this.dateTo == dateTo) return;

                this.dateFrom = dateFrom;
                this.dateTo = dateTo;

                if (Type == FilterType.Period) PerformOnChanged();
            }

            private int[] years = default;
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
                            sql = string.Format(ResourcesSql.FilterDateBetween,
                                Day.ToString("yyyy-MM-dd"), Day.AddDays(1).ToString("yyyy-MM-dd"));
                        }

                        break;
                    case FilterType.Period:
                        if (DateFrom != default && DateTo != default)
                        {
                            sql = string.Format(ResourcesSql.FilterDateBetween,
                                DateFrom.ToString("yyyy-MM-dd"), DateTo.AddDays(1).ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            if (DateFrom != default)
                            {
                                sql = string.Format(ResourcesSql.FilterDateFrom, DateFrom.ToString("yyyy-MM-dd 00:00:00"));
                            }

                            if (DateTo != default)
                            {
                                sql = sql.JoinExcludeEmpty(" AND ",
                                    string.Format(ResourcesSql.FilterDateTo, DateTo.AddDays(1).ToString("yyyy-MM-dd 00:00:00")));
                            }
                        }

                        break;
                    case FilterType.Years:
                        sql = string.Format(ResourcesSql.FilterYears, string.Join(", ", Years));

                        break;
                    default:
                    case FilterType.None:
                        break;
                }

                if (!sql.IsEmpty()) sql = " WHERE " + sql;

                return sql;
            }
        }
    }
}