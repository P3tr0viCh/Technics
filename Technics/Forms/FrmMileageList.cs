using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    public partial class FrmMileageList : Form
    {
        public IMainForm MainForm => Owner as IMainForm;

        private readonly TechModel tech = new TechModel();

        private TechModel Tech
        {
            get => tech;
            set
            {
                tech.Assign(value);

                cboxTech.SelectedValue = value.Id;
            }
        }
        public FrmMileageList()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, IEnumerable<MileageModel> mileages)
        {
            using (var frm = new FrmMileageList()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmMileageList_Load(mileages);

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    foreach (var mileage in mileages)
                    {
                        mileage.TechId = frm.Tech.Id;
                        mileage.TechText = frm.Tech.Text;
                    }
                }

                return result;
            }
        }

        private void FrmMileageList_Load(IEnumerable<MileageModel> mileages)
        {
            LoadData();

            var techIds = mileages.Select(mileage => mileage.TechId).DistinctNotNullLong();

            if (techIds.Count() == 1)
            {
                Tech = Lists.Default.FindTechById(techIds.First());
            }
            else
            {
                Tech = new TechModel();
            }
        }

        private void LoadData()
        {
            bindingSourceTechs.DataSource = Lists.Default.Techs.ToBindingList();

            bindingSourceTechs.Insert(0, new TechModel());
        }

        private bool UpdateData()
        {
            try
            {
                Tech = cboxTech.GetSelectedItem<TechModel>();

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
            if (UpdateData())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}