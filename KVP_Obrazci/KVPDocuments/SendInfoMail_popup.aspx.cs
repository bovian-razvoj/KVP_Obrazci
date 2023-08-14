using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
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
    public partial class SendInfoMail_popup : ServerMasterPage
    {
        Session session = null;
        
        IMessageProcessorRepository messageRepo;
        IEmployeeRepository employeeRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            messageRepo = new MessageProcessorRepository(session);
            employeeRepo = new EmployeeRepository(session);
            XpoDSEmployee.Session = session;

            
            ASPxGridLookupEmployee.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            XpoDSEmployee.Criteria = "[Id] <> " + PrincipalHelper.GetUserPrincipal().ID;
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.KVPDocumentSession.KVPDocumentID);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "InfoMail"), true);

        }

        protected void btnSendInfoMail_Click(object sender, EventArgs e)
        {
            string sSenders = "";
            //int employeeID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupEmployee));
            InfoMailModel model = new InfoMailModel();
            
            List<int> selectedEmployees = ASPxGridLookupEmployee.GridView.GetSelectedFieldValues("Id").OfType<int>().ToList();
            
            //selectedEmployees.Add(PrincipalHelper.GetUserPrincipal().ID);
            
            foreach (var item in selectedEmployees)
            {
                Users usrHead = employeeRepo.GetEmployeeByID(item);
                if (usrHead != null)
                {
                    sSenders += CommonMethods.GetNameByUser(usrHead) + ", ";
                }
                SendEmailToEmployee(model, item, false);

                //model.Notes = ASPxMemoNotes.Text;
                //model.SenderFirstName = PrincipalHelper.GetUserPrincipal().firstName;
                //model.SenderLastname = PrincipalHelper.GetUserPrincipal().lastName;
                //model.StevilkaKVP = SessionHasValue(Enums.KVPDocumentSession.KVPDocumentID) ? GetValueFromSession(Enums.KVPDocumentSession.KVPDocumentID).ToString() : "";

                //Users usrHead = employeeRepo.GetEmployeeByID(item);
                //if (usrHead != null)
                //{
                //    sSenders += CommonMethods.GetNameByUser(usrHead) + ", ";
                //}


                //if (GetKVPDocumentProvider().GetKVPDocumentModel() != null)
                //    modelDoc = GetKVPDocumentProvider().GetKVPDocumentModel();
                //if (modelDoc != null) model.DocumentID = modelDoc.idKVPDocument;
                //messageRepo.ProcessInfoKVPMailToSend(item, model);   



            }

            // mail sended to users who has send email request
            sSenders = sSenders.Substring(0, sSenders.Length - 2);
            model.Senders = sSenders;
            SendEmailToEmployee(model, PrincipalHelper.GetUserPrincipal().ID, true);

            RemoveSessionsAndClosePopUP(true);
        }

        private void SendEmailToEmployee(InfoMailModel model, Int32 usrID, bool sendToSender)
        {
            KVPDocument modelDoc = null;

            model.Notes = ASPxMemoNotes.Text;
            model.SenderFirstName = PrincipalHelper.GetUserPrincipal().firstName;
            model.SenderLastname = PrincipalHelper.GetUserPrincipal().lastName;
            model.StevilkaKVP = SessionHasValue(Enums.KVPDocumentSession.KVPDocumentID) ? GetValueFromSession(Enums.KVPDocumentSession.KVPDocumentID).ToString() : "";
          
            if (GetKVPDocumentProvider().GetKVPDocumentModel() != null)
                modelDoc = GetKVPDocumentProvider().GetKVPDocumentModel();
            if (modelDoc != null) model.DocumentID = modelDoc.idKVPDocument;
            messageRepo.ProcessInfoKVPMailToSend(usrID, model, null, sendToSender);
        }
    }
}