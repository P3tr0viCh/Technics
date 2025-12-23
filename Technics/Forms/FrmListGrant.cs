using System;

namespace Technics
{
    [Flags]
    public enum FrmListGrant
    {
        None = 0,
        Add = 1,
        Change = 2,
        Delete = 4,
    }
}