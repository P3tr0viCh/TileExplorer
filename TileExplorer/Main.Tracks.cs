using P3tr0viCh.Utils;
using static TileExplorer.Database.Models;
using System.Threading.Tasks;
using static TileExplorer.Enums;
using TileExplorer.Properties;
using static TileExplorer.Interfaces;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Collections.Generic;
using System;

namespace TileExplorer
{
    public partial class Main
    {
        private async Task LoadTracksAsync()
        {
            DebugWrite.Line("start");

            overlayTracks.Clear();

            var tracks = await Database.Default.ListLoadAsync<Track>();

            foreach (var track in tracks)
            {
                track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(track);

                overlayTracks.Routes.Add(new MapItemTrack(track));

#if DEBUG && SHOW_TRACK_KM
                double lat1 = 0, lng1 = 0, lat2 = 0, lng2 = 0;

                int i = 0;

                foreach (var trackPoint in track.TrackPoints)
                {
                    lat2 = trackPoint.Lat;
                    lng2 = trackPoint.Lng;

                    if (Geo.Haversine(lat1, lng1, lat2, lng2) < 1000) continue;

                    tracksOverlay.Markers.Add(new MapMarker(new MarkerModel()
                    {
                        Lat = trackPoint.Lat,
                        Lng = trackPoint.Lng,
                        Text = i.ToString(),
                        IsTextVisible = true
                    }));

                    i++;

                    DebugWrite.Line(Geo.Haversine(lat1, lng1, lat2, lng2));
                    
                    lat1 = lat2;
                    lng1 = lng2;
                }
#endif
            }

            DebugWrite.Line("end");
        }

        public MapItemTrack SelectedTrack
        {
            get
            {
                if (Selected == null) return null;

                if (Selected.Type != MapItemType.Track) return null;

                return (MapItemTrack)selected;
            }
        }

        private void TrackChange(Track track)
        {
            if (track == null) return;

            FrmTrack.ShowDlg(this, track);

            SelectMapItem(this, track);
        }

        public void TrackChanged(Track track)
        {
            Utils.GetFrmList(ChildFormType.TrackList)?.ListItemChange(track);

            SelectMapItem(this, track);
        }

        private void TrackDelete(Track track)
        {
            if (track == null) return;

            var name = track.Text;

            if (name.IsEmpty())
            {
                name = track.DateTimeStart.ToString();
            }

            if (!Msg.Question(string.Format(Resources.QuestionTrackDelete, name))) return;

            if (Database.Actions.TrackDeleteAsync(track).Result)
            {
                Selected = null;

                overlayTracks.Routes.Remove(
                  overlayTracks.Routes.Cast<IMapItem>().Where(i => i.Model.Id == track.Id)?
                      .Cast<MapItemTrack>().FirstOrDefault());

                UpdateData(DataLoad.Tiles);

                foreach (var frm in Utils.GetChildForms<Form>(null))
                {
                    switch (((IChildForm)frm).FormType)
                    {
                        case ChildFormType.TrackList:
                            ((IListForm)frm).ListItemDelete(track);

                            break;
                        case ChildFormType.TracksTree:
                        case ChildFormType.ResultYears:
                        case ChildFormType.ResultEquipments:
                            ((IUpdateDataForm)frm).UpdateData();

                            break;
                        case ChildFormType.ChartTrackEle:
                            if (((FrmChartTrackEle)frm).Track.Id == track.Id)
                            {
                                frm.Close();
                            }

                            break;
                    }
                }
            }
        }

        private async Task<bool> OpenTracksAsync(string[] files)
        {
            var status = ProgramStatus.Start(Status.LoadGpx);

            Track track;

            List<Tile> trackTiles;

            try
            {
                var showDlg = true;

                var canToAll = files.Count() > 1;

                Equipment equipmentToAll = null;

                var exitForEach = false;

                foreach (var file in files)
                {
                    track = Utils.Tracks.OpenTrackFromFile(file);

                    DebugWrite.Line("OpenTrackFromFileAsync done");

                    if (showDlg)
                    {
                        switch (FrmTrack.ShowDlg(this, track, canToAll))
                        {
                            case DialogResult.Cancel:
                                continue;
                            case DialogResult.Abort:
                                exitForEach = true;

                                break;
                            case DialogResult.Yes:
                                showDlg = false;

                                equipmentToAll = track.Equipment;

                                break;
                        }
                    }
                    else
                    {
                        track.Equipment = equipmentToAll;

                        await Database.Default.TrackSaveAsync(track);
                    }

                    if (exitForEach)
                    {
                        break;
                    }

                    var mapTrack = new MapItemTrack(track);

                    overlayTracks.Routes.Add(mapTrack);

                    Selected = mapTrack;

                    trackTiles = Utils.Tiles.GetTilesFromTrack(track);

                    DebugWrite.Line("GetTilesFromTrackAsync done");

                    var saveTiles = new List<Tile>();

                    int id;

                    foreach (var trackTile in trackTiles)
                    {
                        id = await Database.Default.GetTileIdByXYAsync(trackTile);

                        if (id == 0)
                        {
                            saveTiles.Add(trackTile);
                        }
                        else
                        {
                            trackTile.Id = id;
                        }
                    }

                    DebugWrite.Line("saveTiles count: " + saveTiles.Count);

                    await Database.Default.TilesSaveAsync(saveTiles);

                    DebugWrite.Line("SaveTilesAsync done");

                    var tracksTiles = new List<TracksTiles>();

                    foreach (var tile in trackTiles)
                    {
                        tracksTiles.Add(new TracksTiles()
                        {
                            TrackId = track.Id,
                            TileId = tile.Id,
                        });
                    }

                    DebugWrite.Line("tracksTiles count: " + tracksTiles.Count);

                    await Database.Default.TracksTilesSaveAsync(tracksTiles);

                    DebugWrite.Line("SaveTracksTilesAsync done");

                    Utils.GetFrmList(ChildFormType.TrackList)?.ListItemChange(track);
                }

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                var msg = e.InnerException != null ? e.InnerException.Message : e.Message;

                Msg.Error(msg);

                return false;
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
        }


        private void OpenTracks()
        {
            openFileDialog.FileName = string.Empty;

            openFileDialog.InitialDirectory = AppSettings.Local.Default.DirectoryTracks;

            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

            AppSettings.Local.Default.DirectoryTracks = Directory.GetParent(openFileDialog.FileName).FullName;

            Task.Run(() => this.InvokeIfNeeded(async () =>
                {
                    if (await OpenTracksAsync(openFileDialog.FileNames))
                    {
                        await UpdateDataAsync(DataLoad.Tiles);

                        foreach (var frm in Utils.GetChildForms<IUpdateDataForm>(null))
                        {
                            switch (frm.FormType)
                            {
                                case ChildFormType.TracksTree:
                                case ChildFormType.ResultYears:
                                case ChildFormType.ResultEquipments:
                                    frm.UpdateData();

                                    break;
                            }
                        }
                    }
                }));
        }
    }
}