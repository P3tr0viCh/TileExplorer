using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;
using static TileExplorer.ProgramStatus;

namespace TileExplorer
{
    public partial class Main
    {
        private bool tracksLoaded = false;

        public List<int> Years { get; private set; }

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

        private async Task TrackChangeAsync(IEnumerable<Track> tracks)
        {
            var count = tracks?.Count();

            if (count == 0) return;

            var track = tracks.FirstOrDefault();

            if (count == 1)
            {
                if (!FrmTrack.ShowDlg(this, track)) return;

                await SelectMapItemAsync(this, track);
            }
            else
            {
                if (!FrmTrackList.ShowDlg(this, tracks)) return;

                await UpdateTracksAsync(tracks);
            }
        }

        private async Task TrackChangeAsync(Track track)
        {
            await TrackChangeAsync(new List<Track>() { track });
        }

        public async Task TrackChangedAsync(Track track)
        {
            await UpdateDataAsync(DataLoad.ObjectChange, track);

            await SelectMapItemAsync(this, track);
        }

        public async Task TrackChangedAsync(IEnumerable<Track> tracks)
        {
            await UpdateDataAsync(DataLoad.Tracks);

            SelectTrackList(tracks);
        }

        private async Task TrackDeleteAsync(List<Track> tracks)
        {
            if (tracks?.Count == 0) return;

            var firstTrack = tracks.FirstOrDefault();

            var name = firstTrack.Text;

            if (name.IsEmpty())
            {
                name = firstTrack.DateTimeStart.ToString();
            }

            var question = tracks.Count == 1 ? Resources.QuestionTrackDelete : Resources.QuestionTrackListDelete;

            if (!Msg.Question(question, name, tracks.Count - 1)) return;

            if (!await Database.Actions.TrackDeleteAsync(tracks)) return;

            Selected = null;

            foreach (var track in tracks)
            {
                overlayTracks.Routes.Remove(
                overlayTracks.Routes.Cast<IMapItem>().Where(i => i.Model.Id == track.Id)?
                    .Cast<MapItemTrack>().FirstOrDefault());

                await UpdateDataAsync(DataLoad.Tiles | DataLoad.ObjectDelete, track);
            }
        }

        private async Task TrackDeleteAsync(Track track)
        {
            await TrackDeleteAsync(new List<Track>() { track });
        }

        private async Task<bool> InternalOpenTracksAsync(string[] files)
        {
            var status = ProgramStatus.Default.Start(Status.LoadGpx);

            Track track;

            var loadedCount = 0;

            try
            {
                var showDlg = true;

                var canToAll = files.Length > 1;

                Equipment equipmentToAll = null;

                foreach (var file in files)
                {
                    track = await Utils.Tracks.OpenTrackFromFileAsync(file);

                    DebugWrite.Line("OpenTrackFromFile done");

                    // await Task.Delay(1000);

                    if (showDlg)
                    {
                        switch (FrmTrack.ShowDlg(this, track, canToAll, false))
                        {
                            case DialogResult.Cancel:
                                continue;
                            case DialogResult.Abort:
                                return loadedCount > 0;
                            case DialogResult.Yes:
                                showDlg = false;

                                equipmentToAll = track.Equipment;

                                break;
                        }
                    }
                    else
                    {
                        track.Equipment = equipmentToAll;
                    }

                    await Database.Default.TrackSaveNewAsync(track);

                    DebugWrite.Line("TrackSaveNewAsync done");

                    OverlayAddTrack(track);

                    await UpdateDataAsync(DataLoad.ObjectChange, track);

                    loadedCount++;
                }

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);

                return loadedCount > 0;
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }
        }

        private async Task OpenTracksAsync(string[] files)
        {
            Selected = null;

            if (files == null || files.Length == 0)
            {
                openFileDialog.FileName = string.Empty;

                openFileDialog.InitialDirectory = AppSettings.Local.Default.DirectoryLastTracks;

                if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

                AppSettings.Local.Default.DirectoryLastTracks = Directory.GetParent(openFileDialog.FileName).FullName;

                files = openFileDialog.FileNames;
            }

            if (!await InternalOpenTracksAsync(files)) return;

            foreach (var track in overlayTracks.Routes)
            {
                track.IsVisible = miMainShowTracks.Checked;
            }

            await UpdateDataAsync(DataLoad.Tiles);
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

            Years?.Clear();

            ctsYears.Start();

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                var years = await Database.Default.LoadYearsAsync();

                Years = years.ToList();

                DebugWrite.Line(string.Join(", ", Years));
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

        private async Task UpdateTracksAsync(IEnumerable<Track> tracks)
        {
            try
            {
                var status = ProgramStatus.Default.Start(Status.SaveData);

                try
                {
                    foreach (var track in tracks)
                    {
                        await Database.Default.TrackSaveAsync(track);
                    }
                }
                finally
                {
                    ProgramStatus.Default.Stop(status);
                }

                await TrackChangedAsync(tracks);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);
            }
        }
    }
}