using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
        internal readonly PresenterDataGridViewTechParts presenterDataGridViewTechParts;

        private async Task TechPartsLoadAsync(IEnumerable<TechModel> techs)
        {
            DebugWrite.Line("start");

            try
            {
                var list = await Database.Default.TechPartsLoadAsync(techs);

                bindingSourceTechParts.DataSource = list;

                presenterDataGridViewTechParts.Sort();

                bindingSourceTechParts.Position = 0;

                TechPartsListChanged();

                Utils.Log.Info(ResourcesLog.LoadOk);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private IEnumerable<TechPartModel> TechPartList => bindingSourceTechParts.Cast<TechPartModel>();

        public TechPartModel TechPartSelected
        {
            get => presenterDataGridViewTechParts.Selected;
            set => presenterDataGridViewTechParts.Selected = value;
        }

        public IEnumerable<TechPartModel> TechPartSelectedList
        {
            get => presenterDataGridViewTechParts.SelectedList;
            set => presenterDataGridViewTechParts.SelectedList = value;
        }

        private void TechPartsListChanged()
        {
            tsbtnTechPartDelete.Enabled = tsbtnTechPartChange.Enabled = bindingSourceTechParts.Count > 0;

            presenterStatusStrip.TechPartCount = bindingSourceTechParts.Count;
        }

        private void TechPartsUpdateChanged(UpdateModel changes)
        {
            DebugWrite.Line(changes.TechParts.Count);

            var techParts = TechPartList.ToList();

            for (var i = 0; i < techParts.Count; i++)
            {
                var changed = changes.TechParts.Where(item => item.Id == techParts[i].Id).FirstOrDefault();

                if (changed == null) continue;

                techParts[i].Mileage = changed.Mileage;
                techParts[i].MileageCommon = changed.MileageCommon;

                bindingSourceTechParts.ResetItem(i);
            }
        }

        private async Task TechPartsChangeAsync(TechPartModel techPart)
        {
            if (!FrmTechPart.ShowDlg(this, techPart)) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var isNew = techPart.IsNew;

                var changes = await Database.Default.TechPartSaveAsync(techPart);

                if (isNew)
                {
                    var pos = bindingSourceTechParts.Add(techPart);

                    bindingSourceTechParts.Position = pos;
                }
                else
                {
                    dgvTechParts.Refresh();
                }

                presenterDataGridViewTechParts.Sort();

                dgvTechParts.SetSelectedRows(techPart);

                TechPartsUpdateChanged(changes);

                TechPartsListChanged();
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemSaveFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }

            dgvTechParts.Focus();
        }

        private async Task TechPartsAddNewAsync()
        {
            var techPart = new TechPartModel()
            {
                DateTimeInstall = DateTime.Now,
            };

            var tech = SelectedTech;

            if (tech != null)
            {
                techPart.TechId = tech.Id;
                techPart.TechText = tech.Text;
            }

            await TechPartsChangeAsync(techPart);
        }

        private async Task TechPartsChangeSelectedAsync()
        {
            var techPart = TechPartSelected;

            if (techPart == null) return;

            dgvTechParts.SetSelectedRows(techPart);

            await TechPartsChangeAsync(techPart);
        }

        private async Task TechPartsDeleteSelectedAsync()
        {
            var techParts = TechPartSelectedList;

            if (!techParts.Any()) return;

            dgvTechParts.SetSelectedRows(techParts);

            if (!Utils.Msg.Question(techParts)) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var updated = await Database.Default.TechPartDeleteAsync(techParts);

                foreach (var techPart in techParts)
                {
                    bindingSourceTechParts.Remove(techPart);
                }

                dgvTechParts.SetSelectedRows(dgvTechParts.GetSelected<TechPartModel>());

                TechPartsUpdateChanged(updated);

                TechPartsListChanged();
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemDeleteFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);
            }

            dgvTechParts.Focus();
        }
    }
}