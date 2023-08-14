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

namespace KVP_Obrazci.Admin
{
    public partial class KVPArchive : ServerMasterPage
    {
        Session session = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            AllowUserWithRole(Enums.UserRole.Admin, Enums.UserRole.SuperAdmin);

            session = XpoHelper.GetNewSession();

            XpoDSKVPDocumentArchive.Session = session;

            ASPxGridViewKVPDocumentArchive.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}