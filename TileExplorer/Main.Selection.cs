using P3tr0viCh.Utils;
using System.Linq;
using System.Threading.Tasks;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class Main
    {

        public MapItemTrack SelectedTrack
        {
            get
            {
                if (Selected == null) return null;

                if (Selected.Type != MapItemType.Track) return null;

                return (MapItemTrack)selected;
            }
        }

        public MapItemMarker SelectedMarker
        {
            get
            {
                if (Selected == null) return null;

                if (Selected.Type != MapItemType.Marker) return null;

                return (MapItemMarker)selected;
            }
        }

        private IMapItem selected = null;

        public IMapItem Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == null)
                {
                    SelectedTrackTiles = null;
                }

                if (selected == value) return;

                if (selected != null)
                {
                    selected.Selected = false;
                }

                selected = value;

                if (selected == null)
                {
                    gMapControl.Invalidate();
                    return;
                }

                selected.Selected = true;

                switch (selected.Type)
                {
                    case MapItemType.Marker:
                        SelectedTrackTiles = null;

                        Utils.GetFrmList(ChildFormType.MarkerList)?.SetSelected(selected.Model);

                        break;
                    case MapItemType.Track:
                        SelectedTrackTiles = selected.Model as Track;

                        Utils.GetFrmList(ChildFormType.TrackList)?.SetSelected(selected.Model);

                        Utils.GetFrmList(ChildFormType.TileInfo)?.SetSelected(selected.Model);

                        break;
                }
            }
        }

        private void UpdateSelectedTrackTiles(Track track)
        {
            foreach (var tile in overlayTiles.Polygons.Cast<MapItemTile>())
            {
                tile.Selected = false;
            }

            gMapControl.Invalidate();

            if (track == null) return;

            var tiles = Task.Run(() =>
            {
                return Database.Default.ListLoadAsync<Tile>(track);
            }).Result;

            foreach (var tile in from item in overlayTiles.Polygons.Cast<MapItemTile>()
                                 from tile in tiles
                                 where item.Model.X == tile.X && item.Model.Y == tile.Y
                                 select item)
            {
                tile.Selected = true;
            }
        }

        public Track SelectedTrackTiles
        {
            set
            {
                UpdateSelectedTrackTiles(value);
            }
        }

        public void SelectMapItem(object sender, BaseId value)
        {
            var item = FindMapItem(value);

            if (item == null)
            {
                if (value is Track track)
                {
                    item = Task.Run(async () =>
                    {
                        track.TrackPoints = await Database.Actions.ListLoadAsync<TrackPoint>(track);

                        if (track.TrackPoints == null) return null;

                        return OverlayAddTrack(overlayTracks, track);
                    }).Result;
                }
            }

            Selected = item;
        }
    }
}