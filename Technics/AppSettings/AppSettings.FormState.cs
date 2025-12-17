using System.ComponentModel;

namespace Technics
{
    internal partial class AppSettings
    {
        [Browsable(false)]
        public FormState FormStateMain { get; set; }

        [Browsable(false)]
        public FormState FormStateListParts { get; set; }
    }
}