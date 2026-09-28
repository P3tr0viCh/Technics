using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Interfaces;

namespace Technics
{
    public partial class Main
    {
        private bool AbnormalExit
        {
            get => Tag != null && (bool)Tag;
            set => Tag = value;
        }

        private bool SelfChange { get; set; } = false;

        private bool CreateProgramDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return false;
            }

            Directory.CreateDirectory(path);

            return true;
        }

        private bool SetDirectories()
        {
            try
            {
                var programDataDirectory =
#if DEBUG
                    Path.Combine(Files.ExecutableDirectory(), Files.ExecutableName());
#else
                    Files.AppDataLocalDirectory();
#endif
                var programDirectoryCreated = CreateProgramDirectory(programDataDirectory);

                AppSettings.Default.Directory = programDataDirectory;

                Utils.Log.Directory = programDataDirectory;

                Utils.Log.WriteProgramStart();

                Utils.Log.Info(Application.StartupPath, ResourcesLog.PathAppDirectory);

                if (programDirectoryCreated)
                {
                    Utils.Log.Info(ResourcesLog.PathDataDirectoryCreateOk, ResourcesLog.PathDataDirectory);
                }

                Utils.Log.Info(programDataDirectory, ResourcesLog.PathDataDirectory);

                Utils.Log.Info(Utils.Log.Directory, ResourcesLog.PathLogsDirectory);

                Utils.Log.Info(AppSettings.Default.FilePath, ResourcesLog.PathSettings);

                return true;
            }
            catch (Exception e)
            {
                Utils.Log.Error(e.Message);

                Utils.Msg.Error(Resources.MsgDirectoriesCreateFail, e.Message);

                WindowState = FormWindowState.Minimized;

                AbnormalExit = true;

                Application.Exit();

                return false;
            }
        }

        private bool SetDatabase()
        {
            try
            {
                var databaseDirectory = AppSettings.Settings.DirectoryDatabase;

                if (databaseDirectory.IsEmpty())
                {
                    databaseDirectory = AppSettings.Default.Directory;
                }

                try
                {
                    Files.CheckDirectoryExists(databaseDirectory);
                }
                catch (Exception e)
                {
                    Utils.Log.Error(e);

                    databaseDirectory = AppSettings.Default.Directory;

                    AppSettings.Settings.DirectoryDatabase = string.Empty;

                    Utils.Msg.Error(Resources.MsgDatabaseCreateFail, e.Message, databaseDirectory);
                }

                Database.Default.FileName = Path.Combine(databaseDirectory, Files.DatabaseFileName());

                Utils.Log.Info(Database.Default.FileName, ResourcesLog.PathDatabase);

                return true;
            }
            catch (Exception e)
            {
                Utils.Log.Error(e.Message);

                Utils.Msg.Error(Resources.MsgDirectoriesCreateFail, e.Message);

                WindowState = FormWindowState.Minimized;

                AbnormalExit = true;

                Application.Exit();

                return false;
            }
        }

        public void AppSettingsLoad()
        {
            if (AppSettings.Default.Load()) return;

            Utils.Log.Error(AppSettings.Default.LastError);
        }

        public void AppSettingsSave()
        {
            if (AppSettings.Default.Save()) return;

            Utils.Log.Error(AppSettings.Default.LastError);
        }

        private void SetTags()
        {
            miListParts.Tag = FrmListType.Parts;
            tsbtnListParts.Tag = FrmListType.Parts;

            miListTechs.Tag = FrmListType.Techs;
            tsbtnListTechs.Tag = FrmListType.Techs;

            miListMaintenance.Tag = FrmListType.Maintenance;
            tsbtnListMaintenance.Tag = FrmListType.Maintenance;
        }

        private void CancelTokens()
        {
            ctsTechsLoad.Cancel();

            ctsMileagesLoad.Cancel();

            ctsTechPartsLoad.Cancel();

            ctsMaintenanceLoad.Cancel();

            ctsCheckDirectoryTracks.Cancel();
        }

        private T ListFindByDateTime<T>(IEnumerable<T> source, DateTime dateTime) where T : IDateTime
        {
            if (source == null || source.IsEmpty() || dateTime == default) return default;

            return source.OrderBy(m => Math.Abs((m.DateTime - dateTime).Ticks)).FirstOrDefault();
        }

        private int CheckValue(int value, int maxSize, int minSize, int defValue)
        {
            return value > maxSize || value < minSize ? defValue : value;
        }
    }
}