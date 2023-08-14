using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Employees
{
    public partial class EmployeesSync : ServerMasterPage
    {
        Session session = null;
        IEmployeeRepository employeeRepo = null;
        IKVPDocumentRepository kvpDocRepo = null;
        IMessageProcessorRepository messageRepo = null;


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            AllowUserWithRole(Enums.UserRole.SuperAdmin, Enums.UserRole.Admin);

            session = XpoHelper.GetNewSession();
            XpoDSEmployee.Session = session;

            employeeRepo = new EmployeeRepository(session);
            kvpDocRepo = new KVPDocumentRepository(session);
            messageRepo = new MessageProcessorRepository(session);


            ASPxGridLookupEmployee.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupEmployee2.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            else
            { }
        }

        protected void btnSync_Click(object sender, EventArgs e)
        {
            memNotes.Text = "";
            int iSourceEmpl = CommonMethods.ParseInt(ASPxGridLookupEmployee.Value);
            int iDestinationEmpl = CommonMethods.ParseInt(ASPxGridLookupEmployee2.Value);

            if ((iSourceEmpl != 0 && iDestinationEmpl != 0) && (iSourceEmpl != iDestinationEmpl))
            {

                int iNumKVPPredlagatelj = kvpDocRepo.ChangePredlagateljOnKVPDocument(iSourceEmpl, iDestinationEmpl);
                int iNumKVPRealizator = kvpDocRepo.ChangeRealizatorOnKVPDocument(iSourceEmpl, iDestinationEmpl);
                int iNumKVPPresoja = kvpDocRepo.ChangePresojaOnKVPDocument(iSourceEmpl, iDestinationEmpl);
                int iNumMesecov = kvpDocRepo.SyncPayment(iSourceEmpl, iDestinationEmpl);
                //int iNumOddelko = kvpDocRepo.ChangeKVPSkupinaAndDepartmentSync(iSourceEmpl, iDestinationEmpl);
                employeeRepo.SetDeleteFlagEmployee(iSourceEmpl);



                string strResults = "Sinhronizacija je bila uspešno izvedena \n\n";
                strResults += "Število KVP dokumentov, kje je bila oseba PREDLAGATELJ: " + iNumKVPPredlagatelj + "\n";
                strResults += "Število KVP dokumentov, kje je bila oseba REALIZATOR: " + iNumKVPRealizator + "\n";
                strResults += "Število KVP dokumentov, kje je bila oseba PRESOJEVALEC: " + iNumKVPPresoja + "\n";
                //strResults += "Število mesecev kje je dobila oseba točke: " + iNumMesecov + "\n";
                //strResults += "Sinhronizacija Oddelkov in KVP SKupine\n";


                Users usrSource = employeeRepo.GetEmployeeByID(iSourceEmpl);
                Users usrDestination = employeeRepo.GetEmployeeByID(iDestinationEmpl);
                

                if ((usrSource != null) && (usrDestination != null))
                {
                    usrSource.SinhronizationNo++;
                    usrSource.LastSinhroDate = DateTime.Now;
                    usrSource.Save();

                    usrDestination.SinhronizationNo++;
                    usrDestination.LastSinhroDate = DateTime.Now;
                    usrDestination.Save();

                    if (usrSource.Username != usrDestination.Username)
                    {
                        // send message to user
                        InfoMailModel model = new InfoMailModel();

                        model.FirstName = usrDestination.Firstname;
                        model.Lastname = usrDestination.Lastname;

                        model.OldUserName = usrSource.Username;
                        model.NewUserName = usrDestination.Username;

                        int intRet = messageRepo.ProcessChangedUserNameToSend(iDestinationEmpl, model);
                        if (intRet == 1)
                        {
                            strResults += "Poslano novo uporabniško ime: " + model.NewUserName + "\n";
                        }

                        usrDestination.Password = usrSource.Password;
                        usrDestination.Save();
                    }
                }

                memNotes.Text = strResults;
            }

            Master.NavigationBarMain.DataBind();
        }
    }
}