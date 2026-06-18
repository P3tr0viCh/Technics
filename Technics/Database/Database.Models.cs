using Dapper.Contrib.Extensions;
using P3tr0viCh.Database;
using System;
using Technics.Properties;

namespace Technics
{
    public partial class Database
    {
        public partial class Models
        {
            // ---------------------------------------------------------------
            [Table(Tables.folders)]
            public class FolderModel : BaseText
            {
                private long? parentId = null;
                public long? ParentId
                {
                    get => parentId;
                    set => parentId = value != Sql.NewId ? value : null;
                }

                [Computed]
                [Write(false)]
                public string Path { get; set; } = string.Empty;

                public override void Clear()
                {
                    base.Clear();

                    ParentId = null;

                    Path = string.Empty;
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

                    Path = source.Path;
                }
            }


            // ---------------------------------------------------------------
            [Table(Tables.techs)]
            public class TechModel : BaseTextDescriptionFolderState
            {
            }

            public enum MileageType
            {
                Single = 0,
                Common = 1,
            }

            // ---------------------------------------------------------------
            [Table(Tables.mileages)]
            public class MileageModel : BaseTechId
            {
                public DateTime DateTime { get; set; } = default;

                public double Mileage { get; set; } = default;

                public double MileageCommon { get; set; } = default;

                public MileageType MileageType { get; set; } = MileageType.Single;

                public string Description { get; set; } = null;

                public override void Clear()
                {
                    base.Clear();

                    DateTime = default;

                    Mileage = default;
                    MileageCommon = default;

                    MileageType = MileageType.Single;

                    Description = null;
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

                    MileageType = source.MileageType;

                    Description = source.Description;
                }
            }

            // ---------------------------------------------------------------
            [Table(Tables.parts)]
            public class PartModel : BaseTextDescriptionFolderState
            {
            }

            // ---------------------------------------------------------------
            [Table(Tables.techparts)]
            public class TechPartModel : BaseTechId
            {
                private long? partId = null;
                public long? PartId
                {
                    get => partId;
                    set => partId = value != Sql.NewId ? value : null;
                }

                [Computed]
                [Write(false)]
                public string PartText { get; set; } = default;

                public DateTime DateTimeInstall { get; set; } = default;
                public DateTime? DateTimeRemove { get; set; } = default;

                public double? Mileage { get; set; } = null;

                public double? MileageCommon { get; set; } = null;

                public override void Clear()
                {
                    base.Clear();

                    PartId = null;
                    PartText = default;

                    DateTimeInstall = default;
                    DateTimeRemove = default;

                    Mileage = null;
                    MileageCommon = null;
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

                    Mileage = source.Mileage;
                    MileageCommon = source.MileageCommon;
                }
            }

            // ---------------------------------------------------------------
            [Table(Tables.mts)]
            public class MtModel : BaseTextDescriptionFolder
            {
            }

            // ---------------------------------------------------------------
            [Table(Tables.maintenance)]
            public class MaintenanceModel : BaseTechId
            {
                private long? mtId = null;
                public long? MtId
                {
                    get => mtId;
                    set => mtId = value != Sql.NewId ? value : null;
                }

                [Computed]
                [Write(false)]
                public string MtText { get; set; } = default;

                public DateTime DateTime { get; set; } = default;

                public double? MileageCommon { get; set; } = null;

                public double? MileageAfterMaintenance { get; set; } = null;

                public override void Clear()
                {
                    base.Clear();

                    MtId = null;
                    MtText = default;

                    DateTime = default;

                    MileageCommon = null;
                    MileageAfterMaintenance = null;
                }

                public void Assign(MaintenanceModel source)
                {
                    if (source == null)
                    {
                        Clear();

                        return;
                    }

                    base.Assign(source);

                    MtId = source.MtId;
                    MtText = source.MtText;

                    DateTime = source.DateTime;

                    MileageCommon = source.MileageCommon;
                    MileageAfterMaintenance = source.MileageAfterMaintenance;
                }
            }
        }
    }
}