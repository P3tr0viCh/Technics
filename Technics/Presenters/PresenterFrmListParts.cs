using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Forms;
using P3tr0viCh.Utils.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Technics.Forms;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics.Presenters
{
    internal class PresenterFrmListParts : PresenterFrmListBase<PartModel>
    {
        public override FrmListType ListType => FrmListType.Parts;

        public PresenterFrmListParts(IFrmList frmList) : base(frmList)
        {
            Grants = Grants.AddFlag(FrmListGrant.MultiChange);

            ItemChangeDialog += PresenterFrmListParts_ItemChangeDialog;
            ItemListChangeDialog += PresenterFrmListParts_ItemListChangeDialog;

            ItemListDeleteDialog += PresenterFrmListParts_ItemListDeleteDialog;
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

            PresenterDataGridView.SortColumn = nameof(TechModel.Text);
        }

        private void PresenterFrmListParts_ItemChangeDialog(object sender, ItemDialogEventArgs<PartModel> e)
        {
            e.Ok = FrmPart.ShowDlg(Form, e.Value);
        }

        private void PresenterFrmListParts_ItemListChangeDialog(object sender, ItemListDialogEventArgs<PartModel> e)
        {
            e.Ok = FrmPartList.ShowDlg(Form, e.Values);
        }

        private void PresenterFrmListParts_ItemListDeleteDialog(object sender, ItemListDialogEventArgs<PartModel> e)
        {
            e.Ok = Utils.Msg.Question(e.Values);
        }

        protected async override Task<IEnumerable<PartModel>> ListLoadAsync(CancellationToken cancellationToken)
        {
            return await Database.Default.ListLoadAsync<PartModel>(ResourcesSql.SelectParts);
        }

        protected override async Task ListItemDeleteAsync(IEnumerable<PartModel> list)
        {
            await Database.Default.PartDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

            DataGridView.Columns[nameof(PartModel.FolderText)].DisplayIndex = 0;
            DataGridView.Columns[nameof(PartModel.Text)].DisplayIndex = 1;

            DataGridView.Columns[nameof(PartModel.Text)].HeaderText = ResourcesColumnHeader.Text;
            DataGridView.Columns[nameof(PartModel.FolderText)].HeaderText = ResourcesColumnHeader.Folder;
            DataGridView.Columns[nameof(PartModel.Description)].HeaderText = ResourcesColumnHeader.Description;

            DataGridView.Columns[nameof(PartModel.FolderId)].Visible = false;
        }

        public override int Compare(PartModel x, PartModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(PartModel.FolderText):
                    result = EmptyStringComparer.Default.Compare(x.FolderText, y.FolderText, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Text, y.Text, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Description, y.Description, ComparerSortOrder.Ascending);
                    break;
                case nameof(PartModel.Text):
                    result = EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.FolderText, y.FolderText, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Description, y.Description, ComparerSortOrder.Ascending);
                    break;
                case nameof(PartModel.Description):
                    result = EmptyStringComparer.Default.Compare(x.Description, y.Description, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Text, y.Text, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.FolderText, y.FolderText, ComparerSortOrder.Ascending);
                    break;
            }

            return result;
        }
    }
}