using P3tr0viCh.Utils;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Enums;
using static Technics.PresenterStatusStripMain;

namespace Technics
{
    public partial class Main : Form, IMainForm, PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
    {
        private readonly PresenterStatusStripMain statusStripPresenter;

        public ProgramStatus ProgramStatus { get; } = new ProgramStatus();

        public Main()
        {
            InitializeComponent();

            statusStripPresenter = new PresenterStatusStripMain(this);
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            Text = string.Format(Resources.TitleMain, new AssemblyDecorator().VersionString(false));

            splitterMileages.Cursor = Cursors.SizeNS;
            splitterTechs.Cursor = Cursors.SizeWE;

            if (!SetProgramDirectory()) return;

            miListParts.Tag = ListType.Parts;
            tsbtnListParts.Tag = ListType.Parts;

            AddTechsRoot();

            AppSettingsLoad();

            ProgramStatus.StatusChanged += ProgramStatus_StatusChanged;

            AppSettings.LoadFormState(this, AppSettings.Default.FormStateMain);

            AppSettings.LoadDataGridColumns(dgvMileages, AppSettings.Default.ColumnsMileages);

            tvTechs.Width = AppSettings.Default.PanelTechsWidth;
            panelBottom.Height = AppSettings.Default.PanelBottomHeight;

            ToolStripsShowText = AppSettings.Default.ToolStripsShowText;

            UpdateSettings();

            await UpdateDataAsync(DataLoad.Techs);
        }

        private void ProgramStatus_StatusChanged(object sender, Status status)
        {
            statusStripPresenter.Status = status;

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
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public void UpdateSettings()
        {
            DataGridViewCellStyles.UpdateSettings();

            MileagesDateTime.DefaultCellStyle = DataGridViewCellStyles.DateTime;

            MileagesMileage.DefaultCellStyle =
                MileagesMileageCommon.DefaultCellStyle = DataGridViewCellStyles.Mileage;
        }

        private void TvTechs_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvTechs.SelectedNode = e.Node;
            }
        }

        private void DgvMileages_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            Utils.SelectCellOnCellMouseDown(dgvMileages, e);
        }

        private void BindingSourceMileages_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    tsbtnMileagesChange.Enabled = tsbtnMileagesDelete.Enabled = true;
                    break;
                case ListChangedType.Reset:
                case ListChangedType.ItemDeleted:
                    tsbtnMileagesDelete.Enabled = tsbtnMileagesChange.Enabled = bindingSourceMileages.Count > 0;
                    break;
            }
        }

        private async void TvTechs_AfterSelect(object sender, TreeViewEventArgs e)
        {
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

        private async Task ShowListAsync(ListType listType)
        {
            if (IsProgramBusy()) return;

            if (FrmList.ShowDlg(this, listType))
            {
                await Task.Delay(500);
//                await UpdateDataAsync();
            }
        }

        private async void MiList_Click(object sender, EventArgs e)
        {
            await ShowListAsync((ListType)((ToolStripItem)sender).Tag);
        }

        private async void TsbtnList_Click(object sender, EventArgs e)
        {
            await ShowListAsync((ListType)((ToolStripItem)sender).Tag);
        }
    }
}