using P3tr0viCh.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace Technics
{
    public partial class Database
    {
        public class Models
        {
            // ------------------------------------------------------------------------------------------------------------
            [Table(Tables.folders)]
            public class Folder : BaseText
            {
                public long? ParentId { get; set; } = null;

                public new void Clear()
                {
                    base.Clear();

                    ParentId = null;
                }

                public void Assign(Folder source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    ParentId = source.ParentId;
                }
            }

            // ------------------------------------------------------------------------------------------------------------
            [Table(Tables.techs)]
            public class Tech : BaseText
            {
                public long? FolderId { get; set; } = null;

                public new void Clear()
                {
                    base.Clear();

                    FolderId = null;
                }

                public void Assign(Tech source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    FolderId = source.FolderId;
                }
            }
        }
    }
}
