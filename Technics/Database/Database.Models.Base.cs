using Dapper.Contrib.Extensions;
using P3tr0viCh.Database;
using Technics.Properties;
using static Technics.Database.Interfaces;

namespace Technics
{
    public partial class Database
    {
        public partial class Models
        {
            public abstract class BaseTextDescription : BaseText
            {
                public string Description { get; set; } = null;

                public override void Clear()
                {
                    base.Clear();

                    Description = null;
                }

                public void Assign(BaseTextDescription source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    Description = source.Description;
                }
            }

            public abstract class BaseTextDescriptionFolder : BaseTextDescription
            {
                private long? folderId = null;
                public long? FolderId
                {
                    get => folderId;
                    set => folderId = value != Sql.NewId ? value : null;
                }

                [Computed]
                [Write(false)]
                public string FolderText { get; set; } = null;

                public override void Clear()
                {
                    base.Clear();

                    FolderId = null;
                    FolderText = null;
                }

                public void Assign(BaseTextDescriptionFolder source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    FolderId = source.FolderId;
                    FolderText = source.FolderText;
                }
            }

            public abstract class BaseTextDescriptionFolderState : BaseTextDescriptionFolder
            {
                public bool State { get; set; } = false;

                [Computed]
                [Write(false)]
                public bool AvailableForUse => State != true;

                [Computed]
                [Write(false)]
                public string StateAsString => State ? Resources.TextCellX : string.Empty;

                public override void Clear()
                {
                    base.Clear();

                    State = false;
                }

                public void Assign(BaseTextDescriptionFolderState source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    State = source.State;
                }
            }

            public abstract class BaseTechId : BaseId, ITechId
            {
                private long? techId = null;
                public long? TechId
                {
                    get => techId;
                    set
                    {
                        if (value == Sql.NewId)
                        {
                            value = null;

                            TechText = null;
                        }

                        techId = value;
                    }
                }

                [Computed]
                [Write(false)]
                public string TechText { get; set; } = default;

                public override void Clear()
                {
                    base.Clear();

                    TechId = null;
                    TechText = default;
                }

                public void Assign(BaseTechId source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    TechId = source.TechId;
                    TechText = source.TechText;
                }
            }
        }
    }
}