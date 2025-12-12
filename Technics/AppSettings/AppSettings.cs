using P3tr0viCh.Utils;
using System.ComponentModel;

namespace Technics
{
    [TypeConverter(typeof(PropertySortedConverter))]
    internal partial class AppSettings : SettingsBase<AppSettings>
    {
        public int PanelTechsWidth { get; set; } = 260;
        public int PanelBottomHeight { get; set; } = 128;

        public void Check()
        {
        }
    }
}