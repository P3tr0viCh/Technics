using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Interfaces;
using static Technics.Database.Models;

namespace Technics
{
    public partial class FrmMileage : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly MileageModel mileage = new MileageModel();

        private bool selfChange = false;

        private MileageModel Mileage
        {
            get => mileage;
            set
            {
                mileage.Assign(value);

                cboxTech.SelectedValue = value.TechId ?? Sql.NewId;

                dtpDateTime.SetDateTime(value.DateTime);

                TextBoxSetValue(tboxMileage, value.Mileage);
                TextBoxSetValue(tboxMileageCommon, value.MileageCommon);

                tboxDescription.SetText(value.Description);
            }
        }

        public FrmMileage()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, MileageModel mileage)
        {
            using (var frm = new FrmMileage()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmMileage_Load(mileage);

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    mileage.Assign(frm.Mileage);
                }

                return result;
            }
        }

        private void FrmMileage_Load(MileageModel mileage)
        {
            LoadData();

            Mileage = mileage;
        }

        private void LoadData()
        {
            bindingSourceTechs.DataSource = Lists.Default.Techs.ToBindingList();

            bindingSourceTechs.Insert(0, new TechModel());

            dtpDateTime.CustomFormat = AppSettings.Default.FormatDateTime;
        }

        private async Task<double> GetMileageCommonPrevAsync(MileageModel mileage)
        {
            try
            {
                return await Database.Default.GetMileageCommonPrevAsync(mileage);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                throw new Exception(string.Format(Resources.MsgDatabaseLoadFail, e.Message));
            }
        }

        private double mileageCommonPrev;

        private void AssertMileageValue(TextBox textBox)
        {
            if (textBox.IsEmpty()) return;

            if (!textBox.IsDouble())
            {
                textBox.Focus();

                throw new Exception(Resources.ErrorValueNeedDouble);
            }

            if (textBox.GetDouble() < 0.0)
            {
                textBox.Focus();

                throw new Exception(Resources.ErrorValueNeedGreaterZero);
            }
        }

        private void TextBoxSetValue(TextBox textBox, double? value)
        {
            selfChange = true;

            textBox.SetDouble(value);

            selfChange = false;
        }

        private async Task<bool> CheckDataAsync()
        {
            try
            {
                Utils.AssertComboBox<TechModel>(cboxTech, Resources.ErrorTechEmpty);

                if (tboxMileage.IsEmpty() && tboxMileageCommon.IsEmpty())
                {
                    tboxMileage.Focus();

                    throw new Exception(Resources.ErrorMileagesEmpty);
                }

                var tech = cboxTech.GetSelectedItem<TechModel>();
   
                var mileagePrev = new MileageModel()
                {
                    Id = Mileage.Id,
                    TechId = tech.Id,
                    DateTime = dtpDateTime.GetDateTime(),
                };

                mileageCommonPrev = await GetMileageCommonPrevAsync(mileagePrev);

                DebugWrite.Line($"mileageCommonPrev = {mileageCommonPrev}");

                AssertMileageValue(tboxMileage);

                var mileage = tboxMileage.GetDouble();

                if (mileage > 0.0)
                {
                    TextBoxSetValue(tboxMileageCommon, mileageCommonPrev + mileage);

                    return true;
                }

                AssertMileageValue(tboxMileageCommon);

                var mileageCommon = tboxMileageCommon.GetDouble();

                if (mileageCommon <= mileageCommonPrev)
                {
                    tboxMileageCommon.Focus();

                    throw new Exception(string.Format(Resources.ErrorMileageCommonWrong,
                        mileageCommon, mileageCommonPrev));
                }

                TextBoxSetValue(tboxMileage, mileageCommon - mileageCommonPrev);

                return true;
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

                mileage.TechId = tech.Id;
                mileage.TechText = tech.Text;

                mileage.DateTime = dtpDateTime.GetDateTime();

                mileage.Mileage = tboxMileage.GetDouble();
                mileage.MileageCommon = tboxMileageCommon.GetDouble();

                mileage.Description = tboxDescription.GetTrimTextNullable();

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

        private void TboxMileages_TextChanged(object sender, EventArgs e)
        {
            if (selfChange) return;

            selfChange = true;

            if (sender == tboxMileage)
            {
                tboxMileageCommon.Text = string.Empty;
            }
            else
            {
                tboxMileage.Text = string.Empty;
            }

            selfChange = false;
        }
    }
}