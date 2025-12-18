using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
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

        private TechPartModel TechPart
        {
            get => techPart;
            set
            {
                techPart.Assign(value);

                cboxTech.SelectedValue = value.TechId ?? Sql.NewId;
                cboxPart.SelectedValue = value.PartId ?? Sql.NewId;

                dtpDateTimeInstall.SetDateTime(value.DateTimeInstall);
                dtpDateTimeRemove.SetDateTime(value.DateTimeRemove);
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
            UpdateSettings();

            await LoadDataAsync();

            TechPart = techPart;
        }
        
        private async Task LoadDataAsync()
        {
            DebugWrite.Line("start");

            var status = MainForm.ProgramStatus.Start(Status.LoadData);

            try
            {
                bindingSourceTechs.DataSource = Lists.Default.Techs.ToBindingList();

                bindingSourceTechs.Insert(0, new TechModel());

                var list = await MainForm.ListLoadAsync<PartModel>();

                bindingSourceParts.DataSource = list.ToBindingList();

                bindingSourceParts.Insert(0, new PartModel());
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseLoadFail, e.Message);
            }
            finally
            {
                MainForm.ProgramStatus.Stop(status);
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

        private async Task<bool> CheckDataAsync()
        {
            try
            {
                Utils.AssertComboBox<TechModel>(cboxTech, Resources.ErrorTechEmpty);

                Utils.AssertComboBox<PartModel>(cboxPart, Resources.ErrorPartEmpty);

                AssertDates();

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
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
    }
}