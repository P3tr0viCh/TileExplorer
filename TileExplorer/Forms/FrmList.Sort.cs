using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class FrmList
    {
        private string sortColumn = string.Empty;
        private int sortColumnIndex = -1;
        private bool sortOrderDescending = true;

        private void Sort()
        {
            var selected = Selected;

            object list;

            switch (FormType)
            {
                case ChildFormType.TrackList:
                    list = bindingSource.Cast<Track>().ToList();

                    ((List<Track>)list).Sort((x, y) =>
                    {
                        int compare = 0;

                        var descending = sortOrderDescending ? -1 : 1;

                        switch (sortColumn)
                        {
                            case nameof(Track.Text):
                                compare = x.Text.CompareTo(y.Text) * descending;
                                break;
                            case nameof(Track.DateTimeStart):
                                compare = x.DateTimeStart.CompareTo(y.DateTimeStart) * descending;
                                break;
                            case nameof(Track.DateTimeFinish):
                                compare = x.DateTimeFinish.CompareTo(y.DateTimeFinish) * descending;
                                break;
                            case nameof(Track.Duration):
                                compare = x.Duration.CompareTo(y.Duration) * descending;
                                break;
                            case nameof(Track.DurationInMove):
                                compare = x.DurationInMove.CompareTo(y.DurationInMove) * descending;
                                break;
                            case nameof(Track.Distance):
                                compare = x.Distance.CompareTo(y.Distance) * descending;
                                break;
                            case nameof(Track.AverageSpeed):
                                compare = x.AverageSpeed.CompareTo(y.AverageSpeed) * descending;
                                break;
                            case nameof(Track.EleAscent):
                                compare = x.EleAscent.CompareTo(y.EleAscent) * descending;
                                break;
                            case nameof(Track.EleDescent):
                                compare = x.EleDescent.CompareTo(y.EleDescent) * descending;
                                break;
                            case nameof(Track.NewTilesCount):
                                compare = x.NewTilesCount.CompareTo(y.NewTilesCount) * descending;
                                break;
                            case nameof(Track.EquipmentText):
                                compare = Utils.CompareTo(x.EquipmentText, y.EquipmentText, sortOrderDescending);
                                break;
                            case nameof(Track.TagsAsString):
                                compare = Utils.CompareTo(x.TagsAsString, y.TagsAsString, sortOrderDescending);
                                break;
                            default:
                                DebugWrite.Line(sortColumn);
                                break;
                        }

                        if (sortColumn != nameof(Track.DateTimeStart))
                        {
                            if (compare == 0)
                            {
                                compare = y.DateTimeStart.CompareTo(x.DateTimeStart);
                            }
                        }

                        return compare;
                    });

                    break;
                case ChildFormType.MarkerList:
                    list = bindingSource.Cast<Marker>().ToList();

                    ((List<Marker>)list).Sort((Marker x, Marker y) => x.Text.CompareTo(y.Text));

                    break;
                case ChildFormType.TagList:
                    list = bindingSource.Cast<TagModel>().ToList();

                    ((List<TagModel>)list).Sort((x, y) => x.Text.CompareTo(y.Text));

                    break;
                case ChildFormType.EquipmentList:
                    list = bindingSource.Cast<Equipment>().ToList();

                    ((List<Equipment>)list).Sort((x, y) => x.Text.CompareTo(y.Text));

                    break;
                default:
                    return;
            }

            bindingSource.DataSource = list;

            Selected = selected;
        }
    }
}