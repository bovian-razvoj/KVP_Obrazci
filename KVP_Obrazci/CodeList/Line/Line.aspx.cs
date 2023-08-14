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

namespace KVP_Obrazci.CodeList.Line
{
    public partial class Line : ServerMasterPage
    {
        Session session;
        ILineRepository lineRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            XpoDSLine.Session = session;

            lineRepo = new LineRepository(session);

            ASPxGridViewLine.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPanelLine_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "DeleteLine")
            {
                int machineID = CommonMethods.ParseInt(ASPxGridViewLine.GetRowValues(ASPxGridViewLine.FocusedRowIndex, "idLinija"));

                if (machineID > 0)
                    lineRepo.DeleteLine(machineID);
            }
            else if (e.Parameter == "AddLine")
            {
                ASPxPopupControlLine.ShowOnPageLoad = true;
            }

            ASPxGridViewLine.DataBind();
        }

        protected void ASPxGridViewLine_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            lineRepo.SaveLineFromBatchUpdate(e.UpdateValues);
        }

        protected void ASPxPopupControlLine_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {

        }
    }
}