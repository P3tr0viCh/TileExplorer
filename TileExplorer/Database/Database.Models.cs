using Dapper.Contrib.Extensions;
using GMap.NET;
using Newtonsoft.Json;
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.ComponentModel;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Database
    {
        public partial class Models
        {
            private const int DefaultOffsetX = 20;
            private const int DefaultOffsetY = -10;

            [Table(Tables.markers)]
            public class Marker : BaseText
            {
                public Marker()
                {
                }

                public Marker(PointLatLng point)
                {
                    Lat = point.Lat;
                    Lng = point.Lng;
                }

                [DisplayName("Широта")]
                public double Lat { get; set; }
                [DisplayName("Долгота")]
                public double Lng { get; set; }

                public bool IsTextVisible { get; set; } = true;

                public int OffsetX { get; set; } = DefaultOffsetX;
                public int OffsetY { get; set; } = DefaultOffsetY;

                public void Assign(Marker source)
                {
                    base.Assign(source);

                    Lat = source.Lat;
                    Lng = source.Lng;

                    IsTextVisible = source.IsTextVisible;

                    OffsetX = source.OffsetX;
                    OffsetY = source.OffsetY;
                }
            }

            [Table(Tables.tiles)]
            public class Tile : BaseId
            {
                public Tile()
                {
                }

                public Tile(int x, int y)
                {
                    X = x;
                    Y = y;
                }

                public Tile(PointLatLng point) : this(Utils.Osm.LngToTileX(point), Utils.Osm.LatToTileY(point))
                {
                }

                public int X { get; set; }
                public int Y { get; set; }

                [Write(false)]
                public TileStatus Status { get; set; } = TileStatus.Unknown;

                [Write(false)]
                public int ClusterId { get; set; } = -1;

                [Write(false)]
                public int HeatmapValue { get; set; } = 0;

                [Write(false)]
                public int TrackCount { get; set; } = 0;

                public override string ToString()
                {
                    return JsonConvert.SerializeObject(this);
                }
            }

            [Table(Tables.tracks_points)]
            public class TrackPoint : Gpx.Point, IBaseId
            {
                public TrackPoint()
                {
                }

                public TrackPoint(Gpx.Point point)
                {
                    Num = point.Num;

                    Lat = point.Lat;
                    Lng = point.Lng;

                    DateTime = point.DateTime;

                    Ele = point.Ele;

                    Distance = point.Distance;
                }

                [Key]
                public long Id { get; set; } = Sql.NewId;

                public long TrackId { get; set; } = Sql.NewId;

                public bool ShowOnMap { get; set; }
            }

            [Table(Tables.tracks_tiles)]
            public class TracksTiles : BaseId
            {
                public long TrackId { get; set; } = Sql.NewId;

                public long TileId { get; set; } = Sql.NewId;
            }

            public class TracksInfo
            {
                public int Count { get; set; }

                public double Distance { get; set; }
            }

            public class ResultYears : BaseId
            {
                [DisplayName("Год")]
                public int Year { get => (int)Id; set => Id = value; }

                [DisplayName("Треки")]
                public int Count { get; set; } = 0;

                [DisplayName("Время в днях")]
                public double DurationSum { get; set; } = 0;

                [DisplayName("Время")]
                public string DurationSumAsString => TimeSpan.FromDays(DurationSum).ToHoursMinutesString();

                [DisplayName("Расстояние")]
                public double DistanceSum { get; set; } = 0;

                [DisplayName("50-")]
                public int DistanceStep0 { get; set; } = 0;
                [DisplayName("50+")]
                public int DistanceStep1 { get; set; } = 0;
                [DisplayName("100+")]
                public int DistanceStep2 { get; set; } = 0;

                [DisplayName("Подъём")]
                public double EleAscentSum { get; set; } = 0;

                [DisplayName("500-")]
                public int EleAscentStep0 { get; set; } = 0;
                [DisplayName("500+")]
                public int EleAscentStep1 { get; set; } = 0;
                [DisplayName("1000+")]
                public int EleAscentStep2 { get; set; } = 0;
            }

            public class ResultEquipments : BaseText
            {
                [DisplayName("Треки")]
                public int Count { get; set; } = 0;

                [DisplayName("Расстояние")]
                public double DistanceSum { get; set; } = 0;

                [DisplayName("Время в днях")]
                public double DurationSum { get; set; } = 0;

                [DisplayName("Время")]
                public string DurationSumAsString => TimeSpan.FromDays(DurationSum).ToHoursMinutesString();
            }
            
            [Table(Tables.tags)]
            public class TagModel : BaseText
            {
            }

            [Table(Tables.tracks_tags)]
            public class TracksTags : BaseId
            {
                public long TrackId { get; set; } = Sql.NewId;

                public long TagId { get; set; } = Sql.NewId;
            }

            [Table(Tables.equipments)]
            public class Equipment : BaseText
            {
                [DisplayName("Марка")]
                public string Brand { get; set; }

                [DisplayName("Модель")]
                public string Model { get; set; }

                public override void Clear()
                {
                    base.Clear();

                    Brand = string.Empty;
                    Model = string.Empty;
                }

                public void Assign(Equipment source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    Text = source.Text;
                    Brand = source.Brand;
                    Model = source.Model;
                }
            }

            [Table(Tables.tracks)]
            public class TracksDistanceByMonth : BaseId
            {
                [DisplayName("Год")]
                public int Year { get; set; }

                [DisplayName("Месяц")]
                public int Month { get; set; }

                [DisplayName("День")]
                public int Day { get; set; }

                [DisplayName("Расстояние")]
                public double Distance { get; set; }
            }
        }
    }
}