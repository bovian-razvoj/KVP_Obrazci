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

namespace KVP_Obrazci.CodeList.Line
{
    public partial class Line_popup : ServerMasterPage
    {
        Session session;
        ILineRepository lineRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            lineRepo = new LineRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSort.Text = (lineRepo.GetCountForSort() + 1).ToString();
            }
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            Linija line = new Linija(session);

            line.idLinija = 0;
            //line.Koda = txtCode.Text;
            line.Opis = txtDesc.Text;
            line.Sort = CommonMethods.ParseInt(txtSort.Text);

            lineRepo.SaveLine(line);

            RemoveSessionsAndClosePopUP(true);
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";


            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Line"), true);

        }
    }
}