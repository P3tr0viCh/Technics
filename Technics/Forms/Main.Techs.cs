using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technics.Models;
using Technics.Properties;
using static Technics.Database.Models;
using static Technics.Enums;

namespace Technics
{
    public partial class Main
    {
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

        private void SelectedChanged()
        {
            var selected = tvTechs.SelectedNode as TreeNodeBase;

            TechsCanChangeItem = selected?.Model.IsNew == false;

            miTechAdd.Visible = selected is TreeNodeFolder;
        }

        private async Task LoadTechsAsync()
        {
            DebugWrite.Line("start");

            try
            {
                var folderList = await ListLoadAsync<Folder>();

                folderList = folderList.OrderBy(f => f.Id).ToList();

                var folderNodes = new Dictionary<long, TreeNodeFolder>
                {
                    [Sql.NewId] = tvTechs.Nodes[0] as TreeNodeFolder
                };

                foreach (var folder in folderList)
                {
                    var folderNode = new TreeNodeFolder(folder);

                    folderNodes[folder.Id] = folderNode;

                    folderNodes[(long)folder.ParentId].Nodes.Add(folderNode);
                }

                var techList = await ListLoadAsync<Tech>();

                foreach (var tech in techList)
                {
                    var techNode = new TreeNodeTech(tech);

                    folderNodes[(long)tech.FolderId].Nodes.Add(techNode);
                }

                tvTechs.ExpandAll();

                Utils.Log.Info(ResourcesLog.LoadTechOk);
            }
            finally
            {
                DebugWrite.Line("end");
            }
        }

        private TreeNodeFolder GetParent(TreeNode node)
        {
            var result = node is TreeNodeTech ? node.Parent : node;

            return result as TreeNodeFolder;
        }

        private void TechsAddNew(TreeNode parent, TreeNode value)
        {
            parent.Nodes.Add(value);

            parent.Expand();

            tvTechs.SelectedNode = value;
        }

        private async Task TechsAddNewItemAsync(TreeNodeBase node)
        {
            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var parent = GetParent(tvTechs.SelectedNode);

                if (node is TreeNodeFolder nodeFolder)
                {
                    nodeFolder.Folder.ParentId = parent.Folder.Id;

                    await Database.Default.ListItemSaveAsync(nodeFolder.Folder);
                }
                else
                {
                    if (node is TreeNodeTech nodeTech)
                    {
                        nodeTech.Tech.FolderId = parent.Folder.Id;

                        await Database.Default.ListItemSaveAsync(nodeTech.Tech);
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
                ProgramStatus.Stop(status);
            }

            tvTechs.Focus();
        }

        private async Task TechsAddNewFolderAsync()
        {
            var node = new TreeNodeFolder
            {
                Text = $"Folder {Str.Random(3)}"
            };

            await TechsAddNewItemAsync(node);
        }

        private async Task TechsAddNewTechAsync()
        {
            var node = new TreeNodeTech
            {
                Text = $"Item {Str.Random(3)}"
            };

            await TechsAddNewItemAsync(node);
        }

        private async Task TechsDeleteSelected()
        {
            var deletedNode = (TreeNodeBase)tvTechs.SelectedNode;

            if (deletedNode == null) return;

            var deletedModel = deletedNode.Model;

            if (deletedModel.IsNew) return;

            var question = deletedNode.Nodes.Count == 0 ?
                Resources.QuestionDeleteListItemEmpty :
                Resources.QuestionDeleteListItem;

            if (!Msg.Question(question, deletedModel.Text)) return;

            var status = ProgramStatus.Start(Status.SaveDatа);

            try
            {
                var deletedNodeParent = deletedNode.Parent as TreeNodeFolder;

                if (deletedNode.Nodes.Count != 0)
                {
                    var parentNodeFolder = deletedNodeParent.Model as Folder;

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

                if (deletedModel is Folder folder)
                {
                    await Database.Default.ListItemDeleteAsync(folder);
                }
                else
                {
                    if (deletedModel is Tech tech)
                    {
                        await Database.Default.ListItemDeleteAsync(tech);
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
                ProgramStatus.Stop(status);
            }

            tvTechs.Focus();
        }
    }
}