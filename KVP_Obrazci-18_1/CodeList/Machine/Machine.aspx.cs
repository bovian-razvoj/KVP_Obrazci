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

namespace KVP_Obrazci.CodeList.Machine
{
    public partial class Machine : ServerMasterPage
    {
        Session session;
        IMachineRepository machineRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            XpoDSMachine.Session = session;

            machineRepo = new MachineRepository(session);

            ASPxGridViewMachine.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ASPxGridViewMachine_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            machineRepo.SaveMachineFromBatchUpdate(e.UpdateValues);
        }

        protected void ASPxPopupControlMachine_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {

        }

        protected void CallbackPanelMachine_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter == "DeleteMachine")
            {
                int machineID = CommonMethods.ParseInt(ASPxGridViewMachine.GetRowValues(ASPxGridViewMachine.FocusedRowIndex, "idStroj"));

                if (machineID > 0)
                    machineRepo.DeleteMachine(machineID);
            }
            else if (e.Parameter == "AddMachine")
            {
                ASPxPopupControlMachine.ShowOnPageLoad = true;
            }

            ASPxGridViewMachine.DataBind();
        }
    }
}