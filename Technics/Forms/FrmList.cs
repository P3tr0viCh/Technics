using P3tr0viCh.Utils;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Technics.Forms;
using static Technics.Enums;
using static Technics.PresenterStatusStripList;

namespace Technics
{
    public partial class FrmList : Form, IFrmList, PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
    {
        public IMainForm MainForm => Owner as IMainForm;

        public DataGridView DataGridView => dataGridView;

        public BindingSource BindingSource => bindingSource;

        public ToolStrip ToolStrip => toolStrip;

        private IPresenterFrmList PresenterFrmList { get; set; }

        private readonly PresenterStatusStripList statusStripPresenter;

        public FrmList()
        {
            InitializeComponent();

            statusStripPresenter = new PresenterStatusStripList(this);
        }

        public static bool ShowDlg(Form owner, ListType listType)
        {
            using (var frm = new FrmList()
            {
                Owner = owner,
            })
            {
                Utils.Log.WriteFormOpen(frm);

                frm.PresenterFrmList = PresenterFrmListFactory.PresenterFrmListInstance(frm, listType);

                try
                {
                    Utils.Log.Info($"ListType = {frm.PresenterFrmList.ListType}");

                    frm.ShowDialog(owner);

                    return frm.PresenterFrmList.Changed;
                }
                finally
                {
                    Utils.Log.WriteFormClose(frm);
                }
            }
        }

        private async void FrmList_Load(object sender, EventArgs e)
        {
            PresenterFrmList.OnListChanged += PresenterFrmList_OnListChanged;

            await PresenterFrmList.FormLoadAsync();
        }

        private void FrmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            PresenterFrmList.FormClosing();
        }

        private void PresenterFrmList_OnListChanged()
        {
            tsbtnChange.Enabled = tsbtnDelete.Enabled = PresenterFrmList.Count > 0;
        }

        ToolStripStatusLabel PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip.GetLabel(StatusLabel label)
        {
            switch (label)
            {
                case StatusLabel.Count: return slCount;
                case StatusLabel.SelectedCount: return slSelectedCount;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void TsbtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void TsbtnAdd_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.ListItemAddNewAsync();
        }

        private async void TsbtnChange_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.ListItemChangeSelectedAsync();
        }

        private async void TsbtnDelete_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.ListItemDeleteSelectedAsync();
        }

        private async void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            await PresenterFrmList.ListItemChangeSelectedAsync();
        }

        private void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            statusStripPresenter.Count = PresenterFrmList.Count;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            statusStripPresenter.SelectedCount = PresenterFrmList.SelectedCount;
        }
    }
}