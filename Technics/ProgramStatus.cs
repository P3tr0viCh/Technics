using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Converters;
using System.ComponentModel;
using static Technics.ProgramStatus;

namespace Technics
{
    public class ProgramStatus : DefaultInstance<ProgramStatus<Status>>
    {
        [TypeConverter(typeof(EnumDescriptionConverter))]
        public enum Status
        {
            [Description("")]
            Idle,
            [Description("Загрузка данных...")]
            LoadData,
            [Description("Сохранение данных...")]
            SaveDatа,
            [Description("Чтение файлов...")]
            ReadFiles,
            [Description("Поиск файлов gpx...")]
            CheckDirectoryTracks,
        }
    }
}