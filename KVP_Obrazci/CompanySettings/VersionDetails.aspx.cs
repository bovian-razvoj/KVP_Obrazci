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
    public partial class VersionDetails : ServerMasterPage
    {
        Session session;
        ICompanySettingsRepository companySettingsRepo;
        Nastavitve model;
        int action = -1;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            session = XpoHelper.GetNewSession();

            companySettingsRepo = new CompanySettingsRepository(session);

            if (!PrincipalHelper.IsUserSuperAdmin())
                newVersionCol.Style.Add("display", "none");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            model = companySettingsRepo.GetCompanySettings();

            if (model != null)
                FillForm();
            else
                SetFormDefaultValues();


            if (model == null)
                action = (int)Enums.UserAction.Add;
            else
                action = (int)Enums.UserAction.Edit;
        }

        private void FillForm()
        {
            versionDetails.InnerHtml = model.Verzija;

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

            model.VerzijaStevilka = txtVersionNumber.Text;

            if (String.IsNullOrEmpty(model.Verzija))
                model.Verzija = "";

            model.Verzija = model.Verzija.Insert(0, "<div><strong style='font-size:18px;'>" + model.VerzijaStevilka + "</strong> - <small>" + DateTime.Now.ToString("dd. MMMM yyyy HH:mm") + "</small><div style='margin-left: 30px;'>" + HtmlEditorVersionDetails.Html + "<br /><br /></div></div>");

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
            Response.Redirect(Request.RawUrl);
        }
    }
}