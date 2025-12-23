using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public static partial class Utils
    {
        public static class Msg
        {
            public static void Info(string text = "Hello, world!")
            {
                Log.Info(text.ReplaceEol(), "Msg.Info");

                P3tr0viCh.Utils.Msg.Info(text);
            }

            public static void Info(string format, object arg0)
            {
                Info(string.Format(format, arg0));
            }

            public static void Info(string format, object arg0, object arg1)
            {
                Info(string.Format(format, arg0, arg1));
            }

            public static bool Question(string text = "To be or not to be?")
            {
                Log.Info(text.ReplaceEol(), "Msg.Question");

                var result = P3tr0viCh.Utils.Msg.Question(text);

                Log.Info(result ? "yes" : "no", "Msg.Question");

                return result;
            }

            public static bool Question(string format, object arg0)
            {
                return Question(string.Format(format, arg0));
            }

            public static bool Question(string format, object arg0, object arg1)
            {
                return Question(string.Format(format, arg0, arg1));
            }

            public static bool Question(IEnumerable<IBaseText> list)
            {
                var count = list?.Count();

                if (count == 0) return false;

                var firstItem = list.FirstOrDefault();

                var text = firstItem.Text;

                var question = count == 1 ? Resources.QuestionItemLinkedDelete : Resources.QuestionItemListLinkedDelete;

                return Question(question, text, count - 1);
            }

            public static void Error(string text = "Error!")
            {
                Log.Info(text.ReplaceEol(), "Msg.Error");

                P3tr0viCh.Utils.Msg.Error(text);
            }

            public static void Error(string format, object arg0)
            {
                Error(string.Format(format, arg0));
            }

            public static void Error(string format, object arg0, object arg1)
            {
                Error(string.Format(format, arg0, arg1));
            }

            public static void Result(bool result, string message, string logMessage)
            {
                if (result)
                {
                    Log.Info(logMessage);
                    Info(message);
                }
                else
                {
                    Log.Error(logMessage);
                    Error(message);
                }
            }

            public static void Result(bool result, string message)
            {
                if (result)
                {
                    Info(message);
                }
                else
                {
                    Error(message);
                }
            }
        }
    }
}