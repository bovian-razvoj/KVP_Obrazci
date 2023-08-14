using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Widgets
{
    public partial class PopUp : System.Web.UI.UserControl
    {
        public event EventHandler ConfirmBtnClick;
        public event EventHandler CancelBtnClick;
        public event EventHandler CloseBtnWindowClick;

        /// <summary>
        /// Pop up control
        /// </summary>
        public ASPxPopupControl PopUpControl { get { return PupUpNotification; } }
        public ASPxLoadingPanel LoadingPanelControl { get { return LoadingPanel; } }

        public string SetTitle { get { return Title.HeaderText; } set { Title.HeaderText = value; } }
        public string SetDescription { get { return Description.Text; } set { Description.Text = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CallbackPopUp_Callback(object source, CallbackEventArgs e)
        {
            if (e.Parameter == "Confirm")
            {
                if (CancelBtnClick != null)
                    ConfirmBtnClick(this, EventArgs.Empty);
            }
            else if (e.Parameter == "Cancel")
            {
                if (CancelBtnClick != null)
                    CancelBtnClick(this, EventArgs.Empty);
            }
        }

        public void ShowPopUp()
        {
            PopUpControl.ShowOnPageLoad = true;
        }

        protected void PupUpNotification_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (CloseBtnWindowClick != null)
                CloseBtnWindowClick(this, EventArgs.Empty);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            if (CancelBtnClick != null)
                ConfirmBtnClick(this, EventArgs.Empty);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            PupUpNotification.ShowOnPageLoad = false;
            if (CancelBtnClick != null)
                CancelBtnClick(this, EventArgs.Empty);
        }
    }
}