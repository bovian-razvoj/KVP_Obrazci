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

namespace KVP_Obrazci.KVPDocuments
{
    public partial class KVPAuditor_popup : ServerMasterPage
    {
        Session session = null;
        int kvpDocID = -1;
        int proposerID = -1;
        IKVPAuditorRepository auditRepo;
        IKVPDocumentRepository kvpDocRepo;
        IEmployeeRepository employeeRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            kvpDocID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.KVPDocumentSession.KVPDocumentID));
            proposerID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.Employee.EmployeeID));

            session = XpoHelper.GetNewSession();
            auditRepo = new KVPAuditorRepository(session);
            kvpDocRepo = new KVPDocumentRepository(session);
            employeeRepo = new EmployeeRepository(session);

            XpoDSEmployee.Session = session;

            
            ASPxGridLookupAuditor.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            XpoDSEmployee.Criteria = "[Id] <> " + proposerID.ToString();
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            int employeeID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupAuditor));
            KVPPresoje audit = new KVPPresoje(session);

            audit.idKVPDocument = kvpDocRepo.GetKVPByID(kvpDocID);
            audit.Presojevalec = employeeRepo.GetEmployeeByID(employeeID);

            

            audit.Opomba = ASPxMemoNotes.Text;
            auditRepo.SaveKVPAuditor(audit);
            //spremenimo status v presojo. Vendar samo enkrat dodamo ta status
            if (audit.idKVPDocument.LastStatusId.Koda == Enums.KVPStatuses.ODOBRITEV_VODJA.ToString())
                kvpDocRepo.ChangeStatusOnKVPDocument(kvpDocID, Enums.KVPStatuses.POSLANO_V_PRESOJO, true);

            RemoveSessionsAndClosePopUP(true);

            audit.idKVPDocument.LastPresojaID = audit.Presojevalec;
            audit.idKVPDocument.Save();
        }


        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.KVPDocumentSession.KVPDocumentID);
            RemoveSession(Enums.Employee.EmployeeID);

            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = kvpDocID.ToString() } 
            };

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}', '{2}');", confirmCancelAction, "Auditor", GenerateURI("../Dashboard.aspx", queryStrings)), true);

        }
    }
}