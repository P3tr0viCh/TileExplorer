using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace TileExplorer
{
    public partial class Database
    {
        public class Models
        {
            public class BaseId
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
            public class Marker : BaseId
            {
                [DisplayName("Широта")]
                public double Lat { get; set; }
                [DisplayName("Долгота")]
                public double Lng { get; set; }

                [DisplayName("Текст")]
                public string Text { get; set; }

                public bool IsTextVisible { get; set; } = true;

                public int OffsetX { get; set; } = DEFAULT_OFFSET_X;
                public int OffsetY { get; set; } = DEFAULT_OFFSET_Y;

                public byte[] Image { get; set; }

                public MarkerImageType ImageType { get; set; } = MarkerImageType.Default;

                public void Assign(Marker source)
                {
                    base.Assign(source);

                    Lat = source.Lat;
                    Lng = source.Lng;
                    Text = source.Text;
                    IsTextVisible = source.IsTextVisible;
                    OffsetX = source.OffsetX;
                    OffsetY = source.OffsetY;
                    ImageType = source.ImageType;
                }
            }

            [Table("tiles")]
            public class Tile : BaseId
            {
                public int X { get; set; }
                public int Y { get; set; }

                [Write(false)]
                [Computed]
                public TileStatus Status { get; set; } = TileStatus.Unknown;

                [Write(false)]
                [Computed]
                public int ClusterId { get; set; } = -1;

                public override string ToString()
                {
                    return $"{GetType().Name}{{Id={Id}, X={X}, Y={Y}, Status:{Status}}}";
                }
            }

            [Table("tracks")]
            public class Track : BaseId
            {
                [DisplayName("Название")]
                public string Text { get; set; }

                [DisplayName("Начало")]
                public DateTime DateTimeStart { get; set; }
                [DisplayName("Окончание")]
                public DateTime DateTimeFinish { get; set; }

                [DisplayName("Время")]
                [Write(false)]
                [Computed]
                public TimeSpan Duration
                {
                    get
                    {
                        return DateTimeFinish - DateTimeStart;
                    }
                }

                [DisplayName("Расстояние")]
                public double Distance { get; set; }

                [Write(false)]
                [Computed]
                public List<TrackPoint> TrackPoints { get; set; }
            }

            [Table("tracks_points")]
            public class TrackPoint : BaseId
            {
                public long TrackId { get; set; } = 0;

                public int Num { get; set; }

                public DateTime DateTime { get; set; }

                public double Lat { get; set; }
                public double Lng { get; set; }

                public double Ele { get; set; }

                public double Distance { get; set; }
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

            public class Results : BaseId
            {
                [DisplayName("Год")]
                public int Year { get => (int)Id; set => Id = value; }

                [DisplayName("Треки")]
                public int Count { get; set; } = 0;

                [DisplayName("Расстояние")]
                public double DistanceSum { get; set; } = 0;

                public new void Clear()
                {
                    base.Clear();

                    Count = 0;
                    DistanceSum = 0;
                }

                public void Assign(Results source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    Count = source.Count;
                    DistanceSum = source.DistanceSum;
                }
            }
        }
    }
}