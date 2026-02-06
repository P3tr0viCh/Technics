using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.ProgramStatus;

namespace Technics
{
    public partial class Main
    {
        internal readonly WrapperCancellationTokenSource ctsTechsLoad = new WrapperCancellationTokenSource();

        private void AddTechsRoot()
        {
            tvTechs.Nodes.Add(new TreeNodeFolder()
            {
                Text = Resources.TextTechAll
            });
        }

        private bool TechsCanChangeItem
        {
            set
            {
                tsbtnTechChange.Enabled = value;
                tsbtnTechDelete.Enabled = value;

                miTechChange.Enabled = value;
                miTechDelete.Enabled = value;
            }
        }

        private IEnumerable<TechModel> selectedTechList;
        private IEnumerable<TechModel> SelectedTechList => selectedTechList;

        private async Task TechsSelectedChangedAsync()
        {
            var selected = tvTechs.SelectedNode as TreeNodeBase;

            TechsCanChangeItem = selected?.Model.IsNew == false;

            miTechAdd.Visible = selected is TreeNodeFolder;

            selectedTechList = GetTechList(tvTechs.SelectedNode);

            await UpdateDataAsync(DataLoad.Mileages | DataLoad.TechParts);

#if DEBUG
            var path = string.Empty;

            if (selected is TreeNodeTech techNode)
            {
                var folderId = techNode.Tech.FolderId;

                var folder = Lists.Default.Folders.Find(folderId);

                if (folder != null)
                {
                    path = folder.Path + Path.DirectorySeparatorChar;
                }

                path += techNode.Tech.Text;
            }
            else
            {
                if (selected is TreeNodeFolder folderNode)
                {
                    path = folderNode.Folder.Path;
                }
            }

            DebugWrite.Line($"selected path: {Path.DirectorySeparatorChar}{path}");
#endif
        }

        private async Task TechsLoadAsync()
        {
            DebugWrite.Line("start");

            ctsTechsLoad.Start();

            try
            {
                tvTechs.Nodes[0].Nodes.Clear();

                var folders = await Database.Default.ListLoadAsync<FolderModel>();

                if (ctsTechsLoad.IsCancellationRequested) return;

                folders = folders.OrderBy(f => f.Text);

                Lists.Default.Folders = new FolderList(folders);

                folders = folders.OrderBy(f => f.Id);

                var folderNodes = new Dictionary<long, TreeNodeFolder>
                {
                    [Sql.NewId] = tvTechs.Nodes[0] as TreeNodeFolder
                };

                foreach (var folder in folders)
                {
                    var folderNode = new TreeNodeFolder(folder);

                    folderNodes[folder.Id] = folderNode;

                    var parentId = folder.ParentId ?? Sql.NewId;

                    folderNodes[parentId].Nodes.Add(folderNode);
                }

                var techs = await Database.Default.ListLoadAsync<TechModel>();

                if (ctsTechsLoad.IsCancellationRequested) return;

                Lists.Default.Techs = new TechList(techs.OrderBy(tech => tech.Text));

                foreach (var tech in Lists.Default.Techs)
                {
                    var techNode = new TreeNodeTech(tech);
                    
                    var folderId = tech.FolderId ?? Sql.NewId;

                    folderNodes[folderId].Nodes.Add(techNode);
                }

                tvTechs.ExpandAll();

                tvTechs.SelectedNode = tvTechs.Nodes[0];

                await TechsSelectedChangedAsync();

                Utils.Log.Info(ResourcesLog.LoadOk);
            }
            catch (TaskCanceledException e)
            {
                DebugWrite.Error(e);
            }
            finally
            {
                ctsTechsLoad.Finally();

                DebugWrite.Line("end");
            }
        }

        private TreeNodeFolder GetParent(TreeNode node)
        {
            var result = node is TreeNodeTech ? node.Parent : node;

            return result as TreeNodeFolder;
        }

        private TechModel SelectedTech => tvTechs.SelectedNode is TreeNodeTech techNode ? techNode.Tech : null;

        private List<TechModel> GetTechList(TreeNode parent)
        {
            var result = new List<TechModel>();

            if (parent is null)
            {
                return result;
            }

            if (parent is TreeNodeTech parentNodeTech)
            {
                result.Add(parentNodeTech.Tech);
            }

            foreach (TreeNode node in parent.Nodes)
            {
                if (node is TreeNodeTech nodeTech)
                {
                    result.Add(nodeTech.Tech);
                }
                else
                {
                    if (node is TreeNodeFolder nodeFolder)
                    {
                        result.AddRange(GetTechList(nodeFolder));
                    }
                }
            }

            return result;
        }

        private void TechsAddNew(TreeNode parent, TreeNode value)
        {
            parent.Nodes.Add(value);

            parent.Expand();

            tvTechs.SelectedNode = value;
        }

        private async Task TechsAddNewItemAsync(BaseId value)
        {
            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                var parent = GetParent(tvTechs.SelectedNode);

                TreeNode node = null;

                if (value is FolderModel folder)
                {
                    folder.ParentId = parent.Folder.Id;

                    node = new TreeNodeFolder(folder);

                    await Database.Default.ListItemSaveAsync(folder);

                    Lists.Default.Folders.Add(folder);
                }
                else
                {
                    if (value is TechModel tech)
                    {
                        tech.FolderId = parent.Folder.Id;

                        node = new TreeNodeTech(tech);

                        await Database.Default.ListItemSaveAsync(tech);

                        Lists.Default.Techs.Add(tech);
                    }
                }

                TechsAddNew(parent, node);
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemSaveFail, e.Message);
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }

            tvTechs.Focus();
        }

        private async Task TechsAddNewFolderAsync()
        {
            var text =
#if DEBUG
                $"Folder {Str.Random(3)}";
#else
                string.Empty;
#endif

            if (!Utils.TextInputBoxShow(ref text, Resources.TitleFolder)) return;

            var folder = new FolderModel
            {
                Text = text
            };

            await TechsAddNewItemAsync(folder);
        }

        private async Task TechsAddNewTechAsync()
        {
            var text =
#if DEBUG
                $"Tech {Str.Random(3)}";
#else
                string.Empty;
#endif

            if (!Utils.TextInputBoxShow(ref text, Resources.TitleTech)) return;

            var tech = new TechModel
            {
                Text = text
            };

            await TechsAddNewItemAsync(tech);
        }

        private async Task TechsChangeSelectedAsync()
        {
            if (!(tvTechs.SelectedNode is TreeNodeBase changedNode)) return;

            var changedModel = changedNode.Model;

            var text = changedModel.Text;

            var caption = string.Empty;

            if (changedModel is FolderModel)
            {
                caption = Resources.TitleTech;
            }
            else
            {
                if (changedModel is TechModel)
                {
                    caption = Resources.TitleTech;
                }
            }

            if (!Utils.TextInputBoxShow(ref text, caption)) return;

            changedModel.Text = text;

            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                if (changedModel is FolderModel folder)
                {
                    await Database.Default.ListItemSaveAsync(folder);

                    Lists.Default.Folders.Replace(folder);
                }
                else
                {
                    if (changedModel is TechModel tech)
                    {
                        await Database.Default.ListItemSaveAsync(tech);

                        Lists.Default.Techs.Remove(tech);

                        Lists.Default.Techs.Add(tech);

                        UpdateTechText(bindingSourceMileages, tech);
                        UpdateTechText(bindingSourceTechParts, tech);
                    }
                }

                changedNode.Model = changedModel;
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemSaveFail, e.Message);
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }

            tvTechs.Focus();
        }

        private void UpdateTechText(BindingSource bindingSource, TechModel tech)
        {
            var list = bindingSource.Cast<BaseTechId>().ToList();

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].TechId != tech.Id) continue;

                list[i].TechText = tech.Text;

                bindingSource.ResetItem(i);
            }
        }

        private async Task TechsDeleteSelectedAsync()
        {
            if (!(tvTechs.SelectedNode is TreeNodeBase deletedNode)) return;

            var deletedModel = deletedNode.Model;

            if (deletedModel.IsNew) return;

            string question;

            if (deletedNode is TreeNodeTech)
            {
                question = Resources.QuestionItemLinkedDelete;
            }
            else
            {
                question = deletedNode.Nodes.Count == 0 ?
                    Resources.QuestionFolderDelete :
                    Resources.QuestionFolderDeleteNotEmpty;
            }

            if (!Utils.Msg.Question(question, deletedModel.Text)) return;

            var status = ProgramStatus.Default.Start(Status.SaveData);

            try
            {
                var deletedNodeParent = deletedNode.Parent as TreeNodeFolder;

                if (deletedNode.Nodes.Count != 0)
                {
                    var parentNodeFolder = deletedNodeParent.Model as FolderModel;

                    var movedNodes = deletedNode.Nodes.Cast<TreeNodeBase>().ToArray();

                    foreach (var node in movedNodes)
                    {
                        if (node is TreeNodeFolder nodeFolder)
                        {
                            nodeFolder.Folder.ParentId = parentNodeFolder.Id;

                            await Database.Default.ListItemSaveAsync(nodeFolder.Folder);
                        }
                        else
                        {
                            if (node is TreeNodeTech nodeTech)
                            {
                                nodeTech.Tech.FolderId = parentNodeFolder.Id;

                                await Database.Default.ListItemSaveAsync(nodeTech.Tech);
                            }
                        }

                        deletedNode.Nodes.Remove(node);

                        deletedNodeParent.Nodes.Add(node);
                    }
                }

                if (deletedModel is FolderModel folder)
                {
                    await Database.Default.ListItemDeleteAsync(folder);

                    Lists.Default.Folders.Remove(folder);
                }
                else
                {
                    if (deletedModel is TechModel tech)
                    {
                        await Database.Default.TechDeleteAsync(tech);

                        Lists.Default.Techs.Remove(tech);
                    }
                }

                deletedNode.Remove();

                tvTechs.SelectedNode = deletedNodeParent;
            }
            catch (Exception e)
            {
                Utils.Log.Query(e);

                Utils.Log.Error(e);

                Utils.Msg.Error(Resources.MsgDatabaseListItemDeleteFail, e.Message);
            }
            finally
            {
                ProgramStatus.Default.Stop(status);
            }

            tvTechs.Focus();
        }
    }
}