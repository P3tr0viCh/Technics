using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Enums;

namespace Technics
{
	internal abstract partial class PresenterFrmListBase<T>
	{
		private async Task ListLoadAsync()
		{
			DebugWrite.Line("start");

			var status = FrmList.MainForm.ProgramStatus.Start(Status.LoadData);

			try
			{
				var list = await Database.Default.ListLoadAsync<T>();

				FrmList.BindingSource.DataSource = list.ToBindingList();

				Utils.Log.Info(string.Format(ResourcesLog.LoadListOk, typeof(T).Name, list.Count));
			}
			catch (Exception e)
			{
				Utils.Log.Query(e);

				Utils.Log.Error(e);

				Utils.Msg.Error(Resources.MsgDatabaseLoadFail, e.Message);
			}
			finally
			{
				FrmList.MainForm.ProgramStatus.Stop(status);
			}

			DebugWrite.Line("end");
		}

		private async Task ListItemSaveAsync(T value)
		{
			var status = FrmList.MainForm.ProgramStatus.Start(Status.SaveDatа);

			try
			{
				await Database.Default.ListItemSaveAsync(value);
			}
			catch (Exception e)
			{
				Utils.Log.Query(e);

				Utils.Log.Error(e);

				Utils.Msg.Error(Resources.MsgDatabaseListItemSaveFail, e.Message);
			}
			finally
			{
				FrmList.MainForm.ProgramStatus.Stop(status);
			}
		}

		private async Task ListItemDeleteAsync(List<T> list)
		{
			var status = FrmList.MainForm.ProgramStatus.Start(Status.SaveDatа);

			try
			{
				await Database.Default.ListItemDeleteAsync(list);
			}
			catch (Exception e)
			{
				Utils.Log.Query(e);

				Utils.Log.Error(e);

				Utils.Msg.Error(Resources.MsgDatabaseListItemDeleteFail, e.Message);
			}
			finally
			{
				FrmList.MainForm.ProgramStatus.Stop(status);
			}
		}
	}
}