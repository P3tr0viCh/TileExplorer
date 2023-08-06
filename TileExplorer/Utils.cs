using GMap.NET;
using P3tr0viCh.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static TileExplorer.Database.Models;

namespace TileExplorer
{
    public static partial class Utils
    {
        public static PointLatLng TrackPointToPointLatLng(TrackPoint trackPointModel)
        {
            return new PointLatLng(trackPointModel.Lat, trackPointModel.Lng);
        }

        public static DateTime DateTimeParse(string str, DateTime def = default)
        {
            return DateTime.TryParseExact(str, Const.DATETIME_FORMAT_GPX, null, DateTimeStyles.AssumeLocal, out DateTime result) ?
                result : def;
        }

        public static double DoubleParse(string str, double def = 0.0)
        {
            return double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double result) ?
                result : def;
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

        public static bool IsChildFormExists(Form form)
        {
            return form != null;
        }
    }
}