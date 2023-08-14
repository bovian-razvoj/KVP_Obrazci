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

namespace KVP_Obrazci.CodeList.Location
{
    public partial class Location : ServerMasterPage
    {
        Session session;
        ILocationRepository locationRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            XpoDSLocation.Session = session;

            locationRepo = new LocationRepository(session);

            ASPxGridViewLocation.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewLocation_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            locationRepo.SaveLocationFromBatchUpdate(e.UpdateValues);
        }

        protected void ASPxPopupControlLocation_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            
        }

        protected void CallbackPanelLocation_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "DeleteLocation")
            {
                int typeID = CommonMethods.ParseInt(ASPxGridViewLocation.GetRowValues(ASPxGridViewLocation.FocusedRowIndex, "idLokacija"));

                if (typeID > 0)
                    locationRepo.DeleteLocation(typeID);
            }
            else if (e.Parameter == "AddLocation")
            {
                ASPxPopupControlLocation.ShowOnPageLoad = true;
            }

            ASPxGridViewLocation.DataBind();
        }
    }
}