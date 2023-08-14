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

namespace KVP_Obrazci.KVPGroups
{
    public partial class Employee_popup : ServerMasterPage
    {
        Session session = null;
        IKVPGroupsRepository kvpGroupRepo = null;
        IEmployeeRepository employeeRepo = null;
        int kvpGroupID = -1;
        bool isChampions = false;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();
            XpoDSUsers.Session = session;
            kvpGroupRepo = new KVPGroupsRepository(session);
            employeeRepo = new EmployeeRepository(session);
            kvpGroupID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.KVPGroups.KVPGroupID));

            if (SessionHasValue(Enums.KVPGroups.KVPGroupUsersCriteriaPopUp))
            {
                //XpoDSUsers.Criteria = "[<KVPSkupina_Users>][Champion=1]";
                isChampions = true;
            }

            ASPxGridViewEmployees.Settings.GridLines = GridLines.Both;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            List<object> selectedRows = ASPxGridViewEmployees.GetSelectedFieldValues("Id");

            kvpGroupRepo.SaveEmployeesToKVPGroup(selectedRows, kvpGroupID, isChampions);

            RemoveSessionsAndClosePopUP(true);
        }


        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.KVPGroups.KVPGroupID);
            RemoveSession(Enums.KVPGroups.KVPGroupUsersCriteriaPopUp);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, isChampions ? "EmployeeChampion" : "Employee"), true);

        }
    }
}