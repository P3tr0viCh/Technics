using P3tr0viCh.Utils;
using System.Collections.Generic;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics
{
    internal class PresenterFrmListParts : PresenterFrmListBase<PartModel>
    {
        public override FrmListType ListType => FrmListType.Parts;

        public PresenterFrmListParts(IFrmList frmList) : base(frmList)
        {
        }

        protected override PartModel GetNewItem()
        {
            var text =
#if DEBUG
                $"Part {Str.Random(5)}";
#else
                string.Empty;
#endif

            return new PartModel()
            {
                Text = text
            };
        }

        protected override string FormTitle => Resources.TitleListParts;

        protected override void LoadFormState()
        {
            AppSettings.LoadFormState(Form, AppSettings.Default.FormStateListParts);
            AppSettings.LoadDataGridColumns(FrmList.DataGridView, AppSettings.Default.ColumnsListParts);

            presenterDataGridView.SortColumn = nameof(TechModel.Text);
        }

        protected override void SaveFormState()
        {
            AppSettings.Default.FormStateListParts = AppSettings.SaveFormState(Form);
            AppSettings.Default.ColumnsListParts = AppSettings.SaveDataGridColumns(FrmList.DataGridView);
        }

        protected override bool ShowItemChangeDialog(PartModel value)
        {
            var text = value.Text;

            if (!Utils.TextInputBoxShow(ref text, Resources.TitlePart)) return false;

            value.Text = text;

            return true;
        }

        protected override bool ShowItemDeleteDialog(IEnumerable<PartModel> list)
        {
            return Utils.Msg.Question(list);
        }

        protected override void UpdateColumns()
        {
            FrmList.DataGridView.Columns[nameof(PartModel.Text)].HeaderText = ResourcesColumnHeader.Text;
        }
    }
}