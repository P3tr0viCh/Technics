using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.ProgramStatus;

namespace Technics.Presenters
{
    internal abstract partial class PresenterFrmListBase<T>
    {
        private async Task ListLoadAsync()
        {
            DebugWrite.Line("start");

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                var list = await Database.Default.ListLoadAsync<T>();

                BindingSource.DataSource = list;

                presenterDataGridView.Sort();

                BindingSource.Position = 0;

                PerformOnListChanged();

                Changed = false;
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseLoadFail, e.Message);
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }

            DebugWrite.Line("end");
        }

        protected virtual async Task ListItemSaveAsync(T value)
        {
            await Database.Default.ListItemSaveAsync(value);
        }

        private async Task PerformListItemSaveAsync(T value)
        {
            var status = ProgramStatus.Default.Start(Status.SaveDatа);

            try
            {
                await ListItemSaveAsync(value);
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }
        }

        protected virtual async Task ListItemDeleteAsync(IEnumerable<T> list)
        {
            await Database.Default.ListItemDeleteAsync(list);
        }

        private async Task PerformListItemDeleteAsync(IEnumerable<T> list)
        {
            var status = ProgramStatus.Default.Start(Status.SaveDatа);

            try
            {
                await ListItemDeleteAsync(list);
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }
        }
    }
}