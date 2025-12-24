using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Filter;
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

                await TechPartsUpdateMileagesAsync(list);

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

        private async Task TechPartsUpdateMileagesAsync(IEnumerable<TechPartModel> list)
        {
            DebugWrite.Line($"count={list.Count()}");

            if (!list.Any()) return;

            await Database.Default.LoadTechPartMileagesAsync(list);

            dgvTechParts.Refresh();
        }

        private async Task TechPartsUpdateMileagesAsync(TechPartModel techPart)
        {
            var list = TechPartList.Where(item => item.PartId == techPart.PartId);

            await TechPartsUpdateMileagesAsync(list);
        }

        private async Task TechPartsUpdateMileagesAsync(MileageModel mileage)
        {
            var list = TechPartList.Where(item => item.TechId == mileage.TechId);

            var parts = list.Select(item => item.PartId);

            list = TechPartList.Where(item => parts.Contains(item.PartId));

            await TechPartsUpdateMileagesAsync(list);
        }

        private async Task TechPartsChangeAsync(TechPartModel techPart)
        {
            if (!FrmTechPart.ShowDlg(this, techPart)) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var isNew = techPart.IsNew;

                await Database.Default.ListItemSaveAsync(techPart);

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

                await TechPartsUpdateMileagesAsync(techPart);

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

                await TechPartsUpdateMileagesAsync(techPart);

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