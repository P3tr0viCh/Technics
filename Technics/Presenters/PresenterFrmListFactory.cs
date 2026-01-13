using System;

namespace Technics.Presenters
{
    internal class PresenterFrmListFactory
    {
        public static IPresenterFrmList PresenterFrmListInstance(IFrmList frmList, FrmListType listType)
        {
            switch (listType)
            {
                case FrmListType.Parts:
                    return new PresenterFrmListParts(frmList);
                case FrmListType.Techs:
                    return new PresenterFrmListTechs(frmList);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}