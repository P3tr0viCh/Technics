using System;
using static Technics.Enums;

namespace Technics.Forms
{
    internal class PresenterFrmListFactory
    {
        public static IPresenterFrmList PresenterFrmListInstance(IFrmList frmList, ListType listType)
        {
            switch (listType)
            {
                case ListType.Parts:
                    return new PresenterFrmListParts(frmList);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}