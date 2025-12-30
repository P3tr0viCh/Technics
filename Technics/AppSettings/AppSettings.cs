using P3tr0viCh.Utils;
using System.ComponentModel;

namespace Technics
{
    [TypeConverter(typeof(PropertySortedConverter))]
    internal partial class AppSettings : SettingsBase<AppSettings>
    {
        public string FormatDateTime { get; set; } = "yyyy.MM.dd HH:mm";

        [Browsable(false)]
        public int PanelTechsWidth { get; set; } = 260;
        [Browsable(false)]
        public int PanelBottomHeight { get; set; } = 128;

        [Browsable(false)]
        public bool ToolStripsShowText { get; set; } = true;

        [Browsable(false)]
        public string DirectoryLastMileages { get; set; } = string.Empty;

        public void Check()
        {
        }
    }
}