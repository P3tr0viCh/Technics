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
            public class Folder : BaseId
            {
                public long? ParentId { get; set; } = null;

                public string Text { get; set; } = null;

                public new void Clear()
                {
                    base.Clear();

                    ParentId = null;
                    Text = null;
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
                    Text = source.Text;
                }
            }
        }
    }
}
