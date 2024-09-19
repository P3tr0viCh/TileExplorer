using GMap.NET.WindowsForms;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    public partial class Main
    {
        private bool tracksLoaded = false;

        private IMapItem OverlayAddTrack(GMapOverlay overlay, Track track)
        {
            var item = new MapItemTrack(track);

            overlay.Routes.Add(item);

            return item;
        }

        private CancellationTokenSource ctsTracks;

        private async Task LoadTracksAsync()
        {
            DebugWrite.Line("start");

            try
            {
                tracksLoaded = false;

                overlayTracks.Clear();

                var tracks = await Database.Default.ListLoadAsync<Track>();

                foreach (var track in tracks)
                {
                    // await Task.Delay(1000, ctsTracks.Token);

                    if (ctsTracks.IsCancellationRequested) return;

                    track.TrackPoints = await Database.Default.ListLoadAsync<TrackPoint>(track);

                    OverlayAddTrack(overlayTracks, track);

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

                tracksLoaded = true;
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.MsgDatabaseLoadListTrackFail, e.Message);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private void LoadTracksStop()
        {
            ctsTracks?.Cancel();
            ctsTracks?.Dispose();
        }

        private void LoadTracks()
        {
            LoadTracksStop();

            ctsTracks = new CancellationTokenSource();

            Task.Run(() => this.InvokeIfNeeded(async () => await LoadTracksAsync()), ctsTracks.Token);
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

            Utils.GetFrmList(ChildFormType.TileInfo)?.ListItemChange(track);

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
                        case ChildFormType.TileInfo:
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
                        UpdateData(DataLoad.Tiles);

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


        private CancellationTokenSource ctsTracksInfo;

        private async Task LoadTracksInfoAsync()
        {
            DebugWrite.Line("start");

            try
            {
                var tracksInfo = await Database.Default.LoadTracksInfoAsync(Database.Filter.Default);

                this.InvokeIfNeeded(() =>
                {
                    statusStripPresenter.TracksCount = tracksInfo.Count;
                    statusStripPresenter.TracksDistance = tracksInfo.Distance / 1000.0;
                });
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private void LoadTracksInfoStop()
        {
            ctsTracksInfo?.Cancel();
            ctsTracksInfo?.Dispose();
        }

        private void LoadTracksInfo()
        {
            LoadMarkersStop();

            ctsTracksInfo = new CancellationTokenSource();

            Task.Run(() => this.InvokeIfNeeded(async () => await LoadTracksInfoAsync()), ctsTracksInfo.Token);
        }
    }
}