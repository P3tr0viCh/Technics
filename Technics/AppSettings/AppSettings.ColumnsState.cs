using System.ComponentModel;

namespace Technics
{
    internal partial class AppSettings
    {
        [Browsable(false)]
        public ColumnState[] ColumnsMileages { get; set; }
    }
}