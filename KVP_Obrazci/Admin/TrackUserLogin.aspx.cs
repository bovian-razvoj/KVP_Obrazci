using DevExpress.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
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
    public partial class TrackUserLogin : ServerMasterPage
    {

        private IActiveUserRepository activeUserRepo;
        private Session session;
        bool filterByPeriod;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();
            
            AllowUserWithRole(Enums.UserRole.SuperAdmin);

            session = XpoHelper.GetNewSession();
            activeUserRepo = new ActiveUserRepository(session);

            ASPxGridViewEmployees.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ASPxGridViewEmployees.DataBind();
            }
        }

        protected void ASPxGridViewEmployees_DataBinding(object sender, EventArgs e)
        {
            activeUserRepo.UpdateUsersLoginActivity();
            List<ActiveUser> list = activeUserRepo.GetHistoryActiveUsers();

            if (list != null && filterByPeriod)
            {
                list = list.Where(au => au.LogInDate.Date >= DateEditDateFrom.Date.Date && au.LogInDate.Date <= DateEditDateTo.Date.Date).ToList();
            }
            else if (list != null)
            {
                list = list.Where(au => au.LogInDate.Date == DateTime.Today.Date).ToList();
            }

            (sender as ASPxGridView).DataSource = list.OrderByDescending(l => l.IsActive);
        }

        protected void ASPxGridViewEmployees_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "FilerByPeriod")
            {
                filterByPeriod = true;
                ASPxGridViewEmployees.DataBind();
            }
        }

        protected void ASPxGridViewEmployees_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "IsActive")
            {
                e.DisplayText = CommonMethods.ParseBool(e.Value) == true ? "DA" : "NE";
            }
        }

        protected void ASPxGridViewEmployees_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName.Equals("IsActive"))
            {
                if (!CommonMethods.ParseBool(e.GetValue("IsActive")))
                {
                    e.Cell.BackColor = Color.Tomato;
                }
                else if (CommonMethods.ParseBool(e.GetValue("IsActive")))
                {
                    e.Cell.BackColor = Color.LightGreen;
                }
            }
        }
    }
}