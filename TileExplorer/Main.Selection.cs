using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using System;
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
                    switch (selected.Type)
                    {
                        case MapItemType.Track:
                            selected.IsVisible = miMainShowTracks.Checked;
                            break;
                    }

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

                        overlayMarkers.Markers.Remove((GMapMarker)selected);
                        overlayMarkers.Markers.Add((GMapMarker)selected);

                        Utils.GetFrmList(ChildFormType.MarkerList)?.SetSelected(selected.Model);

                        break;
                    case MapItemType.Track:
                        selected.IsVisible = true;

                        overlayTracks.Routes.Remove((GMapRoute)selected);
                        overlayTracks.Routes.Add((GMapRoute)selected);

                        SelectedTrackTiles = selected.Model as Track;

                        Utils.GetChildForms<FrmList>(ChildFormType.TrackList | ChildFormType.TileInfo).ForEach(frm =>
                        {
                            frm.SetSelected(selected.Model);
                        });

                        break;
                }
            }
        }

        private async void UpdateSelectedTrackTiles(Track track)
        {
            foreach (var tile in overlayTiles.Polygons.Cast<MapItemTile>())
            {
                tile.Selected = false;
            }

            gMapControl.Invalidate();

            if (track == null) return;

            if (track.TrackTiles == null)
            {
                await Utils.Tracks.CalcTrackTilesAsync(track);
            }

            foreach (var tile in from item in overlayTiles.Polygons.Cast<MapItemTile>()
                                 from tile in track.TrackTiles
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

        public async Task SelectMapItemAsync(object sender, BaseId value)
        {
            var item = FindMapItem(value);

            if (item == null)
            {
                if (value is Track track)
                {
                    try
                    {
                        track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(track);

                        item = OverlayAddTrack(track);
                    }
                    catch (Exception e)
                    {
                        DebugWrite.Error(e);
                    }
                }
            }

            Selected = item;
        }
    }
}