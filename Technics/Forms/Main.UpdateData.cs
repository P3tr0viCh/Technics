using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
        [Flags]
        public enum DataLoad
        {
            Techs = 1,
            Mileages = 2,
            TechParts = 4,
        }

        private async Task UpdateDataAsync(DataLoad load = default)
        {
            var saveSelecteds = true;

            if (load == default)
            {
                load = DataLoad.Techs |
                       DataLoad.Mileages |
                       DataLoad.TechParts;
                
                saveSelecteds = false;
            }

            DebugWrite.Line($"Loading data {load}");

            var status = ProgramStatus.Start(Status.LoadData);

            try
            {
                SelfChange = true;

                if (load.HasFlag(DataLoad.Techs))
                {
                    await TechsLoadAsync();
                }

                if (load.HasFlag(DataLoad.Mileages))
                {
                    IEnumerable<MileageModel> selectedList = null;

                    if (saveSelecteds)
                    {
                        selectedList = MileageSelectedList;
                    }

                    await MileagesLoadAsync(SelectedTechList);

                    if (selectedList != null)
                    {
                        MileageSelectedList = selectedList;
                    }
                }

                if (load.HasFlag(DataLoad.TechParts))
                {
                    IEnumerable<TechPartModel> selectedList = null;

                    if (saveSelecteds)
                    {
                        selectedList = TechPartSelectedList;
                    }

                    await TechPartsLoadAsync(SelectedTechList);

                    if (selectedList != null)
                    {
                        TechPartSelectedList = selectedList;
                    }
                }
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseLoadFail, e.Message);
            }
            finally
            {
                SelfChange = false;

                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }
    }
}