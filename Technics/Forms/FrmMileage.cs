using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class FrmMileage : Form
    {
        private bool selfChange = false;

        private readonly MileageModel mileage = new MileageModel();

        private MileageModel Mileage
        {
            get => mileage;
            set
            {
                mileage.Assign(value);

                selfChange = true;

                cboxTech.SelectedValue = value.TechId ?? Sql.NewId;

                dtpDateTime.SetDateTime(value.DateTime);

                if (value.MileageType == MileageType.Single)
                {
                    tboxMileage.SetDouble(value.Mileage, AppSettings.Default.FormatMileagesMileage);
                }
                else
                {
                    tboxMileageCommon.SetDouble(value.MileageCommon, AppSettings.Default.FormatMileagesMileageCommon);
                }

                tboxDescription.SetText(value.Description);

                selfChange = false;
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
            dtpDateTime.ContextMenuStrip = Utils.CreateMenuDateTimePicker();

            LoadData();

            Mileage = mileage;
        }

        private void LoadData()
        {
            bindingSourceTechs.DataSource = Lists.Default.Techs.ToBindingList();

            bindingSourceTechs.Insert(0, new TechModel());

            dtpDateTime.CustomFormat = AppSettings.Default.FormatDateTime;
        }

        private class MileageCommons
        {
            public double Prev { get; private set; }
            public double Next { get; private set; }

            public MileageCommons(double prev, double next)
            {
                Prev = prev;
                Next = next;
            }
        }

        private async Task<MileageCommons> GetMileageCommonsAsync(MileageModel mileage)
        {
            try
            {
                var prev = await Database.Default.MileagesGetMileageCommonPrevAsync(mileage);
                var next = await Database.Default.MileagesGetMileageCommonNextAsync(mileage);

                return new MileageCommons(prev, next);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                throw new Exception(string.Format(Resources.MsgDatabaseLoadFail, e.Message));
            }
        }

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

        private void TextBoxMileageSetFocus()
        {
            if (Mileage.MileageType == MileageType.Single)
            {
                tboxMileage.Focus();
            }
            else
            {
                tboxMileageCommon.Focus();
            }
        }

        private async Task<bool> CheckDataAsync()
        {
            try
            {
                if (tboxMileage.IsEmpty() && tboxMileageCommon.IsEmpty())
                {
                    tboxMileage.Focus();

                    throw new Exception(Resources.ErrorMileagesEmpty);
                }

                AssertMileageValue(tboxMileage);

                AssertMileageValue(tboxMileageCommon);

                var mileage = tboxMileage.GetDouble();

                var mileageCommon = tboxMileageCommon.GetDouble();

                if (mileage == 0 && mileageCommon == 0)
                {
                    tboxMileage.Focus();

                    throw new Exception(Resources.ErrorMileagesEmpty);
                }

                if (mileage > 0.0)
                {
                    Mileage.MileageType = MileageType.Single;
                }
                else
                {
                    Mileage.MileageType = MileageType.Common;
                }

                var tech = cboxTech.GetSelectedItem<TechModel>();

                if (tech.IsNew) return true;

                var mileageModel = new MileageModel()
                {
                    Id = Mileage.Id,
                    TechId = tech.Id,
                    DateTime = dtpDateTime.GetDateTime(),
                };

                var mileageCommons = await GetMileageCommonsAsync(mileageModel);

                DebugWrite.Line($"mileageCommons = {mileageCommons.Prev}, {mileageCommons.Next}");

                if (Mileage.MileageType == MileageType.Single)
                {
                    mileageCommon = mileageCommons.Prev + mileage;
                }
                else
                {
                    mileage = mileageCommon - mileageCommons.Prev;
                }

                if (mileageCommon > mileageCommons.Next && mileageCommons.Next > 0)
                {
                    TextBoxMileageSetFocus();

                    throw new Exception(string.Format(Resources.ErrorMileageCommonWrongGreater,
                        mileageCommon, mileageCommons.Next));
                }

                if (mileageCommon < mileageCommons.Prev && mileageCommons.Prev > 0)
                {
                    TextBoxMileageSetFocus();

                    throw new Exception(string.Format(Resources.ErrorMileageCommonWrongLess,
                        mileageCommon, mileageCommons.Prev));
                }

                selfChange = true;

                tboxMileageCommon.SetDouble(mileageCommon, AppSettings.Default.FormatMileagesMileageCommon);

                tboxMileage.SetDouble(mileage, AppSettings.Default.FormatMileagesMileage);

                selfChange = false;

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