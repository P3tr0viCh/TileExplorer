using GMap.NET;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;
using static TileExplorer.Interfaces;

namespace TileExplorer
{
    internal static partial class Utils
    {
        public static PointLatLng TrackPointToPointLatLng(TrackPoint trackPointModel)
        {
            return new PointLatLng(trackPointModel.Lat, trackPointModel.Lng);
        }

        public static string AssemblyNameAndVersion()
        {
            var assemblyDecorator = new Misc.AssemblyDecorator();

            return string.Format("{0}/{1}", assemblyDecorator.Assembly.GetName().Name, assemblyDecorator.VersionString());
        }

        public static void ComboBoxInsertItem(BindingSource bindingSource, int index, BaseId value)
        {
            bindingSource.Insert(index, value);
        }

        public static void ComboBoxInsertItem(ComboBox comboBox, int index, BaseId value)
        {
            ComboBoxInsertItem((BindingSource)comboBox.DataSource, index, value);
        }

        public static void OpenPath(string path)
        {
            try
            {
                DebugWrite.Line(path);

                Process.Start(path);
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Msg.Error(Resources.ErrorOpenPath, e.Message);
            }
        }

        public static void DirectoryCreate(string path)
        {
            DebugWrite.Line($"{path}");

            Directory.CreateDirectory(path);
        }

        public static void FileCopy(string sourceFileName, string destFileName)
        {
            DebugWrite.Line($"{sourceFileName} > {destFileName}");

            File.Copy(sourceFileName, destFileName, true);
        }

        public static double LinearInterpolate(double x, double x1, double y1, double x2, double y2)
        {
            var a = (y2 - y1) / (x2 - x1);
            
            var b = y1 - a * x1;
            
            return a * x + b;
        }

        public static ICollection<T> GetChildForms<T>(ChildFormType? type) 
        {
            var forms = new List<T>();

            foreach (var frm in Application.OpenForms)
            {
                if (frm is IChildForm childFrm && frm is T childFrmT &&
                    (type == null || childFrm.FormType == type))
                {
                    forms.Add(childFrmT);
                }
            }

            return forms;
        }

        public static FrmList GetFrmList(ChildFormType type)
        {
            return GetChildForms<FrmList>(type).FirstOrDefault();
        }
    }
}