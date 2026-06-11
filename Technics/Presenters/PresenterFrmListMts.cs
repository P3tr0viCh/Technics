using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Comparers;
using P3tr0viCh.Utils.EventArguments;
using P3tr0viCh.Utils.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Technics.Forms;
using Technics.Properties;
using static Technics.Database.Models;

namespace Technics.Presenters
{
    internal class PresenterFrmListMts : PresenterFrmList<MtModel>
    {
        public override FrmListType ListType => FrmListType.Maintenance;

        public PresenterFrmListMts(IFrmList frmList) : base(frmList)
        {
            ItemsChangeDialog += PresenterFrmListMaintenance_ItemsChangeDialog;
            ItemsDeleteDialog += PresenterFrmListMaintenance_ItemsDeleteDialog;
        }

        protected override MtModel GetNewItem()
        {
#if DEBUG
            var item = base.GetNewItem();

            item.Text = $"Maintenance {Str.Random(5)}";

            return item;
#else
            return base.GetNewItem();
#endif
        }

        protected override string FormTitle => Resources.TitleListMaintenance;

        protected override void LoadFormState()
        {
            base.LoadFormState();

            PresenterDataGridView.SortColumn = nameof(MtModel.Text);
        }

        private async Task PresenterFrmListMaintenance_ItemsChangeDialog(object sender, ItemsDialogEventArgs<MtModel> e)
        {
            e.Ok = FrmMt.ShowDlg(Form, e.Values.First());

            await Task.CompletedTask;
        }

        private async Task PresenterFrmListMaintenance_ItemsDeleteDialog(object sender, ItemsDialogEventArgs<MtModel> e)
        {
            e.Ok = Utils.Msg.Question(e.Values);

            await Task.CompletedTask;
        }

        protected async override Task<IEnumerable<MtModel>> DatabaseListLoadAsync(CancellationToken cancellationToken)
        {
            return await Database.Default.ListLoadAsync<MtModel>(ResourcesSql.SelectMts);
        }

        protected override async Task DatabaseListItemsDeleteAsync(IEnumerable<MtModel> list)
        {
            await Database.Default.MtDeleteAsync(list);
        }

        protected override void UpdateColumns()
        {
            base.UpdateColumns();

            FrmList.DataGridView.Columns[nameof(MtModel.FolderText)].DisplayIndex = 0;
            FrmList.DataGridView.Columns[nameof(MtModel.Text)].DisplayIndex = 1;

            FrmList.DataGridView.Columns[nameof(MtModel.Text)].HeaderText = ResourcesColumnHeader.Text;
            FrmList.DataGridView.Columns[nameof(MtModel.FolderText)].HeaderText = ResourcesColumnHeader.Folder;
            FrmList.DataGridView.Columns[nameof(MtModel.Description)].HeaderText = ResourcesColumnHeader.Description;

            FrmList.DataGridView.Columns[nameof(MtModel.FolderId)].Visible = false;
            FrmList.DataGridView.Columns[nameof(MtModel.Text)].Visible = true;
        }

        public override int Compare(MtModel x, MtModel y, string dataPropertyName, ComparerSortOrder sortOrder)
        {
            var result = 0;

            switch (dataPropertyName)
            {
                case nameof(MtModel.FolderText):
                    result = EmptyStringComparer.Default.Compare(x.FolderText, y.FolderText, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Text, y.Text, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Description, y.Description, ComparerSortOrder.Ascending);
                    break;
                case nameof(MtModel.Text):
                    result = EmptyStringComparer.Default.Compare(x.Text, y.Text, sortOrder);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.FolderText, y.FolderText, ComparerSortOrder.Ascending);
                    if (result == 0)
                        result = EmptyStringComparer.Default.Compare(x.Description, y.Description, ComparerSortOrder.Ascending);
                    break;
                case nameof(MtModel.Description):
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