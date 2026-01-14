using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;
using static Technics.ProgramStatus;

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
            if (load == default)
            {
                load = DataLoad.Techs |
                       DataLoad.Mileages |
                       DataLoad.TechParts;
            }

            DebugWrite.Line($"Loading data {load}");

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                SelfChange = true;

                await Task.Delay(100);

                if (load.HasFlag(DataLoad.Techs))
                {
                    await TechsLoadAsync();
                }

                if (load.HasFlag(DataLoad.Mileages))
                {
                    var selectedList = MileageSelectedList;

                    await MileagesLoadAsync(SelectedTechList);

                    MileageSelectedList = selectedList;
                }

                if (load.HasFlag(DataLoad.TechParts))
                {
                    var selectedList = TechPartSelectedList;

                    await TechPartsLoadAsync(SelectedTechList);

                    TechPartSelectedList = selectedList;
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

                ProgramStatus.Default.Stop(status);

                DebugWrite.Line("end");
            }
        }
    }
}