using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Settings;

namespace Technics
{
    internal class AppSettings : DefaultInstance<SettingsStorage<Settings>>
    {
        public static Settings Settings => Default.Settings;
    }
}