using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics.Forms
{
    public partial class FrmPart : Form
    {
        private readonly PartModel part = new PartModel();

        private PartModel Part
        {
            get => part;
            set
            {
                part.Assign(value);

                cboxFolder.SelectedValue = value.FolderId ?? Sql.NewId;

                tboxText.SetText(value.Text);

                tboxDescription.SetText(value.Description);
            }
        }

        public FrmPart()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, PartModel part)
        {
            using (var frm = new FrmPart()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmPart_Load(part);

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    part.Assign(frm.Part);
                }

                return result;
            }
        }

        private void FrmPart_Load(PartModel part)
        {
            LoadData();

            Part = part;
        }

        private void LoadData()
        {
            bindingSourceFolders.DataSource = Lists.Default.Folders.ToBindingList();

            bindingSourceFolders.Insert(0, new FolderModel());
        }

        private bool CheckData()
        {
            try
            {
                if (tboxText.IsEmpty())
                {
                    tboxText.Focus();

                    throw new Exception(Resources.ErrorValueNeedText);
                }

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
                var folder = cboxFolder.GetSelectedItem<FolderModel>();

                part.FolderId = folder.Id;
                part.FolderText = folder.Text;

                part.Text = tboxText.GetTrimText();

                part.Description = tboxDescription.GetTrimTextNullable();

                return true;
            }
            catch (Exception e)
            {
                DebugWrite.Error(e);

                Utils.Msg.Error(e.Message);

                return false;
            }
        }

        private void BtnOk_Click(object sender, System.EventArgs e)
        {
            if (CheckData() && UpdateData())
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}