using System.ComponentModel;
using static P3tr0viCh.Utils.Converters;

namespace Technics
{
    public static class Enums
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

        public enum LoadFilesType
        {
            FileDialog,
            FolderDialog,
            DirectoryTracks,
        }

        public enum FileExt
        {
            Csv,
            Gpx,
            Other
        }
    }
}