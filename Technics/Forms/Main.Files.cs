using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static P3tr0viCh.Utils.DataTableFile;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
        private enum FilesDialogType
        {
            Files,
            Directory
        }

        private async Task LoadFromFilesAsync(FilesDialogType type)
        {
            var files = Array.Empty<string>();

            switch (type)
            {
                case FilesDialogType.Files:
                    openFileDialog.FileName = string.Empty;

                    openFileDialog.DefaultExt = ".csv";
                    openFileDialog.Filter = Resources.OpenFileDialogFilterCsv;

                    if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                    files = openFileDialog.FileNames;

                    break;
                case FilesDialogType.Directory:
                    break;
                default: return;
            }

            if (files.Length == 0)
            {
                return;
            }

            await LoadFromFilesAsync(files);
        }

        private async Task LoadFromFilesAsync(string[] files)
        {
            DebugWrite.Line("start");

            var status = ProgramStatus.Start(Status.ReadFiles);

            var fileName = string.Empty;

            try
            {
                foreach (var file in files)
                {
                    fileName = file;

                    await MileagesLoadFromFileAsync(file);
                }
            }
            catch (Exception e)
            {
                Utils.Log.Error(e);

                var errorMsg = e.Message;

                if (e is CsvFileWrongHeaderException)
                {
                    errorMsg = Resources.ErrorCsvFileWrongHeader;
                }

                Utils.Msg.Error(Resources.MsgFileReadFail, fileName, errorMsg);
            }
            finally
            {
                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }
    }
}