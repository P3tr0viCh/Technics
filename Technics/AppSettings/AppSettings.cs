using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Attributes;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Technics
{
    [TypeConverter(typeof(PropertySortedConverter))]
    internal partial class AppSettings : SettingsBase<AppSettings>
    {
        private const string Resource = "Properties.ResourcesSettings";

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryDatabase.DisplayName", Resource)]
        [LocalizedDescription("DirectoryDatabase.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory(false, true, true)]
        public string DirectoryDatabase { get; set; } = string.Empty;

        [LocalizedCategory("Category.Directories", Resource)]
        [LocalizedDisplayName("DirectoryTracks.DisplayName", Resource)]
        [LocalizedDescription("DirectoryTracks.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        [CheckDirectory]
        public string DirectoryTracks { get; set; } = string.Empty;

        // --------------------------------------------------------------------------------------------------------
        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("Format.FormatDateTime.DisplayName", Resource)]
        public string FormatDateTime { get; set; } = "yyyy.MM.dd HH:mm";

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("Format.FormatMileagesMileage.DisplayName", Resource)]
        public string FormatMileagesMileage { get; set; } = "#,0.00";

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("Format.FormatMileagesMileageCommon.DisplayName", Resource)]
        public string FormatMileagesMileageCommon { get; set; } = "#,0";

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("Format.FormatTechPartsMileage.DisplayName", Resource)]
        public string FormatTechPartsMileage { get; set; } = "#,0";

        [LocalizedCategory("Category.Format", Resource)]
        [LocalizedDisplayName("Format.FormatTechPartsMileageCommon.DisplayName", Resource)]
        public string FormatTechPartsMileageCommon { get; set; } = "#,0";

        // --------------------------------------------------------------------------------------------------------
        [Browsable(false)]
        public int PanelTechsWidth { get; set; } = 260;
        [Browsable(false)]
        public int PanelBottomHeight { get; set; } = 128;

        [Browsable(false)]
        public bool ToolStripsShowText { get; set; } = true;

        [Browsable(false)]
        public string DirectoryLastMileages { get; set; } = string.Empty;

        // --------------------------------------------------------------------------------------------------------
        public void Check()
        {
        }
    }
}