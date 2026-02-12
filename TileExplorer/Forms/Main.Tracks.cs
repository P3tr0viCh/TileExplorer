using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class Main
    {
        private bool tracksLoaded = false;

        private IMapItem OverlayAddTrack(Track track)
        {
            var item = new MapItemTrack(track);

            overlayTracks.Routes.Add(item);

            return item;
        }

        private readonly WrapperCancellationTokenSource ctsTracks = new WrapperCancellationTokenSource();

        private async Task LoadTracksAsync()
        {
            DebugWrite.Line("start");

            ctsTracks.Start();

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                tracksLoaded = false;

                overlayTracks.Clear();

                var tracks = await Database.Default.ListLoadAsync<Track>();

                foreach (var track in tracks)
                {
                    // await Task.Delay(1000, ctsTracks.Token);

                    if (ctsTracks.IsCancellationRequested) return;

                    var trackPoints = await Database.Default.ListLoadAsync<TrackPoint>(track);

                    track.TrackPoints = trackPoints.ToList();

                    OverlayAddTrack(track);

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
                ctsTracks.Finally();

                ProgramStatus.Default.Stop(status);

                DebugWrite.Line("end");
            }
        }

        public async Task<bool> TrackChangeAsync(List<Track> tracks)
        {
            var count = tracks?.Count();

            if (count == 0) return false;

            if (count == 1)
            {
                var track = tracks.First();

                if (track.IsNew)
                {
                    var opened = await OpenTracksAsync(null);

                    if (opened.IsEmpty()) return false;

                    tracks.Clear();

                    tracks.AddRange(opened);
                }
                else
                {
                    if (!FrmTrack.ShowDlg(this, track)) return false;
                }
            }
            else
            {
                if (!FrmTrackList.ShowDlg(this, tracks)) return false;

                await Database.Actions.UpdateTracksAsync(tracks);
            }

            await TrackChangedAsync(tracks);

            return true;
        }

        private async Task TrackChangeAsync(Track track)
        {
            await TrackChangeAsync(new List<Track>() { track });
        }

        private async Task<bool> TrackDeleteAsync(Track track)
        {
            var name = track.Text;

            if (name.IsEmpty())
            {
                name = Utils.DateTimeToString(track.DateTimeStart);
            }

            if (!Msg.Question(Resources.QuestionTrackDelete, name)) return false;

            if (!await Database.Actions.ListItemDeleteAsync(track)) return false;

            Selected = null;

            var tracks = new List<Track>() { track };

            await TracksDeletedAsync(tracks);

            return true;
        }

        public async Task TrackChangedAsync(IEnumerable<Track> tracks)
        {
            if (tracks.IsEmpty()) return;

            Utils.Forms.ChildFormsListItemsChange(
                ChildFormType.TileInfo |
                ChildFormType.ChartTrackEle |
                ChildFormType.ChartTracksByYear |
                ChildFormType.ChartTracksByMonth,
                tracks);

            await UpdateDataAsync(
                DataLoad.Tiles | DataLoad.TracksInfo,
                ChildFormType.Filter | ChildFormType.ResultYears | ChildFormType.ResultEquipments);

            SelectTrackList(tracks);
        }

        public async Task TracksDeletedAsync(IEnumerable<Track> tracks)
        {
            if (tracks.IsEmpty()) return;

            foreach (var track in tracks)
            {
                overlayTracks.Routes.Remove(
                    overlayTracks.Routes.Cast<MapItemTrack>().Where(i => i.Model.Id == track.Id)?
                        .FirstOrDefault());
            }

            Utils.Forms.ChildFormsListItemsDelete(
                ChildFormType.TrackList |
                ChildFormType.TileInfo |
                ChildFormType.ChartTrackEle |
                ChildFormType.ChartTracksByYear |
                ChildFormType.ChartTracksByMonth,
                tracks);

            await UpdateDataAsync(
                DataLoad.Tiles | DataLoad.TracksInfo,
                ChildFormType.Filter | ChildFormType.ResultYears | ChildFormType.ResultEquipments);

            Selected = null;
        }

        private async Task<List<Track>> InternalOpenTracksAsync(string[] files)
        {
            var status = ProgramStatus.Default.Start(Status.LoadGpx);

            var tracks = new List<Track>();

            try
            {
                var showDlg = true;

                var canToAll = files.Length > 1;

                Equipment equipmentToAll = null;

                foreach (var file in files)
                {
                    var track = await Utils.Tracks.OpenTrackFromFileAsync(file);

                    // await Task.Delay(1000);

                    if (showDlg)
                    {
                        var showDlgResult = FrmTrack.ShowDlg(this, track, canToAll, false);

                        switch (showDlgResult)
                        {
                            case DialogResult.Cancel:
                                continue;
                            case DialogResult.Abort:
                                return tracks;
                            case DialogResult.Yes:
                                showDlg = false;

                                equipmentToAll = track.Equipment;

                                break;
                            case DialogResult.OK:
                                break;
                        }
                    }
                    else
                    {
                        track.Equipment = equipmentToAll;
                    }

                    await Database.Default.TrackSaveNewAsync(track);

                    DebugWrite.Line("TrackSaveNewAsync done");

                    tracks.Add(track);

                    OverlayAddTrack(track);
                }
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }

            return tracks;
        }

        private async Task<List<Track>> OpenTracksAsync(string[] files)
        {
            Selected = null;

            var tracks = new List<Track>();

            if (files == null || files.Length == 0)
            {
                openFileDialog.FileName = string.Empty;

                openFileDialog.InitialDirectory = AppSettings.Local.Default.DirectoryLastTracks;

                if (openFileDialog.ShowDialog(this) != DialogResult.OK) return tracks;

                AppSettings.Local.Default.DirectoryLastTracks = Directory.GetParent(openFileDialog.FileName).FullName;

                files = openFileDialog.FileNames;
            }

            tracks = await InternalOpenTracksAsync(files);

            foreach (var track in overlayTracks.Routes)
            {
                track.IsVisible = miMainShowTracks.Checked;
            }

            return tracks;
        }

        private readonly WrapperCancellationTokenSource ctsTracksInfo = new WrapperCancellationTokenSource();

        private async Task LoadTracksInfoAsync()
        {
            DebugWrite.Line("start");

            ctsTracksInfo.Start();

            try
            {
                var tracksInfo = await Database.Default.LoadTracksInfoAsync(Database.Filter.Default);

                statusStripPresenter.TracksCount = tracksInfo.Count;
                statusStripPresenter.TracksDistance = tracksInfo.Distance / 1000.0;
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            finally
            {
                ctsTracksInfo.Finally();

                DebugWrite.Line("end");
            }
        }

        private readonly WrapperCancellationTokenSource ctsYears = new WrapperCancellationTokenSource();

        private async Task LoadYearsAsync()
        {
            DebugWrite.Line("start");

            Lists.Default.Years.Clear();

            ctsYears.Start();

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                var years = await Database.Default.LoadYearsAsync();

                Lists.Default.Years = years.ToList();

                DebugWrite.Line(string.Join(", ", Lists.Default.Years));
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
                ctsYears.Finally();

                ProgramStatus.Default.Stop(status);

                DebugWrite.Line("end");
            }
        }
    }
}