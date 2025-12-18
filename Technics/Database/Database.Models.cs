using Dapper.Contrib.Extensions;
using P3tr0viCh.Database;
using System;

namespace Technics
{
    public partial class Database
    {
        public class Models
        {
            // ---------------------------------------------------------------
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

            // ---------------------------------------------------------------
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

            // ---------------------------------------------------------------
            public abstract class BaseTechId : BaseId
            {
                public long? TechId { get; set; } = null;

                [Write(false)]
                [Computed]
                public string TechText { get; set; } = default;
                
                public new void Clear()
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

            // ---------------------------------------------------------------
            [Table(Tables.mileages)]
            public class MileageModel : BaseTechId
            {
                public DateTime DateTime { get; set; } = default;

                public double Mileage { get; set; } = default;

                [Write(false)]
                [Computed]
                public double MileageCommon { get; set; } = default;

                public string Description { get; set; } = default;

                public new void Clear()
                {
                    base.Clear();

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

                    DateTime = source.DateTime;

                    Mileage = source.Mileage;
                    MileageCommon = source.MileageCommon;

                    Description = source.Description;
                }
            }

            // ---------------------------------------------------------------
            [Table(Tables.parts)]
            public class PartModel : BaseText
            {
                public new void Clear()
                {
                    base.Clear();
                }

                public void Assign(PartModel source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);
                }
            }

            // ---------------------------------------------------------------
            [Table(Tables.techparts)]
            public class TechPartModel : BaseTechId
            {
                public long? PartId { get; set; } = null;

                [Write(false)]
                [Computed]
                public string PartText { get; set; } = default;

                public DateTime DateTimeInstall { get; set; } = default;
                public DateTime? DateTimeRemove { get; set; } = default;

                public new void Clear()
                {
                    base.Clear();

                    PartId = null;
                    PartText = default;

                    DateTimeInstall = default;
                    DateTimeRemove = default;
                }

                public void Assign(TechPartModel source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    PartId = source.PartId;
                    PartText = source.PartText;

                    DateTimeInstall = source.DateTimeInstall;
                    DateTimeRemove = source.DateTimeRemove;
                }
            }
        }
    }
}