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
        private readonly WrapperCancellationTokenSource ctsList = new WrapperCancellationTokenSource();

        ~PresenterFrmListBase()
        {
            ctsList.Cancel();
        }

        protected virtual async Task<IEnumerable<T>> ListLoadAsync()
        {
            return await Database.Default.ListLoadAsync<T>();
        }

        protected virtual async Task ListItemSaveAsync(T value)
        {
            await Database.Default.ListItemSaveAsync(value);
        }

        protected virtual async Task ListItemDeleteAsync(IEnumerable<T> list)
        {
            await Database.Default.ListItemDeleteAsync(list);
        }

        private async Task PerformListLoadAsync()
        {
            DebugWrite.Line("start");

            ctsList.Start();

            var status = ProgramStatus.Default.Start(Status.LoadData);

            try
            {
                var list = await ListLoadAsync();

                bindingSource.DataSource = list;

                presenterDataGridView.Sort();

                bindingSource.Position = 0;

                PerformOnListChanged();

                Changed = false;
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
                ctsList.Finally();

                ProgramStatus.Default.Stop(status);
            }

            DebugWrite.Line("end");
        }

        private async Task PerformListItemSaveAsync(T value)
        {
            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                await ListItemSaveAsync(value);
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }
        }

        private async Task PerformListItemDeleteAsync(IEnumerable<T> list)
        {
            var status = ProgramStatus.Default.Start(Status.SaveData);

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