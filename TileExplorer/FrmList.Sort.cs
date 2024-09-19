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

                    ((List<Track>)list).Sort((Track x, Track y) =>
                    {
                        int compare = 0;

                        switch (sortColumn)
                        {
                            case nameof(Track.Text):
                                compare = x.Text.CompareTo(y.Text);
                                break;
                            case nameof(Track.DateTimeStart):
                                compare = x.DateTimeStart.CompareTo(y.DateTimeStart);
                                break;
                            case nameof(Track.DateTimeFinish):
                                compare = x.DateTimeFinish.CompareTo(y.DateTimeFinish);
                                break;
                            case nameof(Track.Duration):
                                compare = x.Duration.CompareTo(y.Duration);
                                break;
                            case nameof(Track.DurationInMove):
                                compare = x.DurationInMove.CompareTo(y.DurationInMove);
                                break;
                            case nameof(Track.Distance):
                                compare = x.Distance.CompareTo(y.Distance);
                                break;
                            case nameof(Track.AverageSpeed):
                                compare = x.AverageSpeed.CompareTo(y.AverageSpeed);
                                break;
                            case nameof(Track.EleAscent):
                                compare = x.EleAscent.CompareTo(y.EleAscent);
                                break;
                            case nameof(Track.NewTilesCount):
                                compare = x.NewTilesCount.CompareTo(y.NewTilesCount);
                                break;
                            case nameof(Track.EquipmentName):
                                if (x.EquipmentName.IsEmpty() && y.EquipmentName.IsEmpty())
                                {
                                    compare = x.DateTimeStart.CompareTo(y.DateTimeStart); ;
                                }
                                else
                                {
                                    if (x.EquipmentName.IsEmpty())
                                    {
                                        if (sortOrderDescending)
                                        {
                                            compare = -1;
                                        }
                                        else
                                        {
                                            compare = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (y.EquipmentName.IsEmpty())
                                        {
                                            if (sortOrderDescending)
                                            {
                                                compare = 1;
                                            }
                                            else
                                            {
                                                compare = -1;
                                            }
                                        }
                                        else
                                        {
                                            compare = x.EquipmentName.CompareTo(y.EquipmentName);

                                            if (compare == 0)
                                            {
                                                compare = x.DateTimeStart.CompareTo(y.DateTimeStart);
                                            }
                                        }
                                    }
                                }
                                break;
                        }

                        if (sortOrderDescending)
                        {
                            compare = -compare;
                        }

                        return compare;
                    });

                    break;
                case ChildFormType.MarkerList:
                    list = bindingSource.Cast<Marker>().ToList();

                    ((List<Marker>)list).Sort((Marker x, Marker y) => x.Text.CompareTo(y.Text));

                    break;
                case ChildFormType.EquipmentList:
                    list = bindingSource.Cast<Equipment>().ToList();

                    ((List<Equipment>)list).Sort((Equipment x, Equipment y) => x.Name.CompareTo(y.Name));

                    break;
                default:
                    return;
            }

            bindingSource.DataSource = list;

            Selected = selected;
        }
    }
}