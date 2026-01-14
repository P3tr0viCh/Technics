using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;

namespace Technics
{
    public partial class FrmTechPart : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly TechPartModel techPart = new TechPartModel();

        private bool selfChange = false;

        private TechPartModel TechPart
        {
            get => techPart;
            set
            {
                techPart.Assign(value);

                selfChange = true;

                cboxTech.SelectedValue = value.TechId ?? Sql.NewId;
                cboxPart.SelectedValue = value.PartId ?? Sql.NewId;

                dtpDateTimeInstall.SetDateTime(value.DateTimeInstall);
                dtpDateTimeRemove.SetDateTime(value.DateTimeRemove);

                selfChange = false;
            }
        }

        public FrmTechPart()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, TechPartModel techPart)
        {
            using (var frm = new FrmTechPart()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmTechPart_Load(techPart);

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    techPart.Assign(frm.TechPart);
                }

                return result;
            }
        }

        private async void FrmTechPart_Load(TechPartModel techPart)
        {
            dtpDateTimeInstall.ContextMenuStrip = Utils.CreateMenuDateTimePicker();
            dtpDateTimeRemove.ContextMenuStrip = Utils.CreateMenuDateTimePicker();

            UpdateSettings();

            await LoadDataAsync();

            TechPart = techPart;
        }

        private async Task LoadDataAsync()
        {
            DebugWrite.Line("start");

            var status = ProgramStatus.Default.Start(Status.LoadData);

            selfChange = true;

            try
            {
                bindingSourceTechs.DataSource = Lists.Default.Techs.ToBindingList();

                bindingSourceTechs.Insert(0, new TechModel());

                bindingSourceTechs.Position = 0;

                var list = await Database.Default.ListLoadAsync<PartModel>();

                bindingSourceParts.DataSource = list.OrderBy(part => part.Text).ToBindingList();

                bindingSourceParts.Insert(0, new PartModel());

                bindingSourceParts.Insert(1, new PartModel()
                {
                    Id = Consts.Id.ListEdit,
                    Text = Resources.TextListEdit
                });

                bindingSourceParts.Insert(2, new PartModel()
                {
                    Id = Consts.Id.ListAdd,
                    Text = Resources.TextListAdd
                });

                bindingSourceParts.Position = 0;
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
            dtpDateTimeInstall.CustomFormat =
            dtpDateTimeRemove.CustomFormat =
                AppSettings.Default.FormatDateTime;
        }

        private void AssertDates()
        {
            var dateRemove = dtpDateTimeRemove.GetDateTimeNullable();

            if (dateRemove == null) return;

            var dateInstall = dtpDateTimeInstall.GetDateTime();

            if (dateInstall > dateRemove)
            {
                throw new Exception(Resources.ErrorTechPartDatesWrong);
            }
        }

        private async Task AssertPartInUseAsync()
        {
            DebugWrite.Line("start");

            try
            {
                var part = cboxPart.GetSelectedItem<PartModel>();

                var filter = new Database.Filter.TechParts()
                {
                    Parts = new List<PartModel>() { part },
                };

                var techPartList = await Database.Default.TechPartsLoadAsync(filter);

                var list = techPartList.Where(item => item.Id != TechPart.Id).ToList();

                if (list.Count == 0) return;

                var dateInstall = dtpDateTimeInstall.GetDateTime();
                var dateRemove = dtpDateTimeRemove.GetDateTimeNullable();

                var lastItem = list.First();

                if (dateInstall > lastItem.DateTimeInstall)
                {
                    if (lastItem.DateTimeRemove == default)
                    {
                        throw new Exception(string.Format(Resources.ErrorPartInUse,
                            lastItem.PartText,
                            lastItem.TechText,
                            lastItem.DateTimeInstall.ToString(AppSettings.Default.FormatDateTime)));
                    }

                    if (dateInstall < lastItem.DateTimeRemove)
                    {
                        throw new Exception(string.Format(Resources.ErrorPartDateInstallLessDateRemove,
                            lastItem.PartText,
                            lastItem.TechText,
                            lastItem.DateTimeRemove?.ToString(AppSettings.Default.FormatDateTime)));
                    }

                    return;
                }

                list = list.Where(item => item.DateTimeInstall > dateInstall).ToList();

                if (list.Count == 0) return;

                var nextItem = list.Last();

                if (dateRemove == null)
                {
                    throw new Exception(string.Format(Resources.ErrorPartDateInstallLessDateNextInstall,
                        nextItem.PartText,
                        nextItem.TechText,
                        nextItem.DateTimeInstall.ToString(AppSettings.Default.FormatDateTime)));
                }
                else
                {
                    if (dateRemove > nextItem.DateTimeInstall)
                    {
                        throw new Exception(string.Format(Resources.ErrorPartDateRemoveGreaterDateNextInstall,
                            nextItem.PartText,
                            nextItem.TechText,
                            nextItem.DateTimeInstall.ToString(AppSettings.Default.FormatDateTime)));
                    }
                }
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private async Task<bool> CheckDataAsync()
        {
            try
            {
                Utils.AssertComboBox<TechModel>(cboxTech, Resources.ErrorTechEmpty);

                Utils.AssertComboBox<PartModel>(cboxPart, Resources.ErrorPartEmpty);

                AssertDates();

                await AssertPartInUseAsync();

                return true;
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

                techPart.TechId = tech.Id;
                techPart.TechText = tech.Text;

                var part = cboxPart.GetSelectedItem<PartModel>();

                techPart.PartId = part.Id;
                techPart.PartText = part.Text;

                techPart.DateTimeInstall = dtpDateTimeInstall.GetDateTime();
                techPart.DateTimeRemove = dtpDateTimeRemove.GetDateTimeNullable();

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

            cboxPart.SelectedIndex = 0;

            selfChange = false;
        }

        private async Task AfterChangeListAsync(PartModel value)
        {
            selfChange = true;

            var tech = cboxTech.GetSelectedItem<TechModel>();

            await LoadDataAsync();

            cboxTech.SelectedValue = tech?.Id ?? Sql.NewId;

            cboxPart.SelectedValue = value?.Id ?? Sql.NewId;

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

        private async Task AddPartAsync()
        {
            BeforeChangeList();

            var value = new PartModel();
#if DEBUG

            value.Text = $"Part {Str.Random(5)}";
#endif

            var text = value.Text;

            if (!Utils.TextInputBoxShow(ref text, Resources.TitlePart)) return;

            value.Text = text;

            if (!await ListItemSaveAsync(value)) return;

            await AfterChangeListAsync(value);
        }

        private async Task ChangePartsAsync()
        {
            BeforeChangeList();

            var changed = await MainForm.ShowListAsync(FrmListType.Parts);

            if (!changed) return;

            await AfterChangeListAsync(null);
        }

        private async void CboxPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            var part = cboxPart.GetSelectedItem<PartModel>();

            switch (part?.Id)
            {
                case Consts.Id.ListAdd:
                    await AddPartAsync();

                    break;
                case Consts.Id.ListEdit:
                    await ChangePartsAsync();

                    break;
            }
        }
    }
}