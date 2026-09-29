using P3tr0viCh.Utils.Attributes;
using P3tr0viCh.Utils.Converters;
using P3tr0viCh.Utils.Settings;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Technics
{
    [TypeConverter(typeof(PropertySortedConverter))]
    internal partial class Settings : SettingsPersistenceBase, IFormStates, IColumnStates
    {
        private const string ResourcesName = "Properties.ResourcesSettings";

        [LocalizedCategory("Category.Directories", ResourcesName)]
        [LocalizedDisplayName("DirectoryDatabase.DisplayName", ResourcesName)]
        [LocalizedDescription("DirectoryDatabase.Description", ResourcesName)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory()]
        public string DirectoryDatabase { get; set; } = string.Empty;

        [LocalizedCategory("Category.Directories", ResourcesName)]
        [LocalizedDisplayName("DirectoryTracks.DisplayName", ResourcesName)]
        [LocalizedDescription("DirectoryTracks.Description", ResourcesName)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory]
        public string DirectoryTracks { get; set; } = string.Empty;

        // --------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Format", ResourcesName)]
        [LocalizedDisplayName("Format.FormatDateTime.DisplayName", ResourcesName)]
        public string FormatDateTime { get; set; } = "yyyy.MM.dd HH:mm";

        [LocalizedCategory("Category.Format", ResourcesName)]
        [LocalizedDisplayName("Format.FormatMileagesMileage.DisplayName", ResourcesName)]
        public string FormatMileagesMileage { get; set; } = "#,0.00";

        [LocalizedCategory("Category.Format", ResourcesName)]
        [LocalizedDisplayName("Format.FormatMileagesMileageCommon.DisplayName", ResourcesName)]
        public string FormatMileagesMileageCommon { get; set; } = "#,0";

        [LocalizedCategory("Category.Format", ResourcesName)]
        [LocalizedDisplayName("Format.FormatTechPartsMileage.DisplayName", ResourcesName)]
        public string FormatTechPartsMileage { get; set; } = "#,0";

        [LocalizedCategory("Category.Format", ResourcesName)]
        [LocalizedDisplayName("Format.FormatTechPartsMileageCommon.DisplayName", ResourcesName)]
        public string FormatTechPartsMileageCommon { get; set; } = "#,0";

        [LocalizedCategory("Category.Format", ResourcesName)]
        [LocalizedDisplayName("Format.FormatMaintenanceMileageCommon.DisplayName", ResourcesName)]
        public string FormatMaintenanceMileageCommon { get; set; } = "#,0";

        [LocalizedCategory("Category.Format", ResourcesName)]
        [LocalizedDisplayName("Format.FormatMaintenanceMileageAfterMaintenance.DisplayName", ResourcesName)]
        public string FormatMaintenanceMileageAfterMaintenance { get; set; } = "#,0";

        // --------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public int PanelTechsWidth { get; set; } = 260;
        [Browsable(false)]
        public int PanelTechPartWidth { get; set; } = 432;
        [Browsable(false)]
        public int PanelBottomHeight { get; set; } = 200;

        [Browsable(false)]
        public bool ToolStripsShowText { get; set; } = true;

        [Browsable(false)]
        public bool ArchiveExpanded { get; set; } = true;

        // --------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public string DirectoryLastMileages { get; set; } = string.Empty;

        // --------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public FormStates FormStates { get; set; }

        [Browsable(false)]
        public ColumnStates ColumnStates { get; set; }
    }
}