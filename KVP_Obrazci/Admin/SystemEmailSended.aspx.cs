using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Admin
{
    public partial class SystemEmailSended : ServerMasterPage
    {
        Session session;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            AllowUserWithRole(Enums.UserRole.Admin, Enums.UserRole.SuperAdmin);


            session = XpoHelper.GetNewSession();

            XpoDSSystemEmailMessage.Session = session;

            ASPxGridViewSystemEmailMessage.Settings.GridLines = GridLines.Both;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                
            }

        }

        protected void ASPxGridViewSystemEmailMessage_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Status")
            {
                int value = CommonMethods.ParseInt(e.Value);
                if(value == 0)
                    e.DisplayText = "V ČAKALNI VRSTI";
                else if (value == 1)
                    e.DisplayText = "POSLANO";
                else if (value == 2)
                    e.DisplayText = "NAPAKA V POŠILJANJU";
            }
        }

        protected void ASPxGridViewSystemEmailMessage_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName.Equals("Status"))
            {
                if (CommonMethods.ParseInt(e.GetValue("Status")) == 2)
                {
                    e.Cell.BackColor = Color.Tomato;
                }
                else if (CommonMethods.ParseInt(e.GetValue("Status")) == 1)
                {
                    e.Cell.BackColor = Color.LightGreen;
                }
                else if (CommonMethods.ParseInt(e.GetValue("Status")) == 0)
                {
                    e.Cell.BackColor = Color.Orange;
                }
            }
        }

        protected void ASPxPopupControlSystemEmailBody_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.SystemEmailMessage.SystemEmailMessageID);
            RemoveSession(Enums.SystemEmailMessage.SystemEmailMessageBody);
        }

        protected void SytemEmailMessageCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] split = e.Parameter.Split('|');

            if (split[0] == "DblClickShowEmailBody")
            {
                string emailID = split[1];
                string emailBody = split[2];
                AddValueToSession(Enums.SystemEmailMessage.SystemEmailMessageID, emailID);
                AddValueToSession(Enums.SystemEmailMessage.SystemEmailMessageBody, emailBody);

                ASPxPopupControlSystemEmailBody.ShowOnPageLoad = true;
            }
        }
    }
}