using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Technics.Database.Models;

namespace Technics.Forms
{
    public partial class FrmMt : Form
    {
        private readonly MtModel mt = new MtModel();

        private MtModel Mt
        {
            get => mt;
            set
            {
                mt.Assign(value);

                cboxFolder.SelectedValue = value.FolderId ?? Sql.NewId;

                tboxText.SetText(value.Text);

                tboxDescription.SetText(value.Description);
            }
        }

        public FrmMt()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, MtModel mt)
        {
            using (var frm = new FrmMt()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmMt_Load(mt);

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    mt.Assign(frm.Mt);
                }

                return result;
            }
        }

        private void FrmMt_Load(MtModel mt)
        {
            LoadData();

            Mt = mt;
        }

        private void LoadData()
        {
            bindingSourceFolders.DataSource = Lists.Default.Folders.ToBindingList();

            bindingSourceFolders.Insert(0, new FolderModel());
        }

        private async Task<bool> CheckDataAsync()
        {
            try
            {
                Utils.AssertTextEmpty(tboxText);

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
                var folder = cboxFolder.GetSelectedItem<FolderModel>();

                mt.FolderId = folder.Id;
                mt.FolderText = folder.Text;

                mt.Text = tboxText.GetTrimText();

                mt.Description = tboxDescription.GetTrimTextNullable();

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Utils.Msg.Error(e.Message);

                return false;
            }
        }

        private async void BtnOk_Click(object sender, System.EventArgs e)
        {
            if (await CheckDataAsync() && UpdateData())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}