using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.CodeList.KVPType
{
    public partial class KVPType : ServerMasterPage
    {
        Session session;
        IKVPTypeRepository kvpTypeRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            XpoDSKVPType.Session = session;

            kvpTypeRepo = new KVPTypeRepository(session);

            ASPxGridViewKVPType.Settings.GridLines = GridLines.Both;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxPopupControlType_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {

        }

        protected void ASPxGridViewKVPType_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "DeleteType")
            {
                int typeID = CommonMethods.ParseInt(ASPxGridViewKVPType.GetRowValues(ASPxGridViewKVPType.FocusedRowIndex, "idTip"));

                if (typeID > 0)
                    kvpTypeRepo.DeleteKVPType(typeID);
            }

            ASPxGridViewKVPType.DataBind();
        }

        protected void ASPxGridViewKVPType_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            kvpTypeRepo.SaveKVPTypeFromBatchUpdate(e.UpdateValues);
        }
    }
}