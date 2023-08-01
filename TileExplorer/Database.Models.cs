using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System;

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

                public void Assign(BaseId source)
                {
                    Id = source.Id;
                }
            }

            [Table("markers")]
            public class MapMarker : BaseId
            {
                public double Lat { get; set; }
                public double Lng { get; set; }

                public string Text { get; set; }

                public bool IsTextVisible { get; set; } = true;

                public int OffsetX { get; set; } = DEFAULT_OFFSET_X;
                public int OffsetY { get; set; } = DEFAULT_OFFSET_Y;

                public byte[] Image { get; set; }

                public MarkerImageType ImageType { get; set; } = MarkerImageType.Default;

                public void Assign(MapMarker source)
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
                public string Text { get; set; }

                public DateTime DateTimeStart { get; set; }
                public DateTime DateTimeFinish { get; set; }

                [Write(false)]
                [Computed]
                public TimeSpan Duration
                {
                    get
                    {
                        return DateTimeFinish - DateTimeStart;
                    }
                }

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
                public int Year
                {
                    get
                    {
                        return (int)Id;
                    }
                    set
                    {
                        Id = value;
                    }
                }

                public int Count { get; set; }

                public double DistanceSum { get; set; }
            }
        }
    }
}