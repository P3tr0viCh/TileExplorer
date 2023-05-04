using GMap.NET;

namespace TileExplorer
{
    public static partial class Utils
    {
        public static class Osm
        {
            public static int LatToTileY(double lat)
            {
                return P3tr0viCh.Utils.Osm.LatToTileY(lat, Const.TILE_ZOOM);
            }

            public static int LatToTileY(PointLatLng point)
            {
                return LatToTileY(point.Lat);
            }

            public static int LngToTileX(double lng)
            {
                return P3tr0viCh.Utils.Osm.LngToTileX(lng, Const.TILE_ZOOM);
            }

            public static int LngToTileX(PointLatLng point)
            {
                return LngToTileX(point.Lng);
            }

            public static double TileXToLng(int x)
            {
                return P3tr0viCh.Utils.Osm.TileXToLng(x, Const.TILE_ZOOM);
            }

            public static double TileYToLat(int y)
            {
                return P3tr0viCh.Utils.Osm.TileYToLat(y, Const.TILE_ZOOM);
            }
        }
    }
}