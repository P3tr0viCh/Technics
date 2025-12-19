using System.ComponentModel;

namespace Technics
{
    internal partial class AppSettings
    {
        [Browsable(false)]
        public ColumnState[] ColumnsMileages { get; set; }

        [Browsable(false)]
        public ColumnState[] ColumnsTechParts { get; set; }

        [Browsable(false)]
        public ColumnState[] ColumnsListParts { get; set; }

        [Browsable(false)]
        public ColumnState[] ColumnsListTechs { get; set; }
    }
}