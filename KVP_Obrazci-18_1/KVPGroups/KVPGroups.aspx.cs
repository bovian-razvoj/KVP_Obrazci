using DevExpress.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
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
    public partial class KVPGroups : ServerMasterPage
    {
        Session session = null;
        int kVPGroupIDFocusedRowIndex = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserChampion()) RedirectHome();

            session = XpoHelper.GetNewSession();

            XpoDataSourceKVPGroups.Session = session;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                kVPGroupIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            ASPxGridViewKVPGroups.Settings.GridLines = GridLines.Both;

            if (PrincipalHelper.IsUserChampion())
                btnAddNewKvpGroup.ClientVisible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (kVPGroupIDFocusedRowIndex > 0)
                {
                    ASPxGridViewKVPGroups.FocusedRowIndex = ASPxGridViewKVPGroups.FindVisibleIndexByKeyValue(kVPGroupIDFocusedRowIndex);
                    ASPxGridViewKVPGroups.ScrollToVisibleIndexOnClient = ASPxGridViewKVPGroups.FindVisibleIndexByKeyValue(kVPGroupIDFocusedRowIndex);
                }
            }
        }

        protected void ASPxGridViewKVPGroups_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0].Equals("DblClick") && !String.IsNullOrEmpty(split[1]))
            {
                ASPxWebControl.RedirectOnCallback(GenerateURI("KVPGroupsForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void ASPxGridViewKVPGroups_DataBound(object sender, EventArgs e)
        {

        }

        protected void btnAddNewKvpGroup_Click(object sender, EventArgs e)
        {
            RedirectWithCustomURI("KVPGroupsForm.aspx", (int)Enums.UserAction.Add, 0);
        }
    }
}