using DevExpress.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.KVPDocuments
{
    public partial class KVPDocumentRedCardForm : ServerMasterPage
    {
        Session session = null;
        KVPDocument model = null;
        int action = -1;
        int kvpDocID = -1;
        IKVPDocumentRepository kvpDocRepo;
        IStatusRepository statusRepo;
        IKVPStatusRepository kvpStatusRepo;
        IEmployeeRepository employeeRepo;

        bool transferToKvpForm = false;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            Master.DisableNavBar = true;

            action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
            kvpDocID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            if (PrincipalHelper.IsUserEmployee())
            {
                ASPxGridLookupLeaderTeam.ClientEnabled = false;
                ASPxGridLookupRealizator.ClientEnabled = false;
                ASPxGridLookupStatusRdeciKarton.ClientEnabled = false;
                ASPxGridLookupStatusRdeciKarton.ClientEnabled = false;
            }

            session = XpoHelper.GetNewSession();
            kvpDocRepo = new KVPDocumentRepository(session);
            statusRepo = new StatusRepository(session);
            kvpStatusRepo = new KVPStatusRepository(session);
            employeeRepo = new EmployeeRepository(session);

            XpoDSEmployee.Session = session;
            XpoDSStatus.Session = session;
            XpoDSTip.Session = session;
            XpoDSRedCardType.Session = session;
            XpoDSStatusOnlyRK.Session = session;
            XpoDSDepartment.Session = session;
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();

                if (action != (int)Enums.UserAction.Add)
                {
                    if (kvpDocID > 0)
                    {
                        if (GetKVPDocumentProvider().GetKVPDocumentModel() != null)
                            model = GetKVPDocumentProvider().GetKVPDocumentModel();
                        else
                            model = kvpDocRepo.GetKVPByID(kvpDocID);

                        if (model != null)
                        {
                            GetKVPDocumentProvider().SetKVPDocumentModel(model);
                            FillForm();
                        }
                    }
                }
                else //User action => Add
                {
                    navTabs.Attributes["class"] = navTabs.Attributes["class"].ToString() + " disabled";

                    SetFormDefaultValues();
                }
                //UserActionConfirmBtnUpdate(btnConfirm, action);
                //InitializeEditDeleteButtons();
            }
            else
            {
                if (model == null && SessionHasValue(Enums.KVPDocumentSession.KVPDocumentModel))
                    model = GetKVPDocumentProvider().GetKVPDocumentModel();
            }
        }

        private void FillForm()
        {
            DateEditDatumVnosa.ClientEnabled = false;
            DateEditDatumVnosa.Date = model.DatumVnosa;
            ASPxGridLookupType.Value = model.idTip != null ? model.idTip.idTip : 0;
            ASPxGridLookupProposer.Value = model.Predlagatelj != null ? model.Predlagatelj.Id : 0;
            ASPxGridLookupLeaderTeam.Value = CommonMethods.ParseInt(model.vodja_teama);
            ASPxGridLookupRealizator.Value = model.Realizator != null ? model.Realizator.Id : 0;
            ASPxGridLookupDepartment.Value = model.Predlagatelj != null ? (model.Predlagatelj.DepartmentId != null ? model.Predlagatelj.DepartmentId.Id : 0) : 0;

            ASPxMemoProblemDesc.Text = model.OpisProblem;
            ASPxMemoImprovementProposition.Text = model.PredlogIzboljsave;
            ASPxMemoSavingsOrCosts.Text = model.PrihranekStroski;

            ASPxGridLookupTipRdeciKarton.Value = model.idTipRdeciKarton != null ? model.idTipRdeciKarton.idTipRdeciKarton : 0;

            DateEditDatumPopravila.Date = model.RokOdziva;

            txtOpisLokacija.Text = model.OpisLokacija;
            ASPxMemoPredlogPopravila.Text = model.PredlogPopravila;

            txtStrojStevilka.Text = model.StrojStevilka;
            txtStroj.Text = model.Stroj;
            txtLinija.Text = model.Linija;

            var status = kvpStatusRepo.GetKVPRedCardStatus(model);
            if (status != null)
            {
                ASPxGridLookupStatusRdeciKarton.Value = status.idStatus.idStatus;
            }
        }

        #region Initialization

        private void Initialize()
        {
            
        }

        private void InitializeEditDeleteButtons()
        {
            //VOZILA
            /*if (model == null || (model.Vozila == null || model.Vozila.Count <= 0))
            {
                EnabledDeleteAndEditBtnPopUp(btnEditVehicle, btnDeleteVehicle);
            }
            else if (!btnEditVehicle.Enabled && !btnDeleteVehicle.Enabled)
            {
                EnabledDeleteAndEditBtnPopUp(btnEditVehicle, btnDeleteVehicle, false);
            }
            //SHRAMBA
            /*if (model == null || (model. == null || model.Vozila.Count <= 0))
            {
                EnabledDeleteAndEditBtnPopUp(btnEditStorage, btnDeleteStorage);
            }
            else if (!btnEditStorage.Enabled && !btnDeleteStorage.Enabled)
            {
                EnabledDeleteAndEditBtnPopUp(btnEditStorage, btnDeleteStorage, false);
            }
            //PNEVMATIKE
            if (model == null || (model.Pnevmatike == null || model.Pnevmatike.Count <= 0))
            {
                EnabledDeleteAndEditBtnPopUp(btnEditTires, btnDeleteTires);
            }
            else if (!btnEditTires.Enabled && !btnDeleteTires.Enabled)
            {
                EnabledDeleteAndEditBtnPopUp(btnEditTires, btnDeleteTires, false);
            }
            //KONTAKTNE OSEBE
            if (model == null || (model.KontaktneOsebe == null || model.KontaktneOsebe.Count <= 0))
            {
                EnabledDeleteAndEditBtnPopUp(btnEditContactPerson, btnDeleteContactPerson);
            }
            else if (!btnEditContactPerson.Enabled && !btnDeleteContactPerson.Enabled)
            {
                EnabledDeleteAndEditBtnPopUp(btnEditContactPerson, btnDeleteContactPerson, false);
            }*/
        }

        private void SetFormDefaultValues()
        {
            DateEditDatumVnosa.Date = DateTime.Now;
            ASPxGridLookupProposer.Value = PrincipalHelper.GetUserPrincipal().ID;
            ASPxGridLookupStatusRdeciKarton.Value = statusRepo.GetKVPRedCardStatusIDByCode(Enums.RedCardStatus.ODPRTO.ToString());

            Departments department = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID).DepartmentId;
            if (department != null)
            {
                ASPxGridLookupLeaderTeam.Value = department.DepartmentHeadId;
                ASPxGridLookupDepartment.Value = department.Id;
            }
        }
        #endregion

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new KVPDocument(session);
                model.idKVPDocument = 0;
                model.DatumVnosa = DateEditDatumVnosa.Date;
            }
            else if (model == null && !add)
            {
                model = GetKVPDocumentProvider().GetKVPDocumentModel();
                model.idKVPDocument = model.idKVPDocument;

            }

            int typeID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupType));
            if (model.idTip != null)
                model.idTip = kvpDocRepo.GetTypeByID(typeID, model.idTip.Session);
            else
                model.idTip = kvpDocRepo.GetTypeByID(typeID);


            int proposerID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupProposer));
            if (model.Predlagatelj != null)
                model.Predlagatelj = kvpDocRepo.GetEmployeeByID(proposerID, model.Predlagatelj.Session);
            else
                model.Predlagatelj = kvpDocRepo.GetEmployeeByID(proposerID);

            int realizatorID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealizator));
            if (model.Realizator != null)
                model.Realizator = kvpDocRepo.GetEmployeeByID(realizatorID, model.Realizator.Session);
            else
                model.Realizator = kvpDocRepo.GetEmployeeByID(realizatorID);

            int rdeciKartonTipId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupTipRdeciKarton));
            if (model.idTipRdeciKarton != null)
                model.idTipRdeciKarton = kvpDocRepo.GetRedCardTypeByID(rdeciKartonTipId, model.idTipRdeciKarton.Session);
            else
                model.idTipRdeciKarton = kvpDocRepo.GetRedCardTypeByID(rdeciKartonTipId);

            //model.StevilkaKVP = 
            model.vodja_teama = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupLeaderTeam));
            model.OpisProblem = ASPxMemoProblemDesc.Text;
            model.PredlogIzboljsave = ASPxMemoImprovementProposition.Text;
            model.PrihranekStroski = ASPxMemoSavingsOrCosts.Text;
            //model.predlog_izboljsave2 = 
            model.RokOdziva = DateEditDatumPopravila.Date; //uporabljamo za rok popravila

            model.OpisLokacija = txtOpisLokacija.Text;
            model.PredlogPopravila = ASPxMemoPredlogPopravila.Text;

            model.StrojStevilka = txtStrojStevilka.Text;
            model.Stroj = txtStroj.Text;
            model.Linija = txtLinija.Text;

            if (transferToKvpForm)
            {
                model.idTipRdeciKarton = null;
                model.RokOdziva = DateTime.MinValue;
            }


            model.idKVPDocument = kvpDocRepo.SaveKVP(model);
            
            int statusID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupStatusRdeciKarton));
            if (statusID > 0)
            {
                KVP_Status tmp = kvpStatusRepo.GetKVPRedCardStatus(model.idKVPDocument);
                if (tmp != null && tmp.idStatus.idStatus != statusID)//če kvp status že obstaja v tabeli in je uporabnik spremenil prvotni status potem je potrebno zapis v bazi samo posodobiti 
                {
                    tmp.idStatus = statusRepo.GetStatusByID(statusID, tmp.idStatus.Session);
                    kvpStatusRepo.SaveKVPStatus(tmp);
                }
                else if (tmp == null)//Če kvp status še sploh ni v bazi
                {
                    tmp = new KVP_Status(session);
                    tmp.idStatus = statusRepo.GetStatusByID(statusID);
                    tmp.idKVPDocument = kvpDocRepo.GetKVPByID(model.idKVPDocument, session);
                    tmp.idKVP_Status = 0;

                    kvpStatusRepo.SaveKVPStatus(tmp);
                }
            }

            GetKVPDocumentProvider().SetKVPDocumentModel(model);

            return true;
        }


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            bool isValid = false;
            bool isDeleteing = false;

            switch (action)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    break;
                case (int)Enums.UserAction.Delete:
                    kvpDocRepo.DeleteKVP(kvpDocID);
                    isValid = true;
                    isDeleteing = true;
                    break;
            }

            if (isValid)
            {
                ClearSessionsAndRedirect(isDeleteing);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSessionsAndRedirect();
        }

        private void ClearSessionsAndRedirect(bool isIDDeleted = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = kvpDocID.ToString() } 
            };

            if (isIDDeleted)
                redirectString = "../Dashboard.aspx";
            else
                redirectString = GenerateURI("../Dashboard.aspx", queryStrings);

            List<Enums.KVPDocumentSession> list = Enum.GetValues(typeof(Enums.KVPDocumentSession)).Cast<Enums.KVPDocumentSession>().ToList();
            ClearAllSessions(list, redirectString);
        }

        protected void CallbackPanelKVPDocumentForm_Callback(object sender, CallbackEventArgsBase e)
        {
            string[] split = e.Parameter.Split(';');
            if (split[0] == "TipRdeciKarton")
            {
                DateTime repairDate = DateTime.MinValue;

                if (split[1] == Enums.RedCardType.MANJSE.ToString())
                {
                    repairDate = DateEditDatumVnosa.Date.AddDays(14);
                }
                else if (split[1] == Enums.RedCardType.VECJE.ToString())
                {
                    repairDate = DateEditDatumVnosa.Date.AddDays(28);
                }
                else if (split[1] == Enums.RedCardType.VARNOST.ToString())
                {
                    repairDate = DateEditDatumVnosa.Date;
                }

                CallbackPanelKVPDocumentForm.JSProperties["cpRepairDate"] = repairDate;
            }
        }

        protected void btnPrenosKVPForm_Click(object sender, EventArgs e)
        {
            transferToKvpForm = true;
            btnConfirm_Click(null, EventArgs.Empty);
        }
    }
}