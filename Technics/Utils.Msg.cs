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
                Log.Info(text.SingleLine(), "Msg.Info");

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
                Log.Info(text.SingleLine(), "Msg.Question");

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

            public static void Error(string text = "Error!")
            {
                Log.Info(text.SingleLine(), "Msg.Error");

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

            public static bool Question<T>(IEnumerable<T> list)
            {
                var count = list?.Count();

                if (count == 0) return false;

                var firstItem = list.FirstOrDefault();

                var question = string.Empty;

                if (firstItem is IBaseText itemText)
                {
                    question = count == 1 ?
                        string.Format(Resources.QuestionItemLinkedDelete, itemText.Text) :
                        string.Format(Resources.QuestionItemListLinkedDelete, itemText.Text, count - 1);
                }
                else
                {
                    if (firstItem is MileageModel mileage)
                    {
                        var dt = mileage.DateTime.ToString(AppSettings.Default.FormatDateTime);

                        question = count == 1 ?
                            string.Format(Resources.QuestionMileageDelete, dt) :
                            string.Format(Resources.QuestionMileageListDelete, dt, count - 1);
                    }
                    else
                    {
                        if (firstItem is TechPartModel techPart)
                        {
                            var dt = techPart.DateTimeInstall.ToString(AppSettings.Default.FormatDateTime);

                            question = count == 1 ?
                                string.Format(Resources.QuestionTechPartDelete, techPart.PartText, dt) :
                                string.Format(Resources.QuestionTechPartListDelete, techPart.PartText, dt, count - 1);
                        }
                    }
                }

                return Question(question);
            }
        }
    }
}