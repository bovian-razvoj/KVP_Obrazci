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

namespace KVP_Obrazci.CodeList.Location
{
    public partial class Location_popup : ServerMasterPage
    {
        Session session;
        ILocationRepository locationRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            locationRepo = new LocationRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSort.Text = (locationRepo.GetCntForSort() + 1).ToString();
            }
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            Lokacija location = new Lokacija(session);

            location.idLokacija = 0;
            //location.Koda = txtCode.Text;
            location.Opis = txtDesc.Text;
            location.Sort = CommonMethods.ParseInt(txtSort.Text);

            locationRepo.SaveLocation(location);

            RemoveSessionsAndClosePopUP(true);
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";


            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Location"), true);

        }
    }
}