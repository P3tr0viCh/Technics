#if DEBUG
#define _SHOW_FILES
#endif

using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
        internal class GpxFiles : SettingsBase<GpxFiles>
        {
            public IEnumerable<string> Files { get; set; } = Enumerable.Empty<string>();
        }

        private readonly WrapperCancellationTokenSource ctsCheckDirectoryTracks = new WrapperCancellationTokenSource();

        private async Task<IEnumerable<string>> FindFilesAsync(string directory)
        {
            DebugWrite.Line("start");

            try
            {
                var files = await Utils.EnumerateFilesAsync(directory, ".gpx");

#if SHOW_FILES
                foreach (var file in files)
                {
                    if (ctsCheckDirectoryTracks.IsCancellationRequested) break;

                    DebugWrite.Line(file);
                }
#endif

                return files;
            }
            finally
            {
                DebugWrite.Line("done");
            }
        }

        private IEnumerable<string> ExceptNewFiles(IEnumerable<string> files)
        {
            GpxFiles.Directory = Path.GetDirectoryName(Database.Default.FileName);

            GpxFiles.FileName = $"{Files.ExecutableName()}.GpxFiles.{Files.ExtConfig}";

            DebugWrite.Line($"GpxFiles: {GpxFiles.FilePath}");

            GpxFiles.Default.Load();

            var newFiles = files.Except(GpxFiles.Default.Files, new PathComparer());

            GpxFiles.Default.Files = files;

            GpxFiles.Default.Save();

#if SHOW_FILES
            DebugWrite.Line($"new files count: {newFiles.Count()}");

            foreach (var file in newFiles)
            {
                if (ctsCheckDirectoryTracks.IsCancellationRequested) break;

                DebugWrite.Line(file);
            }
#endif

            return newFiles;
        }

        private async Task<IEnumerable<string>> ExceptNewFilesAsync(IEnumerable<string> files)
        {
            DebugWrite.Line("start");

            try
            {
                return await Task.Factory.StartNew(() =>
                {
                    return ExceptNewFiles(files);
                }, ctsCheckDirectoryTracks.Token);
            }
            finally
            {
                DebugWrite.Line("done");
            }
        }

        private async Task<IEnumerable<string>> GetNewFilesAsync(string directory)
        {
            var status = ProgramStatus.Default.Start(Status.CheckDirectoryTracks);

            try
            {
                var files = await FindFilesAsync(directory);

                if (ctsCheckDirectoryTracks.IsCancellationRequested) return Enumerable.Empty<string>();

                var newFiles = await ExceptNewFilesAsync(files);

                Utils.Log.Info(string.Format(ResourcesLog.CheckDirectoryTracksOk,
                    files.Count(), newFiles.Count()));

                return newFiles;
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }
        }

        private bool QuestionAddTracks(IEnumerable<string> files)
        {
            string question;

            var count = files.Count();

            if (count == 1)
            {
                question = string.Format(Resources.QuestionNewTrackFinded1, Path.GetFileName(files.First()));
            }
            else
            {
                var maxShow = 5;

                var questionFiles = string.Join(Str.Eol, files.Take(maxShow).Select(f => Path.GetFileName(f)));

                if (count <= maxShow)
                {
                    question = string.Format(Resources.QuestionNewTrackFinded2, questionFiles);
                }
                else
                {
                    question = string.Format(Resources.QuestionNewTrackFinded3, questionFiles, count - maxShow);
                }
            }

            return Utils.Msg.Question(question);
        }

        private async Task<IEnumerable<string>> CheckDirectoryTracksAsync()
        {
            var empty = Enumerable.Empty<string>();

            var directoryTracks = AppSettings.Default.DirectoryTracks;

            if (directoryTracks.IsEmpty())
            {
                Utils.Log.Info(ResourcesLog.CheckDirectoryTracksPathEmpty);
                
                return empty;
            }

            if (!Directory.Exists(directoryTracks))
            {
                Utils.Log.Info(ResourcesLog.CheckDirectoryTracksNotExists);
                
                return empty;
            }

            DebugWrite.Line("start");

            ctsCheckDirectoryTracks.Start();

            try
            {
                var files = await GetNewFilesAsync(directoryTracks);

                if (ctsCheckDirectoryTracks.IsCancellationRequested) return Enumerable.Empty<string>();

                if (!files.Any()) return Enumerable.Empty<string>();

                if (!QuestionAddTracks(files))
                {
                    Utils.Log.Info(ResourcesLog.CheckDirectoryTracksCancel);

                    return empty;
                }

                return files;
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);

                return empty;
            }
            catch (Exception e)
            {
                Utils.Log.Error(e);

                return empty;
            }
            finally
            {
                ctsCheckDirectoryTracks.Finally();

                DebugWrite.Line("end");
            }
        }
    }
}