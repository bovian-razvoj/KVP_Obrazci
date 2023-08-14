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

namespace KVP_Obrazci.KVPGroups
{
    public partial class EmployeesKVPGroups : ServerMasterPage
    {
        Session session = null;
        IKVPGroupsRepository kvpGroupRepo = null;
        bool newEmployees = false;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.QueryString["newEmployees"] != null)
                newEmployees = CommonMethods.ParseBool(Request.QueryString["newEmployees"].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            kvpGroupRepo = new KVPGroupsRepository(session);

            XpoDSUsers.Session = session;
            XpoDSKVPGroups.Session = session;
            XpoDSUsersToRemove.Session = session;

            ASPxGridViewEmployees.Settings.GridLines = GridLines.Both;
            ASPxGridLookupKVPGroups.GridView.Settings.GridLines = GridLines.Both;

            if (newEmployees)
            {
                XpoDSUsers.Criteria = "[NewEmployee] = 1";
            }
        }

        protected void CallbackPanelEmployeesKVPGroups_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "AddEmployeToKVPGroup")
            {
                List<object> selectedRows = ASPxGridViewEmployees.GetSelectedFieldValues("Id");
                int kvpGroupID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupKVPGroups));
                kvpGroupRepo.SaveEmployeesToKVPGroup(selectedRows, kvpGroupID, false, newEmployees);
                ASPxGridViewEmployees.DataBind();
                ASPxGridViewEmployees.Selection.UnselectAll();
            }
            else if (e.Parameter == "RemoveEmployeFromKVPGroup")
            {
                List<object> selectedRows = ASPxGridViewEmployeesToRemove.GetSelectedFieldValues("idKVPSkupina_Zaposleni");

                kvpGroupRepo.DeleteEmployeesFromKVPGroupUsers(selectedRows.Cast<int>().ToList());
                ASPxGridViewEmployeesToRemove.DataBind();
                CallbackPanelEmployeesKVPGroups.JSProperties["cpIsDeleted"] = true;
            }
        }
    }
}