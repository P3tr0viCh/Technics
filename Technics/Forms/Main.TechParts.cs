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
        private async Task TechPartsLoadAsync(IEnumerable<TechModel> techs)
        {
            DebugWrite.Line("start");

            try
            {
                var techPartList = await ListLoadAsync<TechPartModel>(GetTechPartsSql(techs));

                bindingSourceTechParts.DataSource = techPartList;

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
            get => bindingSourceTechParts.Current as TechPartModel;
        }

        private void TechPartsListChanged()
        {
           tsbtnTechPartDelete.Enabled = tsbtnTechPartChange.Enabled = bindingSourceTechParts.Count > 0;
        }

        private async Task TechPartsChangeAsync(TechPartModel techPart)
        {
            //if (!FrmTechPart.ShowDlg(this, techPart)) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var isNew = techPart.IsNew;

                await Database.Default.ListItemSaveAsync(techPart);

                Utils.Log.Info(string.Format(ResourcesLog.ListItemSaveOk, nameof(TechPartModel)));

                if (isNew)
                {
                    bindingSourceTechParts.Insert(0, techPart);

                    bindingSourceTechParts.Position = 0;

                    TechPartsListChanged();
                }
                else
                {
                    dgvTechParts.Refresh();
                }

//                await MileagesUpdateMileageCommonAsync(techPart);
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

            if (!Msg.Question(Resources.QuestionTechPartDelete,
                    techPart.PartText,
                    techPart.DateTimeInstall.ToString(AppSettings.Default.FormatDateTime))) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                await ListItemDeleteAsync(techPart);

                bindingSourceTechParts.Remove(techPart);

                TechPartsListChanged();

//                await TechPartsUpdateMileageCommonAsync(techPart);
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