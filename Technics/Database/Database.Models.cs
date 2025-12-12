using Dapper.Contrib.Extensions;
using P3tr0viCh.Database;
using System;

namespace Technics
{
    public partial class Database
    {
        public class Models
        {
            // ------------------------------------------------------------------------------------------------------------
            [Table(Tables.folders)]
            public class FolderModel : BaseText
            {
                public long? ParentId { get; set; } = null;

                public new void Clear()
                {
                    base.Clear();

                    ParentId = null;
                }

                public void Assign(FolderModel source)
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
            public class TechModel : BaseText
            {
                public long? FolderId { get; set; } = null;

                public new void Clear()
                {
                    base.Clear();

                    FolderId = null;
                }

                public void Assign(TechModel source)
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

            // ------------------------------------------------------------------------------------------------------------
            [Table(Tables.mileages)]
            public class MileageModel : BaseId
            {
                public long? TechId { get; set; } = null;
                
                [Write(false)]
                [Computed]
                public string TechText { get; set; } = default;

                public DateTime DateTime { get; set; } = default;

                public double Mileage { get; set; } = default;

                [Write(false)]
                [Computed]
                public double MileageCommon { get; set; } = default;

                public string Description { get; set; } = default;

                public new void Clear()
                {
                    base.Clear();

                    TechId = null;
                    TechText = default;

                    DateTime = default;

                    Mileage = default;
                    MileageCommon = default;

                    Description = default;
                }

                public void Assign(MileageModel source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    TechId = source.TechId;
                    TechText = source.TechText;

                    DateTime = source.DateTime;
                    
                    Mileage = source.Mileage;
                    MileageCommon = source.MileageCommon;

                    Description = source.Description;
                }
            }
        }
    }
}
