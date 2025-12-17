using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;

namespace Technics
{
    internal class PresenterFrmListParts : PresenterFrmListBase<PartModel>
    {
        public override ListType ListType => ListType.Parts;

        public PresenterFrmListParts(IFrmList frmList) : base(frmList)
        {
        }

        private Form Form => FrmList as Form;

        protected override void LoadFormState()
        {
            Form.Text = Resources.TitleListParts;

            AppSettings.LoadFormState(Form, AppSettings.Default.FormStateListParts);
            AppSettings.LoadDataGridColumns(FrmList.DataGridView, AppSettings.Default.ColumnsListParts);
        }

        protected override void UpdateColumns()
        {
            FrmList.DataGridView.Columns[nameof(BaseText.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        protected override void SaveFormState()
        {
            AppSettings.Default.FormStateListParts = AppSettings.SaveFormState(Form);
            AppSettings.Default.ColumnsListParts = AppSettings.SaveDataGridColumns(FrmList.DataGridView);
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

        protected override bool ShowItemChangeDialog(PartModel value)
        {
            var text = value.Text;

            if (!Utils.TextInputBoxShow(ref text, Resources.TitlePart)) return false;

            value.Text = text;

            return true;
        }

        protected override bool ShowItemDeleteDialog(List<PartModel> list)
        {
            if (list?.Count == 0) return false;

            var firstItem = list.FirstOrDefault();

            var text = firstItem.Text;

            var question = list.Count == 1 ? Resources.QuestionItemDelete : Resources.QuestionItemListDelete;

            return Utils.Msg.Question(question, text, list.Count - 1);
        }
    }
}