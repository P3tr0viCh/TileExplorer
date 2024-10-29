using P3tr0viCh.Utils;
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

            var status = ProgramStatus.Start(Status.LoadData);

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

                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }

        private async Task TrackChangeAsync(Track track)
        {
            if (track == null) return;

            FrmTrack.ShowDlg(this, track);

            await SelectMapItemAsync(this, track);
        }

        public async Task TrackChangedAsync(Track track)
        {
            Utils.Forms.GetChildForms<FrmList>(ChildFormType.TileInfo | ChildFormType.TrackList).ForEach(frm =>
            {
                frm.ListItemChange(track);
            });

            await SelectMapItemAsync(this, track);
        }

        private async Task TrackDeleteAsync(Track track)
        {
            if (track == null) return;

            var name = track.Text;

            if (name.IsEmpty())
            {
                name = track.DateTimeStart.ToString();
            }

            if (!Msg.Question(string.Format(Resources.QuestionTrackDelete, name))) return;

            if (!await Database.Actions.TrackDeleteAsync(track)) return;

            Selected = null;

            overlayTracks.Routes.Remove(
                overlayTracks.Routes.Cast<IMapItem>().Where(i => i.Model.Id == track.Id)?
                    .Cast<MapItemTrack>().FirstOrDefault());

            await UpdateDataAsync(DataLoad.TrackList | DataLoad.TracksTree | DataLoad.Tiles | DataLoad.Summary);

            Utils.Forms.GetChildForms<Form>().ForEach(frm =>
            {
                switch (((IChildForm)frm).FormType)
                {
                    case ChildFormType.TileInfo:
                        if (((IListForm)frm).Count == 0)
                        {
                            frm.Close();
                        }

                        break;
                    case ChildFormType.ChartTrackEle:
                        if (((FrmChartTrackEle)frm).Track.Id == track.Id)
                        {
                            frm.Close();
                        }

                        break;
                }
            });
        }

        private async Task<bool> InternalOpenTracksAsync(string[] files)
        {
            var status = ProgramStatus.Start(Status.LoadGpx);

            Track track;

            try
            {
                /*                var showDlg = true;

                                var canToAll = files.Count() > 1;

                                Equipment equipmentToAll = null;

                                var exitForEach = false;*/

                foreach (var file in files)
                {
                    track = await Utils.Tracks.OpenTrackFromFileAsync(file);

                    DebugWrite.Line("OpenTrackFromFile done");

                    // await Task.Delay(1000);

                    /*                    if (showDlg)
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
                                            track.Equipment = equipmentToAll;*/

                    await Database.Default.TrackSaveNewAsync(track);

                    DebugWrite.Line("TrackSaveNewAsync done");

                    OverlayAddTrack(track);
                }

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(e.Message);

                return false;
            }
            finally
            {
                ProgramStatus.Stop(status);
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

            await UpdateDataAsync(DataLoad.TrackList | DataLoad.TracksTree | DataLoad.Tiles | DataLoad.Summary);
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

            var status = ProgramStatus.Start(Status.LoadData);

            try
            {
                Years = await Database.Default.LoadYearsAsync();

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

                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }
    }
}