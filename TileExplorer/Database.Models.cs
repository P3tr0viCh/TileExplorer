using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using static TileExplorer.Enums;
using Newtonsoft.Json;
using GMap.NET;
using P3tr0viCh.Utils;

namespace TileExplorer
{
    public partial class Database
    {
        public class Models
        {
            private const int DEFAULT_OFFSET_X = 20;
            private const int DEFAULT_OFFSET_Y = -10;

            public interface IBaseId
            {
                long Id { get; set; }
            }

            public interface IModelText
            {
                string Text { get; set; }
            }

            public class BaseId : IBaseId
            {
                [Key]
                public long Id { get; set; } = 0;

                public void Clear()
                {
                    Id = 0;
                }

                public void Assign(BaseId source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    Id = source.Id;
                }
            }

            [Table("markers")]
            public class Marker : BaseId, IModelText
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

                [DisplayName("Текст")]
                public string Text { get; set; }

                public bool IsTextVisible { get; set; } = true;

                public int OffsetX { get; set; } = DEFAULT_OFFSET_X;
                public int OffsetY { get; set; } = DEFAULT_OFFSET_Y;

                public void Assign(Marker source)
                {
                    base.Assign(source);

                    Lat = source.Lat;
                    Lng = source.Lng;
                    Text = source.Text;
                    IsTextVisible = source.IsTextVisible;
                    OffsetX = source.OffsetX;
                    OffsetY = source.OffsetY;
                }
            }

            [Table("tiles")]
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
                [Computed]
                public TileStatus Status { get; set; } = TileStatus.Unknown;

                [Write(false)]
                [Computed]
                public int ClusterId { get; set; } = -1;

                [Write(false)]
                [Computed]
                public List<Track> Tracks { get; set; } = null;

                public override string ToString()
                {
                    return JsonConvert.SerializeObject(this);
                }
            }

            [Table("tracks")]
            public class Track : BaseId, IModelText
            {
                private readonly Gpx.Track gpx = new Gpx.Track();

                [Write(false)]
                public Gpx.Track Gpx
                {
                    get => gpx;
                    set => gpx.Assign(value);
                }

                [DisplayName("Название")]
                public string Text
                {
                    get => gpx.Text;
                    set => gpx.Text = value;
                }

                [DisplayName("Начало")]
                public DateTime DateTimeStart
                {
                    get => gpx.DateTimeStart;
                    set => gpx.DateTimeStart = value;
                }
                [DisplayName("Окончание")]
                public DateTime DateTimeFinish
                {
                    get => gpx.DateTimeFinish;
                    set => gpx.DateTimeFinish = value;
                }

                [DisplayName("Время")]
                public long Duration
                {
                    get => gpx.Duration;
                    set => gpx.Duration = value;
                }

                [DisplayName("Время")]
                [Write(false)]
                [Computed]
                public string DurationAsString => TimeSpan.FromSeconds(Duration).ToHoursMinutesString();

                [DisplayName("Время в движении")]
                public long DurationInMove
                {
                    get => gpx.DurationInMove;
                    set => gpx.DurationInMove = value;
                }

                [DisplayName("Время в движении")]
                [Write(false)]
                [Computed]
                public string DurationInMoveAsString => TimeSpan.FromSeconds(DurationInMove).ToHoursMinutesString();

                [DisplayName("Расстояние")]
                public double Distance
                {
                    get => gpx.Distance;
                    set => gpx.Distance = value;
                }

                [DisplayName("Скорость")]
                [Write(false)]
                public float AverageSpeed { get; set; }

                [DisplayName("Подъём")]
                public float EleAscent
                {
                    get => gpx.EleAscent;
                    set => gpx.EleAscent = value;
                }

                [Write(false)]
                [Computed]
                public List<TrackPoint> TrackPoints { get; set; } = null;

                [Write(false)]
                [Computed]
                public List<Tile> TrackTiles { get; set; } = null;

                [DisplayName("Плитки +")]
                [Write(false)]
                public int NewTilesCount { get; set; } = 0;

                private readonly Equipment equipment = new Equipment();
                [DisplayName("Снаряжение")]
                [Write(false)]
                [Computed]
                public Equipment Equipment
                {
                    get
                    {
                        return equipment;
                    }
                    set
                    {
                        equipment.Assign(value);
                    }
                }
                [DisplayName("Снаряжение")]
                [Write(false)]
                [Computed]
                public string EquipmentName => Equipment.Name;
                [DisplayName("Снаряжение: ID")]
                public long EquipmentId { get => Equipment.Id; set => Equipment.Id = value; }
                [DisplayName("Снаряжение: название")]
                [Write(false)]
                [Computed]
                public string EquipmentText { get => Equipment.Text; set => Equipment.Text = value; }
                [DisplayName("Снаряжение: марка")]
                [Write(false)]
                [Computed]
                public string EquipmentBrand { get => Equipment.Brand; set => Equipment.Brand = value; }

                [DisplayName("Снаряжение: модель")]
                [Write(false)]
                [Computed]
                public string EquipmentModel { get => Equipment.Model; set => Equipment.Model = value; }

                public new void Clear()
                {
                    base.Clear();

                    Text = string.Empty;

                    DateTimeStart = default;
                    DateTimeFinish = default;

                    Duration = 0;
                    DurationInMove = 0;

                    Distance = 0;

                    EleAscent = 0;

                    TrackPoints = null;

                    NewTilesCount = 0;

                    Equipment = null;
                }

                public void Assign(Track source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    Gpx = source.Gpx;

                    if (source.TrackPoints == null)
                    {
                        TrackPoints = null;
                    }
                    else
                    {
                        TrackPoints = new List<TrackPoint>();

                        TrackPoints.AddRange(source.TrackPoints);

                    }

                    NewTilesCount = source.NewTilesCount;

                    Equipment = source.Equipment;
                }
            }

            [Table("tracks_points")]
            public class TrackPoint : BaseId
            {
                public long TrackId { get; set; } = 0;

                public int Num { get; set; }

                public DateTime DateTime { get; set; }

                public double Lat { get; set; }
                public double Lng { get; set; }

                public float Ele { get; set; }

                public double Distance { get; set; }

                public bool ShowOnMap { get; set; }
            }

            [Table("tracks_tiles")]
            public class TracksTiles : BaseId
            {
                public long TrackId { get; set; } = 0;

                public long TileId { get; set; } = 0;
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

                [DisplayName("Расстояние")]
                public double DistanceSum { get; set; } = 0;

                [DisplayName("Время в днях")]
                public double DurationSum { get; set; } = 0;

                [DisplayName("Время")]
                public string DurationSumAsString => TimeSpan.FromDays(DurationSum).ToHoursMinutesString();

                [DisplayName("50-")]
                public double DistanceStep0 { get; set; } = 0;
                [DisplayName("50+")]
                public double DistanceStep1 { get; set; } = 0;
                [DisplayName("100+")]
                public double DistanceStep2 { get; set; } = 0;
            }

            public class ResultEquipments : BaseId
            {
                [DisplayName("Название")]
                public string Text { get; set; } = string.Empty;

                [DisplayName("Треки")]
                public int Count { get; set; } = 0;

                [DisplayName("Расстояние")]
                public double DistanceSum { get; set; } = 0;

                [DisplayName("Время в днях")]
                public double DurationSum { get; set; } = 0;

                [DisplayName("Время")]
                public string DurationSumAsString => TimeSpan.FromDays(DurationSum).ToHoursMinutesString();
            }

            [Table("equipments")]
            public class Equipment : BaseId
            {
                [DisplayName("Название")]
                public string Text { get; set; }

                [DisplayName("Марка")]
                public string Brand { get; set; }

                [DisplayName("Модель")]
                public string Model { get; set; }

                [DisplayName("Название")]
                [Write(false)]
                [Computed]
                public string Name
                {
                    get
                    {
                        if (Text.IsEmpty())
                        {
                            if (Brand.IsEmpty() && Model.IsEmpty())
                            {
                                return string.Empty;
                            }

                            if (Brand.IsEmpty())
                            {
                                return Model;
                            }

                            if (Model.IsEmpty())
                            {
                                return Brand;
                            }

                            return Brand + " " + Model;
                        }

                        return Text;
                    }
                }

                public new void Clear()
                {
                    base.Clear();

                    Text = string.Empty;
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

            [Table("tracks")]
            public class TracksTree : BaseId
            {
                [DisplayName("Год")]
                public int Year { get; set; }

                [DisplayName("Месяц")]
                public int Month { get; set; }
            }
        }
    }
}