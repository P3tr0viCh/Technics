using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using Technics.Properties;
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
            if (load == default)
            {
                load = DataLoad.Techs |
                       DataLoad.Mileages |
                       DataLoad.TechParts;
            }

            DebugWrite.Line($"Loading data {load}");

            var status = ProgramStatus.Start(Status.LoadData);

            try
            {
                if (load.HasFlag(DataLoad.Techs))
                {
                    await TechsLoadAsync();
                }

                if (load.HasFlag(DataLoad.Mileages))
                {
                    await MileagesLoadAsync(SelectedTechList);
                }

                if (load.HasFlag(DataLoad.TechParts))
                {
                    await TechPartsLoadAsync(SelectedTechList);
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
                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }
    }
}