using P3tr0viCh.Utils;
using System;
using System.Windows.Forms;
using static Technics.PresenterStatusStripFrmList;

namespace Technics
{
    public partial class FrmList : Form, IFrmList, PresenterStatusStrip<StatusLabel>.IPresenterStatusStrip
    {
        public IMainForm MainForm => Owner as IMainForm;

        public DataGridView DataGridView => dataGridView;

        public BindingSource BindingSource => bindingSource;

        public ToolStrip ToolStrip => toolStrip;

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
                
                frm.PresenterFrmList.OnListChanged += frm.PresenterFrmList_OnListChanged;

                frm.ShowDialog(owner);

                return frm.PresenterFrmList.Changed;
            }
        }

        private void PresenterFrmList_OnListChanged()
        {
            tsbtnChange.Enabled = tsbtnDelete.Enabled = PresenterFrmList.Count > 0;

            statusStripPresenter.Count = PresenterFrmList.Count;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            statusStripPresenter.SelectedCount = PresenterFrmList.SelectedCount;
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
    }
}