using P3tr0viCh.Database;
using System.Collections.Generic;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    internal class PresenterFrmListTechs : PresenterFrmListBase<TechModel>
    {
        public override FrmListType ListType => FrmListType.Techs;

        public PresenterFrmListTechs(IFrmList frmList) : base(frmList)
        {
            Grants = FrmListGrant.Change | FrmListGrant.Delete;
        }

        protected override TechModel GetNewItem() { return new TechModel(); }

        protected override string FormTitle => Resources.TitleListTechs;

        protected override void LoadFormState()
        {
            AppSettings.LoadFormState(Form, AppSettings.Default.FormStateListTechs);
            AppSettings.LoadDataGridColumns(FrmList.DataGridView, AppSettings.Default.ColumnsListTechs);

            SortColumn = nameof(TechModel.Text);
        }

        protected override void SaveFormState()
        {
            AppSettings.Default.FormStateListTechs = AppSettings.SaveFormState(Form);
            AppSettings.Default.ColumnsListTechs = AppSettings.SaveDataGridColumns(FrmList.DataGridView);
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

        protected override void UpdateColumns()
        {
            FrmList.DataGridView.Columns[nameof(TechModel.FolderId)].Visible = false;
            FrmList.DataGridView.Columns[nameof(BaseText.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        protected override int Compare(TechModel x, TechModel y)
        {
            return x.Text.CompareTo(y.Text);
        }
    }
}