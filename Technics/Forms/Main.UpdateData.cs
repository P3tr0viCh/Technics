using P3tr0viCh.Utils;
using System;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
        private async Task UpdateDataAsync()
        {
            var status = ProgramStatus.Start(Status.LoadData);

            try
            {
                await LoadTechsAsync();

                await LoadMileagesAsync();
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