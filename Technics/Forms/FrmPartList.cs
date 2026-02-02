using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static Technics.Database.Models;

namespace Technics.Forms
{
    public partial class FrmPartList : Form
    {
        private readonly FolderModel folder = new FolderModel();

        private FolderModel Folder
        {
            get => folder;
            set
            {
                folder.Assign(value);

                cboxFolder.SelectedValue = value?.Id ?? Sql.NewId;
            }
        }

        public FrmPartList()
        {
            InitializeComponent();
        }

        public static bool ShowDlg(Form owner, IEnumerable<PartModel> parts)
        {
            using (var frm = new FrmPartList()
            {
                Owner = owner,
            })
            {
                frm.Load += (sender, args) => frm.FrmPart_Load(parts);

                var result = frm.ShowDialog(owner) == DialogResult.OK;

                if (result)
                {
                    foreach (var part in parts)
                    {
                        part.FolderId = frm.Folder.Id;
                        part.FolderText = frm.Folder.Text;
                    }
                }

                return result;
            }
        }

        private void FrmPart_Load(IEnumerable<PartModel> parts)
        {
            LoadData();

            var folderIds = parts.Select(part => part.FolderId).Distinct();

            if (folderIds.Count() == 1)
            {
                Folder = Lists.Default.Folders.Find(folderIds.First());
            }
            else
            {
                Folder = new FolderModel();
            }
        }

        private void LoadData()
        {
            bindingSourceFolders.DataSource = Lists.Default.Folders.ToBindingList();

            bindingSourceFolders.Insert(0, new FolderModel());
        }

        private bool UpdateData()
        {
            try
            {
                Folder = cboxFolder.GetSelectedItem<FolderModel>();

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