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

            if (!SetProgramDirectory()) return;

            AddTechRoot();

            AppSettingsLoad();

            ProgramStatus.StatusChanged += ProgramStatus_StatusChanged;

            AppSettings.LoadFormState(this, AppSettings.Default.FormStateMain);

            await LoadTechAsync();
        }

        private void ProgramStatus_StatusChanged(object sender, Status status)
        {
            statusStripPresenter.Status = status;

            if (ProgramStatus.IsIdle)
            {
                UseWaitCursor = false;

                panelLeft.Enabled = true;
            }
            else
            {
                UseWaitCursor = true;

                panelLeft.Enabled = false;
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

            AppSettingsSave();

            Utils.Log.WriteProgramStop();
        }

        private void MiFileClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        ToolStripStatusLabel PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip.GetLabel(StatusLabel label)
        {
            switch (label)
            {
                case StatusLabel.Status: return LabelStatus;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void AddTechRoot()
        {
            tvTechs.Nodes.Add(new TreeNodeFolder()
            {
                Text = Resources.TextTechAll
            });
        }

        private TreeNodeFolder GetParent(TreeNode node)
        {
            var result = node is TreeNodeItem ? node.Parent : node;

            return result as TreeNodeFolder;
        }

        private void TechAdd(TreeNode parent, TreeNode value)
        {
            parent.Nodes.Add(value);

            parent.Expand();

            tvTechs.SelectedNode = value;
        }

        private async void MiTechAddFolder_Click(object sender, EventArgs e)
        {
            var node = new TreeNodeFolder
            {
                Text = $"Folder {Str.Random(3)}"
            };

            var parent = GetParent(tvTechs.SelectedNode);

            node.Folder.ParentId = parent.Folder.Id;

            await Database.Default.ListItemSaveAsync(node.Folder);

            TechAdd(parent, node);
        }

        private void MiTechAddItem_Click(object sender, EventArgs e)
        {
            var node = new TreeNodeItem
            {
                Text = "Item"
            };

            var parent = GetParent(tvTechs.SelectedNode);

            TechAdd(parent, node);
        }
    }
}