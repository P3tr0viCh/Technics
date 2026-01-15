using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static P3tr0viCh.Utils.DataTableFile;
using static Technics.Database.Models;
using static Technics.Enums;
using static Technics.ProgramStatus;

namespace Technics
{
    public partial class Main
    {
        private FileExt GetFileExt(string file)
        {
            var ext = Path.GetExtension(file);

            switch (ext)
            {
                case ".csv": return FileExt.Csv;
                case ".gpx": return FileExt.Gpx;
                default: return FileExt.Other;
            }
        }

        private async Task<IEnumerable<MileageModel>> LoadFromFileAsync(string file)
        {
            try
            {
                IEnumerable<MileageModel> loaded;

                var ext = GetFileExt(file);

                switch (ext)
                {
                    case FileExt.Csv:
                        loaded = await MileagesLoadFromFileCsvAsync(file);

                        break;
                    case FileExt.Gpx:
                        var mileage = await MileagesLoadFromFileGpxAsync(file);

                        loaded = new List<MileageModel>() { mileage };

                        break;
                    default:
                        return Enumerable.Empty<MileageModel>();
                }

                Utils.Log.Info(string.Format(ResourcesLog.LoadFromFileOk, file, loaded.Count()));

                return loaded;
            }
            catch (Exception e)
            {
                string errorMsg;

                if (e is CsvFileWrongHeaderException)
                {
                    errorMsg = string.Format(ResourcesLog.LoadFromFileFailWrongHeader, file);
                }
                else
                {
                    errorMsg = string.Format(ResourcesLog.LoadFromFileFail, e.Message, file);
                }

                Utils.Log.Error(errorMsg);

                return Enumerable.Empty<MileageModel>();
            }
        }

        private async Task<IEnumerable<MileageModel>> LoadFromFilesAsync(IEnumerable<string> files)
        {
            DebugWrite.Line("start");

            var status = ProgramStatus.Default.Start(Status.ReadFiles);

            try
            {
                var mileages = new List<MileageModel>();

                foreach (var file in files)
                {
                    var loaded = await LoadFromFileAsync(file);

                    mileages.AddRange(loaded);
                }

                if (mileages.Count == 0) return Enumerable.Empty<MileageModel>();

                await Database.Default.MileageSaveAsync(mileages);

                return mileages;
            }
            finally
            {
                ProgramStatus.Default.Stop(status);

                DebugWrite.Line("end");
            }
        }

        private async Task LoadFromFilesAsync(LoadFilesType type)
        {
            IEnumerable<string> files;

            var directoryLastMileages = string.Empty;

            try
            {
                switch (type)
                {
                    case LoadFilesType.FileDialog:
                        openFileDialog.FileName = string.Empty;

                        openFileDialog.InitialDirectory = AppSettings.Default.DirectoryLastMileages;

                        openFileDialog.Filter = Resources.FilterOpenFileDialogMileages;

                        if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                        files = openFileDialog.FileNames;

                        directoryLastMileages = Directory.GetParent(files.First()).FullName;

                        break;
                    case LoadFilesType.FolderDialog:
                        folderBrowserDialog.SelectedPath = AppSettings.Default.DirectoryLastMileages;

                        if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

                        directoryLastMileages = folderBrowserDialog.SelectedPath;

                        files = await Utils.EnumerateFilesAsync(folderBrowserDialog.SelectedPath,
                            Resources.FilterFolderBrowserDialogMileages);

                        break;
                    case LoadFilesType.DirectoryTracks:
                        files = await CheckDirectoryTracksAsync();

                        break;
                    default: return;
                }
            }
            catch (Exception e)
            {
                Utils.Log.Error(e);

                Utils.Msg.Error(e.Message);

                return;
            }

            var count = files.Count();

            switch (type)
            {
                case LoadFilesType.FileDialog:
                case LoadFilesType.FolderDialog:
                    AppSettings.Default.DirectoryLastMileages = directoryLastMileages;

                    Utils.Log.Info(string.Format(ResourcesLog.OpenFiles, directoryLastMileages, count));

                    break;
            }

            if (count == 0) return;

            var mileages = await LoadFromFilesAsync(files);

            await SelectMileagesAsync(mileages);
        }
    }
}