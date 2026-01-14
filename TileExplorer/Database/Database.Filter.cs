using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Linq;
using TileExplorer.Properties;

namespace TileExplorer
{
    public partial class Database
    {
        public class Filter : DefaultInstance<Filter>
        {
            public delegate void Changed();

            public event Changed OnChanged;

            public enum FilterDateType
            {
                AllDate,
                Day,
                Period,
                Years,
            }

            private FilterDateType dateType = FilterDateType.AllDate;
            public FilterDateType DateType
            {
                get => dateType;
                set
                {
                    if (dateType == value) return;

                    dateType = value;

                    PerformOnChanged();
                }
            }

            private DateTime day = default;
            public DateTime Day
            {
                get => day;
                set
                {
                    if (day == value) return;

                    day = value;

                    if (DateType == FilterDateType.Day) PerformOnChanged();
                }
            }

            private DateTime dateFrom = default;
            public DateTime DateFrom
            {
                get => dateFrom;
                set
                {
                    if (dateFrom == value) return;

                    dateFrom = value;

                    if (DateType == FilterDateType.Period) PerformOnChanged();
                }
            }

            private DateTime dateTo = default;
            public DateTime DateTo
            {
                get => dateTo;
                set
                {
                    if (dateTo == value) return;

                    dateTo = value;

                    if (DateType == FilterDateType.Period) PerformOnChanged();
                }
            }

            public void SetDates(DateTime dateFrom, DateTime dateTo)
            {
                if (this.dateFrom == dateFrom && this.dateTo == dateTo) return;

                this.dateFrom = dateFrom;
                this.dateTo = dateTo;

                if (DateType == FilterDateType.Period) PerformOnChanged();
            }

            private int[] years = default;
            public int[] Years
            {
                get => years;
                set
                {
                    if (years == default && value == default) return;

                    if (value != default && years != default && value.SequenceEqual(years)) return;

                    years = value;

                    if (DateType == FilterDateType.Years) PerformOnChanged();
                }
            }

            private bool useEquipments = false;
            public bool UseEquipments
            {
                get => useEquipments;
                set
                {
                    if (useEquipments == value) return;

                    useEquipments = value;

                    PerformOnChanged();
                }
            }

            private long[] equipments = default;
            public long[] Equipments
            {
                get => equipments;
                set
                {
                    if (equipments == default && value == default) return;

                    if (value != default && equipments != default && equipments.SequenceEqual(value)) return;

                    equipments = value;

                    if (UseEquipments) PerformOnChanged();
                }
            }

            public void Clear()
            {
                dateType = FilterDateType.AllDate;
                day = default;
                dateFrom = default;
                dateTo = default;
                years = default;
                useEquipments = false;
                equipments = default;
            }

            public void Assign(Filter source)
            {
                if (source == null)
                {
                    Clear();

                    return;
                }

                dateType = source.dateType;
                day = source.day;
                dateFrom = source.dateFrom;
                dateTo = source.dateTo;
                years = source.years;
                useEquipments = source.useEquipments;
                equipments = source.equipments;

                PerformOnChanged();
            }

            private void PerformOnChanged()
            {
                OnChanged?.Invoke();
            }

            public string ToSql()
            {
                var sql = string.Empty;

                switch (DateType)
                {
                    case FilterDateType.Day:
                        if (Day != default)
                        {
                            sql = string.Format(ResourcesSql.FilterDateBetween,
                                Day.ToString("yyyy-MM-dd"), Day.AddDays(1).ToString("yyyy-MM-dd"));
                        }

                        break;
                    case FilterDateType.Period:
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
                    case FilterDateType.Years:
                        sql = string.Format(ResourcesSql.FilterYears, string.Join(", ", Years));

                        break;
                    default:
                    case FilterDateType.AllDate:
                        break;
                }

                if (UseEquipments && Equipments != default)
                {
                    sql = sql.JoinExcludeEmpty(" AND ",
                        string.Format(ResourcesSql.FilterEquipments, string.Join(", ", Equipments)));
                }

                if (!sql.IsEmpty()) sql = " WHERE " + sql;

                return sql;
            }
        }
    }
}