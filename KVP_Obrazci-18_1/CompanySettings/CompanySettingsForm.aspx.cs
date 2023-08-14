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

namespace KVP_Obrazci.CompanySettings
{
    public partial class CompanySettingsForm : ServerMasterPage
    {
        Session session;
        ICompanySettingsRepository companySettingsRepo;
        Nastavitve model;
        int action = -1;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserSuperAdmin() && !PrincipalHelper.IsUserAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            companySettingsRepo = new CompanySettingsRepository(session);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            model = companySettingsRepo.GetCompanySettings();
            
            if (!IsPostBack)
            {
                if (model != null)
                    FillForm();
                else
                    SetFormDefaultValues();
            }

            if(model == null)
                action = (int)Enums.UserAction.Add;
            else
                action = (int)Enums.UserAction.Edit;
        }

        private void FillForm()
        {
            txtPayout.Text = model.Izplacilo.ToString();
            txtQuotient.Text = model.Kolicnik.ToString();
            CheckBoxPosiljanjePoste.Checked = model.MailPosiljanje;
            ASPxMemoOpombe.Text = model.Opombe;
            txtKVPNumber.Text = model.StevilkaKVP.ToString();
        }

        private void SetFormDefaultValues()
        {
            action = (int)Enums.UserAction.Add;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Nastavitve(session);
                model.NastavitveID = 0; 
            }

            model.Izplacilo = CommonMethods.ParseDecimal(txtPayout.Text);
            model.Kolicnik = CommonMethods.ParseDecimal(txtQuotient.Text);
            model.MailPosiljanje = CheckBoxPosiljanjePoste.Checked;
            model.Opombe = ASPxMemoOpombe.Text;
            model.StevilkaKVP = CommonMethods.ParseInt(txtKVPNumber.Text);

            companySettingsRepo.SaveCompanySettings(model);

            return true;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            bool isValid = false;

            switch (action)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    break;
            }

            this.Master.NavigationBarMain.DataBind();
        }
    }
}