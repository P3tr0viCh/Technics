using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics.Presenters
{
    internal class PresenterFrmListTechs : PresenterFrmListBase<TechModel>
    {
        public override FrmListType ListType => FrmListType.Techs;

        public PresenterFrmListTechs(IFrmList frmList) : base(frmList)
        {
            Grants = FrmListGrant.Change | FrmListGrant.Delete;

            ItemChangeDialog += PresenterFrmListTechs_ItemChangeDialog;

            ItemListDeleteDialog += PresenterFrmListTechs_ItemListDeleteDialog;
        }

        protected override string FormTitle => Resources.TitleListTechs;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(TechModel.Text);
        }

        private bool ShowItemChangeDialog(TechModel value)
        {
            var text = value.Text;

            if (!Utils.TextInputBoxShow(ref text, Resources.TitleTech)) return false;

            value.Text = text;

            return true;
        }

        private void PresenterFrmListTechs_ItemChangeDialog(object sender, ItemDialogEventArgs<TechModel> e)
        {
            e.Ok = ShowItemChangeDialog(e.Value);
        }

        private void PresenterFrmListTechs_ItemListDeleteDialog(object sender, ItemListDialogEventArgs<TechModel> e)
        {
            e.Ok = Utils.Msg.Question(e.Values);
        }

        protected override async Task ListItemDeleteAsync(IEnumerable<TechModel> list)
        {
            await Database.Default.TechDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

            FrmList.DataGridView.Columns[nameof(TechModel.FolderId)].Visible = false;

            FrmList.DataGridView.Columns[nameof(TechModel.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        public override int Compare(TechModel x, TechModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            return EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
        }
    }
}