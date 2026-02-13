using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Interfaces;
using P3tr0viCh.Utils.Presenters;
using System;
using System.Windows.Forms;
using Technics.Interfaces;
using Technics.Presenters;
using static Technics.Presenters.PresenterStatusStripFrmList;

namespace Technics
{
    public partial class FrmList : Form, IFrmList, PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
    {
        public IMainForm MainForm => Owner as IMainForm;

        public DataGridView DataGridView => dataGridView;

        public ToolStrip ToolStrip => toolStrip;

        public StatusStrip StatusStrip => statusStrip;

        private IPresenterFrmList PresenterFrmList { get; set; }

        private readonly PresenterStatusStripFrmList statusStripPresenter;

        public FrmList()
        {
            InitializeComponent();

            statusStripPresenter = new PresenterStatusStripFrmList(this);
        }

        public static bool ShowDlg(Form owner, FrmListType listType)
        {
            using (var frm = new FrmList()
            {
                Owner = owner,
            })
            {
                frm.PresenterFrmList = PresenterFrmListFactory.PresenterFrmListInstance(frm, listType);

                frm.PresenterFrmList.ListChanged += frm.PresenterFrmList_ListChanged;

                frm.ShowDialog(owner);

                return frm.PresenterFrmList.Changed;
            }
        }

        private void PresenterFrmList_ListChanged(object sender, EventArgs e)
        {
            tsbtnChange.Enabled = tsbtnDelete.Enabled = !DataGridView.IsEmpty();

            statusStripPresenter.Count = DataGridView.Count();
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            statusStripPresenter.SelectedCount = DataGridView.SelectedCount();
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
            await PresenterFrmList.AddNewItemAsync();
        }

        private async void TsbtnChange_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.SelectedChangeAsync();
        }

        private async void TsbtnDelete_Click(object sender, EventArgs e)
        {
            await PresenterFrmList.SelectedDeleteAsync();
        }
    }
}