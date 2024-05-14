using GMap.NET;
using P3tr0viCh.Utils;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public static partial class Utils
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

        public static void WriteDebug(string s, [CallerMemberName] string memberName = "")
        {
            Debug.WriteLine(memberName + ": " + s);
        }

        public static void WriteError(Exception e, [CallerMemberName] string memberName = "")
        {
            if (e == null) return;

            WriteError(e.Message, memberName);

            WriteError(e.InnerException, memberName);
        }

        public static void WriteError(string err, [CallerMemberName] string memberName = "")
        {
            var error = string.Format("{0} fail: {1}", memberName, err);

            Debug.WriteLine(error);
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
                Process.Start(path);
            }
            catch (Exception e)
            {
                WriteError(e);

                Msg.Error(Resources.ErrorOpenPath, e.Message);
            }
        }
    }
}