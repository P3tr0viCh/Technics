using P3tr0viCh.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics.Presenters
{
    internal class PresenterFrmListParts : PresenterFrmListBase<PartModel>
    {
        public override FrmListType ListType => FrmListType.Parts;

        public PresenterFrmListParts(IFrmList frmList) : base(frmList)
        {
        }

        protected override PartModel GetNewItem()
        {
#if DEBUG
            var item = base.GetNewItem();

            item.Text = $"Part {Str.Random(5)}";

            return item;
#else
            return base.GetNewItem();
#endif
        }

        protected override string FormTitle => Resources.TitleListParts;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            presenterDataGridView.SortColumn = nameof(TechModel.Text);
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

        protected override async Task ListItemDeleteAsync(IEnumerable<PartModel> list)
        {
            await Database.Default.PartDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            DataGridView.Columns[nameof(PartModel.Text)].HeaderText = ResourcesColumnHeader.Text;
        }

        public override int Compare(PartModel x, PartModel y, string dataPropertyName)
        {
            return Comparer.Default.Compare(x.Text, y.Text);
        }
    }
}