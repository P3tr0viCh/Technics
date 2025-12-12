using P3tr0viCh.Utils;
using System;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Enums;
using static Technics.PresenterStatusStripMain;

namespace Technics
{
    public partial class Main : Form, PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
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

            AddTechsRoot();

            AppSettingsLoad();

            ProgramStatus.StatusChanged += ProgramStatus_StatusChanged;

            AppSettings.LoadFormState(this, AppSettings.Default.FormStateMain);

            AppSettings.LoadDataGridColumns(dgvMileages, AppSettings.Default.ColumnsMileages);

            tvTechs.Width = AppSettings.Default.PanelTechsWidth;
            panelBottom.Height = AppSettings.Default.PanelBottomHeight;

            await UpdateDataAsync();
        }

        private void ProgramStatus_StatusChanged(object sender, Status status)
        {
            statusStripPresenter.Status = status;

            if (ProgramStatus.IsIdle)
            {
                UseWaitCursor = false;

                toolStripContainer.ContentPanel.Enabled = true;
            }
            else
            {
                UseWaitCursor = true;

                toolStripContainer.ContentPanel.Enabled = false;
            }
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

            AppSettingsSave();

            Utils.Log.WriteProgramStop();
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

        private void TvTechs_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvTechs.SelectedNode = e.Node;
            }
        }

        private void TvTechs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedChanged();
        }

        private async void MiTechAddFolder_Click(object sender, EventArgs e)
        {
            await TechsAddNewFolderAsync();
        }

        private async void MiTechAddItem_Click(object sender, EventArgs e)
        {
            await TechsAddNewTechAsync();
        }

        private void TsbtnTechChange_Click(object sender, EventArgs e)
        {

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

        private void MiTechChange_Click(object sender, EventArgs e)
        {

        }

        private async void MiTechDelete_Click(object sender, EventArgs e)
        {
            await TechsDeleteSelectedAsync();
        }

        private async void MiMileagesAddMileage_Click(object sender, EventArgs e)
        {
            await MileageAddNewAsync();
        }
    }
}