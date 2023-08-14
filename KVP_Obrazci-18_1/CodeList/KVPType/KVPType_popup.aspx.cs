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

namespace KVP_Obrazci.CodeList.KVPType
{
    public partial class KVPType_popup : ServerMasterPage
    {
        Session session;
        IKVPTypeRepository kvpTypeRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            kvpTypeRepo = new KVPTypeRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            Tip kvpType = new Tip(session);

            kvpType.idTip = 0;
            kvpType.Koda = txtCode.Text;
            kvpType.Naziv = txtName.Text;
            kvpType.TockePredlagatelj = CommonMethods.ParseInt(txtProposerPoint.Text);
            kvpType.TockeRealizator = CommonMethods.ParseInt(txtRealizatorPoint.Text);
            
            kvpTypeRepo.SaveKVPType(kvpType);

            RemoveSessionsAndClosePopUP(true);
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.KVPDocumentSession.KVPDocumentID);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "KVPType"), true);

        }
    }
}