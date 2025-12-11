using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technics.Properties;
using static Technics.Enums;
using static Technics.Database.Models;

namespace Technics
{
    public partial class Main
    {
        private async Task LoadTechAsync()
        {
            DebugWrite.Line("start");

            var status = ProgramStatus.Start(Status.LoadData);

            try
            {
                var folderList = await Database.Default.ListLoadAsync<Folder>();

                folderList = folderList.OrderBy(f => f.Id).ToList();

                var map = new Dictionary<long, TreeNodeFolder>();

                foreach (var folder in folderList)
                {
                    var folderNode = new TreeNodeFolder()
                    {
                        Folder = folder,
                        Text = folder.Text,
                    };

                    map[folder.Id] = folderNode;

                    if (folder.ParentId == Sql.NewId || !map.ContainsKey((long)folder.ParentId))
                    {
                        tvTechs.Nodes[0].Nodes.Add(folderNode);
                    }
                    else
                    {
                        map[(long)folder.ParentId].Nodes.Add(folderNode);
                    }
                }

                tvTechs.ExpandAll();

                Utils.Log.Info(string.Format(ResourcesLog.LoadListOk, typeof(Folder).Name, folderList.Count));
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseLoadListFail, e.Message);
            }
            finally
            {
                ProgramStatus.Stop(status);

                DebugWrite.Line("end");
            }
        }
    }
}