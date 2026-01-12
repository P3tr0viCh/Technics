using P3tr0viCh.Utils;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms.Design;

namespace Technics
{
    [TypeConverter(typeof(PropertySortedConverter))]
    internal partial class AppSettings : SettingsBase<AppSettings>
    {
        private const string Resource = "Properties.ResourcesSettings";

        [LocalizedAttribute.Category("Category.Directories", Resource)]
        [LocalizedAttribute.DisplayName("DirectoryDatabase.DisplayName", Resource)]
        [LocalizedAttribute.Description("DirectoryDatabase.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DirectoryDatabase { get; set; } = string.Empty;

        [LocalizedAttribute.Category("Category.Directories", Resource)]
        [LocalizedAttribute.DisplayName("DirectoryTracks.DisplayName", Resource)]
        [LocalizedAttribute.Description("DirectoryTracks.Description", Resource)]
        [Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string DirectoryTracks { get; set; } = string.Empty;

        // --------------------------------------------------------------------------------------------------------
        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("Format.FormatDateTime.DisplayName", Resource)]
        public string FormatDateTime { get; set; } = "yyyy.MM.dd HH:mm";

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("Format.FormatMileagesMileage.DisplayName", Resource)]
        public string FormatMileagesMileage { get; set; } = "#,0.00";

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("Format.FormatMileagesMileageCommon.DisplayName", Resource)]
        public string FormatMileagesMileageCommon { get; set; } = "#,0";

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("Format.FormatTechPartsMileage.DisplayName", Resource)]
        public string FormatTechPartsMileage { get; set; } = "#,0";

        [LocalizedAttribute.Category("Category.Format", Resource)]
        [LocalizedAttribute.DisplayName("Format.FormatTechPartsMileageCommon.DisplayName", Resource)]
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