using Dapper.Contrib.Extensions;
using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TileExplorer
{
    public partial class Database
    {
        public partial class Models
        {
            [Table(Tables.tracks)]
            public class Track : BaseId, IBaseText
            {
                private readonly Gpx.Track gpx = new Gpx.Track();

                public Track()
                {
                }

                public Track(string path)
                {
                    DebugWrite.Line(path);

                    gpx.OpenFromFile(path);

                    DebugWrite.Line("xml loaded");

                    TrackPoints = new List<TrackPoint>();

                    foreach (var point in gpx.Points)
                    {
                        TrackPoints.Add(new TrackPoint(point));
                    }

                    DebugWrite.Line($"point count: {TrackPoints.Count}");
                }

                [DisplayName("Название")]
                public string Text { get => gpx.Text; set => gpx.Text = value; }

                [DisplayName("Начало")]
                public DateTime DateTimeStart { get => gpx.DateTimeStart; set => gpx.DateTimeStart = value; }
                [DisplayName("Окончание")]
                public DateTime DateTimeFinish { get => gpx.DateTimeFinish; set => gpx.DateTimeFinish = value; }

                [DisplayName("Время")]
                public long Duration { get => gpx.Duration; set => gpx.Duration = value; }

                [DisplayName("Время")]
                [Write(false)]
                [Computed]
                public string DurationAsString => TimeSpan.FromSeconds(Duration).ToHoursMinutesString();

                [DisplayName("Время в движении")]
                public long DurationInMove { get => gpx.DurationInMove; set => gpx.DurationInMove = value; }

                [DisplayName("Время в движении")]
                [Write(false)]
                [Computed]
                public string DurationInMoveAsString => TimeSpan.FromSeconds(DurationInMove).ToHoursMinutesString();

                [DisplayName("Расстояние")]
                public double Distance { get => gpx.Distance; set => gpx.Distance = value; }

                [DisplayName("Скорость")]
                [Write(false)]
                public double AverageSpeed { get => gpx.AverageSpeed; }

                [DisplayName("Подъём")]
                public double EleAscent { get => gpx.EleAscent; set => gpx.EleAscent = value; }

                [DisplayName("Спуск")]
                public double EleDescent { get => gpx.EleDescent; set => gpx.EleDescent = value; }

                [Write(false)]
                public List<TrackPoint> TrackPoints { get; set; } = null;

                [Write(false)]
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
                    get => equipment; set => equipment.Assign(value);
                }

                [DisplayName("Снаряжение: ID")]
                public long EquipmentId { get => Equipment.Id; set => Equipment.Id = value; }

                [DisplayName("Снаряжение")]
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

                [Write(false)]
                public IEnumerable<TagModel> Tags { get; set; } = null;

                [DisplayName("Теги")]
                [Write(false)]
                [Computed]
                public string TagsAsString
                {
                    get
                    {
                        if (Tags == null) return string.Empty;
                        
                        return string.Join(", ", Tags.Select(tag => tag.Text));
                    }
                }

                public override void Clear()
                {
                    base.Clear();

                    gpx.Clear();

                    NewTilesCount = 0;

                    Equipment = null;

                    Tags = null;
                }

                public void Assign(Track source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    gpx.Assign(source.gpx);

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

                    if (source.Tags == null)
                    {
                        Tags = null;
                    }
                    else
                    {
                        Tags = source.Tags.ToList();
                    }
                }
            }
        }
    }
}