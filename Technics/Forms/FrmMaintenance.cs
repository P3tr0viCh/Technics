using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Interfaces;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.ProgramStatus;

namespace Technics
{
    public partial class FrmMaintenance : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly MaintenanceModel maintenance = new MaintenanceModel();

        private bool selfChange = false;

        private MaintenanceModel Maintenance
        {
            get => maintenance;
            set
            {
                maintenance.Assign(value);

                selfChange = true;

                cboxTech.SelectedValue = value.TechId ?? Sql.NewId;
                cboxMt.SelectedValue = value.MtId ?? Sql.NewId;

                dtpDateTime.SetDateTime(value.DateTime);

                selfChange = false;
            }
        }

        public FrmMaintenance()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, MaintenanceModel maintenance)
        {
            using (var frm = new FrmMaintenance()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmMaintenance_Load(maintenance);

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    maintenance.Assign(frm.Maintenance);
                }

                return result;
            }
        }

        private async void FrmMaintenance_Load(MaintenanceModel maintenance)
        {
            dtpDateTime.AddMenuDateTimeActions();

            UpdateSettings();

            await LoadDataAsync(maintenance.TechId);

            Maintenance = maintenance;

            UpdateMtsList(Maintenance.TechId);
        }

        private IEnumerable<MtModel> Mts { get; set; }

        private async Task LoadDataAsync(long? techId)
        {
            DebugWrite.Line("start");

            var status = ProgramStatus.Default.Start(Status.LoadData);

            selfChange = true;

            try
            {
                bindingSourceTechs.DataSource = null;

                var techs = Lists.Default.Techs.GetAvailableForUse(techId);

                techs.Insert(0, new TechModel());

                bindingSourceTechs.DataSource = techs;

                var list = await Database.Default.ListLoadAsync<MtModel>();

                list = list.OrderBy(mt => mt.Text);

                Mts = list;
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseLoadFail, e.Message);
            }
            finally
            {
                selfChange = false;

                ProgramStatus.Default.Stop(status);
            }

            DebugWrite.Line("end");
        }

        private void UpdateSettings()
        {
            dtpDateTime.CustomFormat = AppSettings.Default.FormatDateTime;
        }

        private async Task<bool> CheckDataAsync()
        {
            try
            {
                Utils.AssertComboBox<TechModel>(cboxTech, Resources.ErrorTechEmpty);

                Utils.AssertComboBox<PartModel>(cboxMt, Resources.ErrorMaintenanceEmpty);

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(e.Message);

                return false;
            }
        }

        private bool UpdateData()
        {
            try
            {
                var tech = cboxTech.GetSelectedItem<TechModel>();

                maintenance.TechId = tech.Id;
                maintenance.TechText = tech.Text;

                var mt = cboxMt.GetSelectedItem<MtModel>();

                maintenance.MtId = mt.Id;
                maintenance.MtText = mt.Text;

                maintenance.DateTime = dtpDateTime.GetDateTime();

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Utils.Msg.Error(e.Message);

                return false;
            }
        }

        private async void BtnOk_Click(object sender, EventArgs e)
        {
            if (await CheckDataAsync() && UpdateData())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void BeforeChangeList()
        {
            selfChange = true;

            cboxMt.SelectedIndex = 0;

            selfChange = false;
        }

        private async Task AfterChangeListAsync(PartModel value)
        {
            selfChange = true;

            var tech = cboxTech.GetSelectedItem<TechModel>();

            await LoadDataAsync(maintenance.TechId);

            cboxTech.SelectedValue = tech?.Id ?? Sql.NewId;

            UpdateMtsList(tech);

            cboxMt.SelectedValue = value?.Id ?? Sql.NewId;

            selfChange = false;
        }

        private async Task<bool> ListItemSaveAsync(PartModel value)
        {
            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                await Database.Default.ListItemSaveAsync(value);

                return true;
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemSaveFail, e.Message);

                return false;
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }
        }

        private async Task AddMtAsync()
        {
            BeforeChangeList();

            var value = new PartModel();
#if DEBUG

            value.Text = $"Part {Str.Random(5)}";
#endif

            var text = value.Text;

            if (!Utils.TextInputBoxShow(ref text, Resources.TitlePart)) return;

            value.Text = text;

            var tech = cboxTech.GetSelectedItem<TechModel>();

            var folder = Lists.Default.Folders.Find(tech?.FolderId);

            value.FolderId = folder?.Id;
            value.FolderText = folder?.Text;

            if (!await ListItemSaveAsync(value)) return;

            await AfterChangeListAsync(value);
        }

        private async Task ChangeMtsAsync()
        {
            BeforeChangeList();

            var changed = await MainForm.ShowListAsync(FrmListType.Parts);

            if (!changed) return;

            await AfterChangeListAsync(null);
        }

        private void CboxTech_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            var tech = cboxTech.GetSelectedItem<TechModel>();

            UpdateMtsList(tech);
        }

        private IEnumerable<MtModel> GetMtsForTech(TechModel tech)
        {
            var availableMts = Mts.
                Where(mt => mt.Id == Maintenance.MtId).
                ToBindingList();

            if (tech?.IsNew ?? true) return availableMts;

            var folders = Lists.Default.Folders.GetFolderList(tech?.FolderId);

            DebugWrite.Line($"folders: {string.Join(", ", folders.Select(f => f.Text))}");

            DebugWrite.Line($"mts folderid: {string.Join(", ", availableMts.Select(p => p.FolderId))}");

            return availableMts.Where(mt =>
                mt.FolderId == null ||
                folders.Select(f => f.Id).Contains((long)mt.FolderId));
        }

        private void UpdateMtsList(TechModel tech)
        {
            selfChange = true;

            bindingSourceMts.DataSource = null;

            var list = GetMtsForTech(tech).ToBindingList();

            list.Insert(0, new MtModel());

            list.Insert(1, new MtModel()
            {
                Id = Consts.Id.ListEdit,
                Text = Resources.TextListEdit
            });

            list.Insert(2, new MtModel()
            {
                Id = Consts.Id.ListAdd,
                Text = Resources.TextListAdd
            });

            bindingSourceMts.DataSource = list;

            cboxMt.SelectedValue = Maintenance.MtId ?? Sql.NewId;

            selfChange = false;
        }

        private void UpdateMtsList(long? techId)
        {
            var tech = Lists.Default.Techs.Find(techId);

            UpdateMtsList(tech);
        }

        private async void CboxMaintenance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            var mt = cboxMt.GetSelectedItem<MtModel>();

            switch (mt?.Id)
            {
                case Consts.Id.ListAdd:
                    await AddMtAsync();

                    break;
                case Consts.Id.ListEdit:
                    await ChangeMtsAsync();

                    break;
                default:
                    maintenance.MtId = mt.Id;

                    break;
            }
        }

        private void FrmMaintenance_FormClosing(object sender, FormClosingEventArgs e)
        {
            selfChange = true;
        }
    }
}