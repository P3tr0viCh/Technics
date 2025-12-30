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
        private async Task<IEnumerable<MileageModel>> LoadFromFileAsync(string file)
        {
            try
            {
                var loaded = await MileagesLoadFromFileAsync(file);

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

                await Database.Default.MileageSaveAsync(mileages);

                return mileages.Count > 0;
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

            switch (type)
            {
                case FilesDialogType.Files:
                    openFileDialog.FileName = string.Empty;

                    openFileDialog.InitialDirectory = AppSettings.Default.DirectoryLastMileages;

                    openFileDialog.Filter = Resources.FilterOpenFileDialogMileages;

                    if (openFileDialog.ShowDialog() != DialogResult.OK) return;

                    files = openFileDialog.FileNames;

                    break;
                case FilesDialogType.Directory:
                    folderBrowserDialog.SelectedPath = AppSettings.Default.DirectoryLastMileages;

                    if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

                    files = await Utils.GetFilesAsync(folderBrowserDialog.SelectedPath, Resources.FilterFolderBrowserDialogMileages);

                    foreach (var file in files)
                    {
                        DebugWrite.Line(file);
                    }

                    break;
                default: return;
            }

            if (files.Length == 0) return;

            AppSettings.Default.DirectoryLastMileages = Directory.GetParent(files[0]).FullName;

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