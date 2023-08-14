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

namespace KVP_Obrazci.CodeList.Machine
{
    public partial class Machine_popup : ServerMasterPage
    {
        Session session;
        IMachineRepository machineRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            machineRepo = new MachineRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSort.Text = (machineRepo.GetCountForSort() + 1).ToString();
            }
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            Stroj machine = new Stroj(session);

            machine.idStroj = 0;
            //machine.Koda = txtCode.Text;
            machine.Opis = txtDesc.Text;
            machine.Sort = CommonMethods.ParseInt(txtSort.Text);

            machineRepo.SaveMachine(machine);

            RemoveSessionsAndClosePopUP(true);
        }

        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";


            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "Machine"), true);

        }
    }
}