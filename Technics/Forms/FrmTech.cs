using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Technics.Database.Models;

namespace Technics.Forms
{
    public partial class FrmTech : Form
    {
        private readonly TechModel tech = new TechModel();

        private TechModel Tech
        {
            get => tech;
            set
            {
                tech.Assign(value);

                tboxText.SetText(value.Text);

                tboxDescription.SetText(value.Description);

                cboxState.Checked = value.State;
            }
        }

        public FrmTech()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, TechModel part)
        {
            using (var frm = new FrmTech()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmTech_Load(part);

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    part.Assign(frm.Tech);
                }

                return result;
            }
        }

        private void FrmTech_Load(TechModel part)
        {
            LoadData();

            Tech = part;
        }

        private void LoadData()
        {
        }

        private bool CheckData()
        {
            try
            {
                Utils.AssertTextEmpty(tboxText);

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
                tech.Text = tboxText.GetTrimText();

                tech.Description = tboxDescription.GetTrimTextNullable();

                tech.State = cboxState.Checked;

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Utils.Msg.Error(e.Message);

                return false;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (CheckData() && UpdateData())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}