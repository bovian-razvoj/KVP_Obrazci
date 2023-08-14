using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Admin
{
    public partial class SystemEmailBody_popup : ServerMasterPage
    {
        Session session = null;
        int systemEmailMessageID = -1;
        string emailBody = "";
        IKVPAuditorRepository auditRepo;
        IKVPDocumentRepository kvpDocRepo;
        IEmployeeRepository employeeRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            systemEmailMessageID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.SystemEmailMessage.SystemEmailMessageID));
            emailBody = GetStringValueFromSession(Enums.SystemEmailMessage.SystemEmailMessageBody);

            session = XpoHelper.GetNewSession();
            auditRepo = new KVPAuditorRepository(session);
            kvpDocRepo = new KVPDocumentRepository(session);
            employeeRepo = new EmployeeRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxHtmlEditorEmailBody.Html = emailBody;
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP(true);
        }


        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.SystemEmailMessage.SystemEmailMessageID);
            RemoveSession(Enums.SystemEmailMessage.SystemEmailMessageBody);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "SystemEmailBody"), true);

        }
    }
}