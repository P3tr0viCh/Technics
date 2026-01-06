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

        private async Task<bool> LoadFromFilesAsync(string[] files)
        {
            DebugWrite.Line("start");

            var status = ProgramStatus.Start(Status.ReadFiles);

            try
            {
                var mileages = new List<MileageModel>();

                foreach (var file in files)
                {
                    var loaded = await LoadFromFileAsync(file);

                    mileages.AddRange(loaded);
                }

                if (mileages.Count == 0) return false;

                await Database.Default.MileageSaveAsync(mileages);

                return true;
            }
            finally
            {
                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }

        private enum FilesDialogType
        {
            Files,
            Directory
        }

        private async Task LoadFromFilesAsync(FilesDialogType type)
        {
            string[] files;

            string directoryLastMileages;

            try
            {
                switch (type)
                {
                    case FilesDialogType.Files:
                        openFileDialog.FileName = string.Empty;

                        openFileDialog.InitialDirectory = AppSettings.Default.DirectoryLastMileages;

                        openFileDialog.Filter = Resources.FilterOpenFileDialogMileages;

                        if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                        files = openFileDialog.FileNames;

                        directoryLastMileages = Directory.GetParent(files[0]).FullName;

                        break;
                    case FilesDialogType.Directory:
                        folderBrowserDialog.SelectedPath = AppSettings.Default.DirectoryLastMileages;

                        if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

                        directoryLastMileages = folderBrowserDialog.SelectedPath;

                        files = await Utils.EnumerateFilesAsync(folderBrowserDialog.SelectedPath, Resources.FilterFolderBrowserDialogMileages);

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

            AppSettings.Default.DirectoryLastMileages = directoryLastMileages;

            Utils.Log.Info(string.Format(ResourcesLog.OpenFiles, directoryLastMileages, files.Length));

            if (files.Length == 0) return;

            var result = await LoadFromFilesAsync(files);

            if (result)
            {
                if (tvTechs.Nodes[0].IsSelected)
                {
                    await UpdateDataAsync(DataLoad.Mileages | DataLoad.TechParts);
                }

                tvTechs.SelectedNode = tvTechs.Nodes[0];
            }
        }
    }
}