using P3tr0viCh.Database;
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
                var list = techs.Count() > 0
                    ? await Database.Default.ListLoadAsync<TechPartModel>(Database.Default.GetTechPartsSql(techs))
                    : Enumerable.Empty<TechPartModel>();

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

        public TechPartModel TechPartSelected => bindingSourceTechParts.Current as TechPartModel;

        private void TechPartsListChanged()
        {
            tsbtnTechPartDelete.Enabled = tsbtnTechPartChange.Enabled = bindingSourceTechParts.Count > 0;

            statusStripPresenter.TechPartCount = bindingSourceTechParts.Count;
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
            var techPart = TechPartSelected;

            if (techPart == null) return;

            dgvTechParts.SetSelectedRows(techPart);

            if (!Utils.Msg.Question(Resources.QuestionTechPartDelete,
                    techPart.PartText,
                    techPart.DateTimeInstall.ToString(AppSettings.Default.FormatDateTime))) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                await Database.Default.ListItemDeleteAsync(techPart);

                bindingSourceTechParts.Remove(techPart);

                dgvTechParts.SetSelectedRows(dgvMileages.GetSelected<TechPartModel>());

                //await TechPartsUpdateMileagesAsync(techPart);

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