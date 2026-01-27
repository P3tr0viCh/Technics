using P3tr0viCh.Utils.Comparers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Interfaces;
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
        }

        protected override string FormTitle => Resources.TitleListTechs;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(TechModel.Text);
        }

        protected override bool ShowItemChangeDialog(TechModel value)
        {
            var text = value.Text;

            if (!Utils.TextInputBoxShow(ref text, Resources.TitleTech)) return false;

            value.Text = text;

            return true;
        }

        protected override bool ShowItemDeleteDialog(IEnumerable<TechModel> list)
        {
            return Utils.Msg.Question(list);
        }

        protected override async Task ListItemDeleteAsync(IEnumerable<TechModel> list)
        {
            await Database.Default.TechDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            DataGridView.Columns[nameof(TechModel.FolderId)].Visible = false;

            DataGridView.Columns[nameof(TechModel.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        public override int Compare(TechModel x, TechModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            return EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
        }
    }
}