using P3tr0viCh.Utils;
using P3tr0viCh.Database;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Technics
{
    public static partial class Utils
    {
        public static class Log
        {
            private class InternalLog : DefaultInstance<P3tr0viCh.Utils.Log> { }

            public static string Directory
            {
                get => InternalLog.Default.Directory; set => InternalLog.Default.Directory = value;
            }

            public static void WriteProgramStart() => InternalLog.Default.WriteProgramStart();

            public static void WriteProgramStop() => InternalLog.Default.WriteProgramStop();

            public static void WriteFormOpen(Form frm) => InternalLog.Default.WriteFormOpen(frm);

            public static void WriteFormClose(Form frm) => InternalLog.Default.WriteFormClose(frm);

            public static void Info(string s, [CallerMemberName] string memberName = "")
            {
                s = s.ReplaceEol();

                DebugWrite.Line(s, memberName);

                InternalLog.Default.Write($"{memberName}: {s}");
            }

            public static void Error(Exception e, [CallerMemberName] string memberName = "")
            {
                if (e == null) return;

                Error(e.Message, memberName);

                Error(e.InnerException, memberName);
            }

            public static void Error(string err, [CallerMemberName] string memberName = "")
            {
                err = err.ReplaceEol();

                DebugWrite.Error(err, memberName);

                InternalLog.Default.Write($"{memberName} fail: {err}");
            }

            public static void Query(Exception e, [CallerMemberName] string memberName = "")
            {
                var query = e.GetQuery();

                if (query.IsEmpty()) return;

                Error($"query: {query.ReplaceEol()}", memberName);
            }
        }
    }
}