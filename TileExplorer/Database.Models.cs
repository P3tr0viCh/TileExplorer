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
            }

            [Table("markers")]
            public class Marker : BaseId
            {
                public double Lat { get; set; }
                public double Lng { get; set; }

                public string Text { get; set; }

                public bool IsTextVisible { get; set; } = true;

                public int OffsetX { get; set; }
                public int OffsetY { get; set; }

                public byte[] Image { get; set; }

                public MarkerImageType ImageType { get; set; } = MarkerImageType.Default;
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

                public DateTime DateTime { get; set; }

                public int Distance { get; set; }

                [Write(false)]
                [Computed]
                public List<TrackPoint> TrackPoints { get; set; }
            }

            [Table("tracks_points")]
            public class TrackPoint : BaseId
            {
                public long TrackId { get; set; } = 0;

                public double Lat { get; set; }
                public double Lng { get; set; }

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
        }
    }
}