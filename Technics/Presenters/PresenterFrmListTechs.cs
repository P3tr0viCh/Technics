using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Forms;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics.Presenters
{
    internal class PresenterFrmListTechs : PresenterFrmList<TechModel>
    {
        public override FrmListType ListType => FrmListType.Techs;

        public PresenterFrmListTechs(IFrmList frmList) : base(frmList)
        {
            Grants = FrmListGrant.Change | FrmListGrant.Delete | FrmListGrant.Sort;

            ItemsChangeDialog += PresenterFrmListTechs_ItemsChangeDialog;
            ItemsDeleteDialog += PresenterFrmListTechs_ItemsDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListTechs;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(TechModel.Text);
        }

        private async Task PresenterFrmListTechs_ItemsChangeDialog(object sender, ItemsDialogEventArgs<TechModel> e)
        {
            e.Ok = FrmTech.ShowDlg(Form, e.Values.First());

            await Task.CompletedTask;
        }

        private async Task PresenterFrmListTechs_ItemsDeleteDialog(object sender, ItemsDialogEventArgs<TechModel> e)
        {
            e.Ok = Utils.Msg.Question(e.Values);

            await Task.CompletedTask;
        }

        protected override async Task DatabaseListItemsDeleteAsync(IEnumerable<TechModel> list)
        {
            await Database.Default.TechDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

            FrmList.DataGridView.Columns[nameof(TechModel.Text)].DisplayIndex = 0;

            FrmList.DataGridView.Columns[nameof(TechModel.Text)].HeaderText = ResourcesColumnHeader.Text;
            FrmList.DataGridView.Columns[nameof(TechModel.StateAsString)].HeaderText = ResourcesColumnHeader.State;
            FrmList.DataGridView.Columns[nameof(TechModel.Description)].HeaderText = ResourcesColumnHeader.Description;

            FrmList.DataGridView.Columns[nameof(TechModel.FolderId)].Visible = false;
            FrmList.DataGridView.Columns[nameof(TechModel.FolderText)].Visible = false;
            FrmList.DataGridView.Columns[nameof(TechModel.State)].Visible = false;
            FrmList.DataGridView.Columns[nameof(TechModel.AvailableForUse)].Visible = false;

            FrmList.DataGridView.Columns[nameof(TechModel.StateAsString)].DefaultCellStyle = DataGridViewCellStyles.PartsState;
        }

        public override int Compare(TechModel x, TechModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            return EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
        }
    }
}