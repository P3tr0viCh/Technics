#if DEBUG
#define _DUMMY_DATABASE
#endif

using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Enums;
using static Technics.PresenterStatusStripMain;

namespace Technics
{
    public partial class Main : Form, IMainForm, PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
    {
        private readonly PresenterStatusStripMain presenterStatusStrip;

        public ProgramStatus ProgramStatus { get; } = new ProgramStatus();

        public Main()
        {
            InitializeComponent();

            presenterStatusStrip = new PresenterStatusStripMain(this);

            presenterDataGridViewMileages = new PresenterDataGridViewMileages(dgvMileages);
            presenterDataGridViewTechParts = new PresenterDataGridViewTechParts(dgvTechParts);
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            Text = string.Format(Resources.TitleMain, new AssemblyDecorator().VersionString(false));

            splitterMileages.Cursor = Cursors.SizeNS;
            splitterTechs.Cursor = Cursors.SizeWE;

            if (!SetDirectories()) return;

            AppSettingsLoad();

            if (!SetDatabase()) return;

            SetTags();

            AddTechsRoot();

            ProgramStatus.StatusChanged += ProgramStatus_StatusChanged;

            AppSettings.LoadFormState(this, AppSettings.Default.FormStateMain);

            AppSettings.LoadDataGridColumns(dgvMileages, AppSettings.Default.ColumnsMileages);
            AppSettings.LoadDataGridColumns(dgvTechParts, AppSettings.Default.ColumnsTechParts);

            tvTechs.Width = AppSettings.Default.PanelTechsWidth;
            panelBottom.Height = AppSettings.Default.PanelBottomHeight;

            ToolStripsShowText = AppSettings.Default.ToolStripsShowText;

            presenterDataGridViewMileages.SortColumn = MileagesDateTime.Name;
            presenterDataGridViewMileages.SortOrderDescending = true;

            presenterDataGridViewTechParts.SortColumn = TechPartsDateTimeInstall.Name;
            presenterDataGridViewTechParts.SortOrderDescending = true;

            UpdateSettings();

#if DUMMY_DATABASE
            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                await new Database.Dummy().FillTables();
            }
            finally
            {
                ProgramStatus.Stop(status);
            }
#endif

            await UpdateDataAsync(DataLoad.Techs);
        }

        private void ProgramStatus_StatusChanged(object sender, Status status)
        {
            presenterStatusStrip.Status = status;

            UseWaitCursor = !ProgramStatus.IsIdle;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AbnormalExit) return;

            switch (e.CloseReason)
            {
                case CloseReason.WindowsShutDown:
                    Utils.Log.Info(ResourcesLog.CloseReasonShutDown);
                    break;
                case CloseReason.TaskManagerClosing:
                    Utils.Log.Info(ResourcesLog.CloseReasonTaskManager);
                    break;
                case CloseReason.ApplicationExitCall:
                    Utils.Log.Info(ResourcesLog.CloseReasonApplicationExitCall);
                    break;
                default:
                    break;
            }

            AppSettings.Default.FormStateMain = AppSettings.SaveFormState(this);

            AppSettings.Default.ColumnsMileages = AppSettings.SaveDataGridColumns(dgvMileages);
            AppSettings.Default.ColumnsTechParts = AppSettings.SaveDataGridColumns(dgvTechParts);

            AppSettings.Default.PanelTechsWidth = tvTechs.Width;
            AppSettings.Default.PanelBottomHeight = panelBottom.Height;

            AppSettings.Default.ToolStripsShowText = miViewToolStripsShowText.Checked;

            AppSettingsSave();

            Utils.Log.WriteProgramStop();
        }

        private void TsbtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MiFileClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowAbout()
        {
            FrmAbout.Show(14);
        }

        private void MiHelpAbout_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }

        ToolStripStatusLabel PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip.GetLabel(StatusLabel label)
        {
            switch (label)
            {
                case StatusLabel.Status: return LabelStatus;
                case StatusLabel.MileageCount: return LabelMileageCount;
                case StatusLabel.TechPartCount: return LabelTechPartCount;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void TvTechs_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvTechs.SelectedNode = e.Node;
            }
        }

        private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            Utils.SelectCellOnCellMouseDown((DataGridView)sender, e);
        }

        private async void TvTechs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SelfChange) return;

            await TechsSelectedChangedAsync();
        }

        private async void MiTechAddFolder_Click(object sender, EventArgs e)
        {
            await TechsAddNewFolderAsync();
        }

        private async void MiTechAddItem_Click(object sender, EventArgs e)
        {
            await TechsAddNewTechAsync();
        }

        private async void TsbtnTechChange_Click(object sender, EventArgs e)
        {
            await TechsChangeSelectedAsync();
        }

        private async void TvTechs_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is TreeNodeTech)
            {
                await TechsChangeSelectedAsync();
            }
        }

        private async void TsbtnTechDelete_Click(object sender, EventArgs e)
        {
            await TechsDeleteSelectedAsync();
        }

        private async void MiTechAddFolder2_Click(object sender, EventArgs e)
        {
            await TechsAddNewFolderAsync();
        }

        private async void MiTechAddTech2_Click(object sender, EventArgs e)
        {
            await TechsAddNewTechAsync();
        }

        private async void MiTechChange_Click(object sender, EventArgs e)
        {
            await TechsChangeSelectedAsync();
        }

        private async void MiTechDelete_Click(object sender, EventArgs e)
        {
            await TechsDeleteSelectedAsync();
        }

        private async void TsbtnMileagesAdd_Click(object sender, EventArgs e)
        {
            await MileagesAddNewAsync();
        }

        private async void TsbtnMileagesChange_Click(object sender, EventArgs e)
        {
            await MileagesChangeSelectedAsync();
        }

        private async void TsbtnMileagesDelete_Click(object sender, EventArgs e)
        {
            await MileagesDeleteSelectedAsync();
        }

        private async void DgvMileages_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            await MileagesChangeSelectedAsync();
        }

        private async void DgvTechParts_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            await TechPartsChangeSelectedAsync();
        }

        private bool ToolStripsShowText
        {
            get => miViewToolStripsShowText.Checked;
            set
            {
                miViewToolStripsShowText.Checked = value;

                AppSettings.Default.ToolStripsShowText = value;

                toolStripMain.SetShowTextAndToolTips(value);
                toolStripTechs.SetShowTextAndToolTips(value);
                toolStripMileages.SetShowTextAndToolTips(value);
            }
        }

        private void MiViewToolStripsShowText_Click(object sender, EventArgs e)
        {
            ToolStripsShowText = !ToolStripsShowText;
        }

        private bool IsProgramBusy()
        {
            if (ProgramStatus.IsIdle) return false;

            Utils.Msg.Error(Resources.TextProgramBusy);

            return true;
        }

        private DataLoad GetUpdateDataLoad(FrmListType listType)
        {
            switch (listType)
            {
                case FrmListType.Techs: return DataLoad.Techs;
                case FrmListType.Parts: return DataLoad.TechParts;
                default: return default;
            }
        }

        public async Task<bool> ShowListAsync(FrmListType listType)
        {
            if (IsProgramBusy()) return false;

            if (FrmList.ShowDlg(this, listType))
            {
                var dataLoad = GetUpdateDataLoad(listType);

                await UpdateDataAsync(dataLoad);

                return true;
            }

            return false;
        }

        private async void MiList_Click(object sender, EventArgs e)
        {
            await ShowListAsync((FrmListType)((ToolStripItem)sender).Tag);
        }

        private async void TsbtnList_Click(object sender, EventArgs e)
        {
            await ShowListAsync((FrmListType)((ToolStripItem)sender).Tag);
        }

        private async void TsbtnTechPartAdd_Click(object sender, EventArgs e)
        {
            await TechPartsAddNewAsync();
        }

        private async void TsbtnTechPartChange_Click(object sender, EventArgs e)
        {
            await TechPartsChangeSelectedAsync();
        }

        private async void TsbtnTechPartDelete_Click(object sender, EventArgs e)
        {
            await TechPartsDeleteSelectedAsync();
        }

        private async void TsbtnMileageAddFromFile_ButtonClick(object sender, EventArgs e)
        {
            await LoadFromFilesAsync(FilesDialogType.Files);
        }

        private async void MiMileagesFromFiles_Click(object sender, EventArgs e)
        {
            await LoadFromFilesAsync(FilesDialogType.Files);
        }

        private async void MiMileagesFromDirectory_Click(object sender, EventArgs e)
        {
            await LoadFromFilesAsync(FilesDialogType.Directory);
        }

        private async void MiFileSettings_Click(object sender, EventArgs e)
        {
            await ShowSettingsAsync();
        }

        private void DgvMileages_SelectionChanged(object sender, EventArgs e)
        {
            presenterStatusStrip.MileageSelectedCount = dgvMileages.SelectedCount();
        }

        private void DgvTechParts_SelectionChanged(object sender, EventArgs e)
        {
            presenterStatusStrip.TechPartSelectedCount = dgvTechParts.SelectedCount();
        }
    }
}