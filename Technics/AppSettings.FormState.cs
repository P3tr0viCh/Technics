using System.ComponentModel;

namespace Technics
{
    internal partial class AppSettings
    {
        [Browsable(false)]
        public FormState FormStateMain { get; set; }
    }
}