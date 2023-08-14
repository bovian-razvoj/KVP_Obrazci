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
        string sStevilkaRK = "";
        IKVPDocumentRepository kvpDocRepo;
        IStatusRepository statusRepo;
        IKVPStatusRepository kvpStatusRepo;
        IEmployeeRepository employeeRepo;
        ILocationRepository locationRepo;
        ILineRepository lineRepo;
        IMachineRepository machineRepo;
        IKVPGroupsRepository kvpGroupsRepo;
        IMessageProcessorRepository messageProcessRepo;

        bool transferToKvpForm = false;
        bool submitToTPMAdmin = false;

        bool saveKVPAndAddAttachment = false;

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

            if (PrincipalHelper.IsUserAlsoTpmAdmin())
            {
                ASPxGridLookupLeaderTeam.ClientEnabled = false;
                ASPxGridLookupRealizator.ClientEnabled = true;
                ASPxGridLookupStatusRdeciKarton.ClientEnabled = false;
                ASPxGridLookupStatusRdeciKarton.ClientEnabled = false;
            }


            session = XpoHelper.GetNewSession();
            kvpDocRepo = new KVPDocumentRepository(session);
            statusRepo = new StatusRepository(session);
            kvpStatusRepo = new KVPStatusRepository(session);
            employeeRepo = new EmployeeRepository(session);
            locationRepo = new LocationRepository(session);
            lineRepo = new LineRepository(session);
            machineRepo = new MachineRepository(session);
            kvpGroupsRepo = new KVPGroupsRepository(session);
            messageProcessRepo = new MessageProcessorRepository(session);

            XpoDSEmployee.Session = session;
            XpoDSStatus.Session = session;
            XpoDSTip.Session = session;
            XpoDSRedCardType.Session = session;
            XpoDSStatusOnlyRK.Session = session;
            XpoDSDepartment.Session = session;
            XpoDSLocation.Session = session;
            XpoDSLine.Session = session;
            XpoDSMachine.Session = session;
            XpoDSKVPComment.Session = session;


            ASPxGridLookupProposer.GridView.Settings.GridLines = GridLines.Both;

            ShowRequiredFields();
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
                        if (GetKVPDocumentProvider().GetRedCardDocumentModel() != null)
                            model = GetKVPDocumentProvider().GetRedCardDocumentModel();
                        else
                            model = kvpDocRepo.GetKVPByID(kvpDocID);

                        if (model != null)
                        {
                            GetKVPDocumentProvider().SetRedCardDocumentModel(model);
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
                if (model == null && SessionHasValue(Enums.KVPDocumentSession.RedCardDocumentModel))
                    model = GetKVPDocumentProvider().GetRedCardDocumentModel();
            }

            CustomizeByUserRole();
        }

        private void FillForm()
        {
            DateEditDatumVnosa.Date = model.DatumVnosa;

            //ASPxGridLookupType.Value = model.idTip != null ? model.idTip.idTip : 0;
            ASPxGridLookupProposer.Value = model.Predlagatelj != null ? model.Predlagatelj.Id : 0;
            ASPxGridLookupLeaderTeam.Value = CommonMethods.ParseInt(model.vodja_teama);
            ASPxGridLookupRealizator.Value = model.Realizator != null ? model.Realizator.Id : 0;
            ASPxGridLookupDepartment.Value = model.Predlagatelj != null ? (model.Predlagatelj.DepartmentId != null ? model.Predlagatelj.DepartmentId.Id : 0) : 0;

            ASPxMemoFailureDesc.Text = model.OpisNapakeRK;
            ASPxMemoActivity.Text = model.AktivnostRK;
            //ASPxMemoSavingsOrCosts.Text = model.PrihranekStroski;

            ASPxGridLookupTipRdeciKarton.Value = model.idTipRdeciKarton != null ? model.idTipRdeciKarton.idTipRdeciKarton : 0;

            DateEditDatumPopravila.Date = model.RokOdziva;

            ASPxGridLookupLocation.Value = model.LokacijaID != null ? model.LokacijaID.idLokacija : 0;
            ASPxGridLookupLine.Value = model.LinijaID != null ? model.LinijaID.idLinija : 0;
            ASPxGridLookupMachine.Value = model.StrojID != null ? model.StrojID.idStroj : 0;
            txtStrojStevilka.Text = model.StrojStevilka;

            ASPxMemoFailureDesc.Text = model.OpisNapakeRK;
            ASPxMemoActivity.Text = model.AktivnostRK;
            CheckBoxSecurity.Checked = model.VarnostRK;

            //txtOpisLokacija.Text = model.OpisLokacija;
            //ASPxMemoPredlogPopravila.Text = model.PredlogPopravila;

            //txtStrojStevilka.Text = model.StrojStevilka;
            //txtStroj.Text = model.Stroj;
            //txtLinija.Text = model.Linija;

            var status = kvpStatusRepo.GetKVPRedCardStatus(model);
            if (status != null)
            {
                ASPxGridLookupStatusRdeciKarton.Value = status.idStatus.idStatus;
            }

            if (model.StevilkaKVP.ToUpper().Contains("P"))
            {
                RCNumberingType.SelectedIndex = RCNumberingType.Items.FindByValue(Enums.RedCardNumberingType.Manual.ToString()).Index;
                txtRCNumberManual.Text = model.StevilkaKVP;
            }
            else if (model.StevilkaKVP.ToUpper().Contains("E"))
            {
                RCNumberingType.SelectedIndex = RCNumberingType.Items.FindByValue(Enums.RedCardNumberingType.System.ToString()).Index;
                txtRCNumberSystem.Text = model.StevilkaKVP;
            }

            HtmlGenericControl control = (HtmlGenericControl)historyStatusItem.FindControl("historyStatusBadge");
            control.InnerText = model.KVP_Statuss.Count.ToString();

            CustomizeByRCStatus();

            //if the champion has come from gridview where he sees all kvp's than we have to disable all functionalities (read-only)
            if (IsActiveTabAllKVPs())
            {
                SetEnableAllButtons(false);
            }

            btnPrintRedCard.ClientVisible = true;
        }

        #region Initialization

        private void Initialize()
        {
            if (SessionHasValue(Enums.CommonSession.DownloadDocument))
            {
                DocumentEntity obj = (DocumentEntity)GetValueFromSession(Enums.CommonSession.DownloadDocument);

                byte[] byteFile = File.ReadAllBytes(Server.MapPath(obj.Url));
                string resultExtension = Path.GetExtension(obj.Name);
                string format = "pdf";
                if (resultExtension.Equals(".jpg"))
                    format = "jpg";
                else if (resultExtension.Equals(".jpeg"))
                    format = "jpeg";
                else if (resultExtension.Equals(".png"))
                    format = "png";
                RemoveSession(Enums.CommonSession.DownloadDocument);
                WriteDocumentToResponse(byteFile, format, false, obj.Name);
            }
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

            ASPxGridLookupTipRdeciKarton.Value = kvpDocRepo.GetRedCardTypeByCode(Enums.RedCardType.MANJSE.ToString()).idTipRdeciKarton;
            DateEditDatumPopravila.Date = DateEditDatumVnosa.Date.AddDays(14);//za manjse popravilo je časa 14 dni

            if (PrincipalHelper.IsUserTpmAdmin() || PrincipalHelper.IsUserAlsoTpmAdmin())
                RCNumberingType.SelectedIndex = RCNumberingType.Items.FindByValue(Enums.RedCardNumberingType.Manual.ToString()).Index;
            else
                RCNumberingType.SelectedIndex = RCNumberingType.Items.FindByValue(Enums.RedCardNumberingType.System.ToString()).Index;

            btnSubmitRedCard.ClientVisible = true;
        }
        #endregion

        private bool AddOrEditEntityObject(bool add = false, Enums.RedCardStatus redCardStatus = Enums.RedCardStatus.PRESERVE_STATUS)
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

            /*int typeID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupType));
            if (model.idTip != null)
                model.idTip = kvpDocRepo.GetTypeByID(typeID, model.idTip.Session);
            else
                model.idTip = kvpDocRepo.GetTypeByID(typeID);
            */

            int proposerID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupProposer));
            if (model.Predlagatelj != null)
                model.Predlagatelj = kvpDocRepo.GetEmployeeByID(proposerID, model.Predlagatelj.Session);
            else
                model.Predlagatelj = kvpDocRepo.GetEmployeeByID(proposerID);

            int realizatorID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealizator));
            if (model.Realizator != null)
                model.Realizator = kvpDocRepo.GetEmployeeByID(realizatorID, model.Realizator.Session);
            else
                model.Realizator = kvpDocRepo.GetEmployeeByID(realizatorID, model.Session);

            int rdeciKartonTipId = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupTipRdeciKarton));
            if (model.idTipRdeciKarton != null)
                model.idTipRdeciKarton = kvpDocRepo.GetRedCardTypeByID(rdeciKartonTipId, model.idTipRdeciKarton.Session);
            else
                model.idTipRdeciKarton = kvpDocRepo.GetRedCardTypeByID(rdeciKartonTipId);


            //K predlagatelju še shranimo njegovo KVP skupino
            if (add)
            {
                KVPSkupina_Users kvpGroupUser = kvpGroupsRepo.GetKVPGroupUserByUserID(model.Predlagatelj.Id, model.Session);
                if (kvpGroupUser != null)
                    model.KVPSkupinaID = kvpGroupUser.idKVPSkupina;
                else
                    throw new Exception("&%messageType=2&% There is no KVPGroup for User/Proposer: " + PrincipalHelper.GetUserPrincipal().firstName + " " + PrincipalHelper.GetUserPrincipal().lastName);
            }


            model.StevilkaKVP = GetTextBoxByNumberingType().Text;

            model.vodja_teama = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupLeaderTeam));

            model.AktivnostRK = ASPxMemoActivity.Text;
            model.RokOdziva = DateEditDatumPopravila.Date; //uporabljamo za rok popravila

            model.OpisNapakeRK = ASPxMemoFailureDesc.Text;

            model.StrojStevilka = txtStrojStevilka.Text;


            int locationID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupLocation));
            if (model.LokacijaID != null)
                model.LokacijaID = locationRepo.GetLocationByID(locationID, model.LokacijaID.Session);
            else
                model.LokacijaID = locationRepo.GetLocationByID(locationID, model.Session);

            int lineID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupLine));
            if (model.LinijaID != null)
                model.LinijaID = lineRepo.GetLineByID(lineID, model.LinijaID.Session);
            else
                model.LinijaID = lineRepo.GetLineByID(lineID, model.Session);

            int machineID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupMachine));
            if (model.StrojID != null)
                model.StrojID = machineRepo.GetMachineByID(machineID, model.StrojID.Session);
            else
                model.StrojID = machineRepo.GetMachineByID(machineID, model.Session);

            if (transferToKvpForm)
            {
                model.OpisProblem = "Prenos iz rdečega kartona.";
                model.PredlogIzboljsave = model.OpisNapakeRK;
                model.idTipRdeciKarton = null;
                model.RokOdziva = DateTime.MinValue;
            }

            bool isNumberingTypeSystem = (RCNumberingType.SelectedItem.Value.ToString() == Enums.RedCardNumberingType.System.ToString());
            if (!isNumberingTypeSystem && add)
            {
                bool existNum = kvpDocRepo.ExistManualRCNumber(txtRCNumberManual.Text);
                if (!existNum)
                    model.StevilkaKVP = txtRCNumberManual.Text;
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Exception", String.Format("$('#expModal').modal('show')"), true);
                    return false;
                }
            }

            model.VarnostRK = CheckBoxSecurity.Checked;
            bool firstVarnostRK = false;
            if (model.VarnostRK && model.DatumPoslanePosteZaVarnost.CompareTo(DateTime.MinValue) == 0)
            {
                firstVarnostRK = true;
                model.DatumPoslanePosteZaVarnost = DateTime.Now;
            }

            if (redCardStatus == Enums.RedCardStatus.IZVRSEN)
            {
                model.DatumZakljuceneIdeje = DateTime.Now;
            }


                if (redCardStatus != Enums.RedCardStatus.PRESERVE_STATUS)
            {
                model.LastStatusId = statusRepo.GetStatusByCode(redCardStatus.ToString(), model.Session);

                kvpDocID = model.idKVPDocument = kvpDocRepo.SaveRC(model, Enums.SubmitRedCardType.SaveNewStatus, isNumberingTypeSystem);
            }
            else
                kvpDocID = model.idKVPDocument = kvpDocRepo.SaveRC(model, Enums.SubmitRedCardType.OnlySaveProposal, isNumberingTypeSystem);

            //za potrebe maila potrebujemo številko RK.
            if (firstVarnostRK)
                messageProcessRepo.ProcessSecurityInfoRedCardMailToSend(model, model.Session);

            /*int statusID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupStatusRdeciKarton));
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
            }*/

            sStevilkaRK = model.StevilkaKVP;

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
                    isValid = AddOrEditEntityObject(true, Enums.RedCardStatus.ODPRTO);
                    submitToTPMAdmin = true;
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
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = kvpDocID.ToString() },
                new QueryStrings() { Attribute = Enums.QueryStringName.stRK.ToString(), Value = sStevilkaRK}
            };


            if (submitToTPMAdmin)
            {
                queryStrings.Add(new QueryStrings() { Attribute = Enums.QueryStringName.successMessage.ToString(), Value = "true" });
            }

            

            if (isIDDeleted)
                redirectString = "../DashboardRedCards.aspx";
            else
                redirectString = GenerateURI("../DashboardRedCards.aspx", queryStrings);

            if (saveKVPAndAddAttachment)
            {
                queryStrings.Add(new QueryStrings() { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                queryStrings.Add(new QueryStrings() { Attribute = "addAttachment", Value = "true" });
                redirectString = GenerateURI("KVPDocumentRedCardForm.aspx", queryStrings);
            }

            List<Enums.KVPDocumentSession> list = Enum.GetValues(typeof(Enums.KVPDocumentSession)).Cast<Enums.KVPDocumentSession>().ToList();
            ClearAllSessions(list, redirectString, CommonMethods.IsCallbackRequest(this.Request));
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
            else if (split[0] == "GetHistoryStatuses")
            {
                if (model != null)
                {
                    List<KVP_Status> list = kvpStatusRepo.GetKVPStatusesBYDocID(model.idKVPDocument);

                    var query = from s in list
                                select new
                                {
                                    date = s.ts.ToString("dd. MMMM yyyy - HH:mm"),
                                    content = s.idStatus.Naziv + "<div><small>" + (!String.IsNullOrEmpty(s.Opomba) ? s.Opomba : "") + "</small></div> <div style='padding-top:10px;'><em>" + (s.IDPrijava != null ? (s.IDPrijava.Lastname + "  " + s.IDPrijava.Firstname) : "") + "</em></div>"

                                };
                    string json = JsonConvert.SerializeObject(query.ToList());
                    CallbackPanelKVPDocumentForm.JSProperties["cpStatusHistory"] = json;
                }
            }
            else if (split[0] == "ProposalTheSameAsLeader")
            {
                int departmentHeadID = employeeRepo.GetDepartmentHeadID();

                ASPxGridLookupLeaderTeam.Value = departmentHeadID;
            }
            
            else if (split[0] == "InfoMailPopup")
            {
                AddValueToSession(Enums.KVPDocumentSession.KVPDocumentID, model != null ? model.StevilkaKVP : "");
                ASPxPopupControlSendInfoMail.ShowOnPageLoad = true;
            }
            else if (split[0] == "SecurityCheckBoxChanged")
            {
                if (CheckBoxSecurity.Checked)
                {
                    ASPxGridLookupTipRdeciKarton.ClientEnabled = false;
                    ASPxGridLookupTipRdeciKarton.Value = kvpDocRepo.GetRedCardTypeByCode(Enums.RedCardType.MANJSE.ToString()).idTipRdeciKarton;
                }
                else
                {
                    if (PrincipalHelper.IsUserTpmAdmin() || PrincipalHelper.IsUserAlsoTpmAdmin())
                        ASPxGridLookupTipRdeciKarton.ClientEnabled = true;
                }
            }
            else if (e.Parameter == "PrintRedCard")
            {
                CallbackPanelKVPDocumentForm.JSProperties["cpPrintID"] = ConcatenateURLForPrint(kvpDocID, "KVPDocumentRedCard", true);
            }

            if ((action != (int)Enums.UserAction.Add) && (model != null))
            {
                HtmlGenericControl control = (HtmlGenericControl)historyStatusItem.FindControl("historyStatusBadge");
                control.InnerText = kvpDocRepo.GetKVPByID(model.idKVPDocument, session).KVP_Statuss.Count.ToString();
            }

            else if (e.Parameter == "SaveAndAddAttachment")
            {
                saveKVPAndAddAttachment = true;
                btnConfirm_Click(this, e);
            }
        }

        protected void btnPrenosKVPForm_Click(object sender, EventArgs e)
        {
            transferToKvpForm = true;
            btnConfirm_Click(null, EventArgs.Empty);
        }

        protected void ASPxGridViewKVPComments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "CommentAdd")
            {
                bool redirect = false;
                KVPKomentarji comment = new KVPKomentarji(session);

                if (action == (int)Enums.UserAction.Add)
                {
                    bool isOK = AddOrEditEntityObject(true);
                    redirect = isOK;

                    if (!isOK)
                    {
                        ASPxGridViewKVPComments.JSProperties["cp_error"] = true;
                        return;
                    }
                }


                comment.Koda = Enums.KVPCommentCode.LeaderNotes.ToString();
                comment.KVPDocId = kvpDocRepo.GetKVPByID(kvpDocID, session);
                comment.KVPKomentarjiID = 0;
                comment.Opombe = ASPxMemoNotesLeader.Text;
                comment.UserId = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, session);
                kvpDocRepo.SaveKVPComment(comment);
                ASPxGridViewKVPComments.DataBind();

                if (redirect)
                    ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocumentRedCardForm.aspx", (int)Enums.UserAction.Edit, kvpDocID));

            }
        }

        protected void ASPxGridViewKVPComments_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void ASPxGridViewKVPComments_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int commentID = CommonMethods.ParseInt(ASPxGridViewKVPComments.GetRowValues(e.VisibleIndex, "KVPKomentarjiID"));
            kvpDocRepo.DeleteKVPComment(commentID);
            ASPxGridViewKVPComments.DataBind();
        }

        protected void ASPxPopupControlSendInfoMail_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.KVPDocumentSession.KVPDocumentID);
        }

        public void CustomizeByUserRole()
        {
            if (PrincipalHelper.IsUserSuperAdmin()) return;

            if (PrincipalHelper.IsUserTpmAdmin() || PrincipalHelper.IsUserAlsoTpmAdmin())
            {
                //samo ob dodajanju novega rdečega kartona lahko omogočimo nastavitev tipa številčenja
                if (action == (int)Enums.UserAction.Add)
                {
                    lblRedCardNumbering.ClientVisible = true;
                    RCNumberingType.ClientVisible = true;
                }

                //če je rdeči karton v fazi dodajanja ali če je že shranjen in ima status ODPRTO potem lahko tpm administratorju omogočimo podati RK v realizacijo
                if (action == (int)Enums.UserAction.Add || (model != null && model.LastStatusId.Koda == Enums.RedCardStatus.ODPRTO.ToString()))
                {
                    btnSetRealizationRC.ClientVisible = true;
                }

                //če je rdeči karton že reliziran potem tpm administratorju omogočimo zaključitev rdečega kartona
                if (model != null && model.LastStatusId.Koda == Enums.RedCardStatus.RK_REALIZIRAN.ToString())
                    btnCompleteRC.ClientVisible = true;

                if (model != null && model.LastStatusId.Koda == Enums.RedCardStatus.ODPRTO.ToString())
                    CheckBoxSecurity.ClientEnabled = true;

                ASPxGridLookupLeaderTeam.ClientEnabled = false;

                btnSubmitRedCard.ClientVisible = false;
                btnConfirm.ClientVisible = false;
            }
            else
            {
                ASPxGridLookupProposer.ClientEnabled = false;
                ASPxGridLookupLeaderTeam.ClientEnabled = false;
                ASPxGridLookupTipRdeciKarton.ClientEnabled = false;
                DateEditDatumPopravila.ClientEnabled = false;
                ASPxGridLookupRealizator.ClientEnabled = (PrincipalHelper.IsUserLeader() || PrincipalHelper.IsUserAlsoTpmAdmin()) ? true : false;
                ASPxMemoActivity.ClientEnabled = false;
                btnPrenosKVPForm.ClientVisible = false;
            }

            //če je koda rdečega kartona RK_V_REALIZACIJI in da je vpisani uporabnik enak realizatorju RK-ja poetem omogočimo realizacijo RK-ja
            if ((model != null && model.LastStatusId.Koda == Enums.RedCardStatus.RK_V_REALIZACIJI.ToString()) && model.Realizator.Id == PrincipalHelper.GetUserPrincipal().ID ||
                (model != null && model.LastStatusId.Koda == Enums.RedCardStatus.RK_V_REALIZACIJI.ToString()) && (PrincipalHelper.IsUserTpmAdmin() || PrincipalHelper.IsUserAlsoTpmAdmin()))
                btnRealizeRC.ClientVisible = true;
        }

        public void CustomizeByRCStatus()
        {
            if (PrincipalHelper.IsUserSuperAdmin()) return;

            if (model.LastStatusId.Koda == Enums.RedCardStatus.ODPRTO.ToString())
            {
                if (PrincipalHelper.IsUserTpmAdmin() || PrincipalHelper.IsUserAlsoTpmAdmin())
                {

                }
                else
                {
                    ASPxGridLookupLocation.ClientEnabled = false;
                    ASPxGridLookupLine.ClientEnabled = false;
                    ASPxGridLookupMachine.ClientEnabled = false;
                    txtStrojStevilka.ClientEnabled = false;
                    ASPxMemoFailureDesc.ClientEnabled = false;
                    CheckBoxSecurity.ClientEnabled = false;
                }
            }
            else if (model.LastStatusId.Koda == Enums.RedCardStatus.IZVRSEN.ToString() || model.LastStatusId.Koda == Enums.RedCardStatus.RK_V_REALIZACIJI.ToString() || model.LastStatusId.Koda == Enums.RedCardStatus.RK_REALIZIRAN.ToString())
            {
                ASPxGridLookupProposer.ClientEnabled = false;
                ASPxGridLookupLeaderTeam.ClientEnabled = false;
                ASPxGridLookupTipRdeciKarton.ClientEnabled = false;
                DateEditDatumPopravila.ClientEnabled = false;
                ASPxGridLookupRealizator.ClientEnabled = (PrincipalHelper.IsUserLeader() || PrincipalHelper.IsUserAlsoTpmAdmin() ) ? true : false;
                ASPxMemoActivity.ClientEnabled = false;
                btnPrenosKVPForm.ClientVisible = false;

                ASPxGridLookupLocation.ClientEnabled = false;
                ASPxGridLookupLine.ClientEnabled = false;
                ASPxGridLookupMachine.ClientEnabled = false;
                txtStrojStevilka.ClientEnabled = false;
                ASPxMemoFailureDesc.ClientEnabled = false;
                CheckBoxSecurity.ClientEnabled = false;
            }
        }

        public ASPxTextBox GetTextBoxByNumberingType()
        {
            object selectedValue = RCNumberingType.Value;
            if (selectedValue != null)
            {
                if (selectedValue.ToString() == Enums.RedCardNumberingType.System.ToString())
                    return txtRCNumberSystem;
                else
                    return txtRCNumberManual;

            }
            else
                return txtRCNumberSystem;
        }

        private bool? HasRedCardSystemNumbering()
        {
            if (model != null)
            {
                if (model.StevilkaKVP.StartsWith("P"))
                    return false;
                else if (model.StevilkaKVP.StartsWith("E"))
                    return true;
                else//v tem primeru imamo stevilkoKVP-ja ki pripada kvp dokumentu
                    return null;
            }
            else
                return true;
        }

        protected void btnSetRealizationRC_Click(object sender, EventArgs e)
        {
            bool userActionAdd = (action == (int)Enums.UserAction.Add);

            bool isOK = AddOrEditEntityObject(userActionAdd, Enums.RedCardStatus.RK_V_REALIZACIJI);
            if (isOK)
                ClearSessionsAndRedirect();
        }

        protected void btnRealizeRC_Click(object sender, EventArgs e)
        {
            AddOrEditEntityObject(false, Enums.RedCardStatus.RK_REALIZIRAN);
            ClearSessionsAndRedirect();
        }

        protected void btnCompleteRC_Click(object sender, EventArgs e)
        {
            AddOrEditEntityObject(false, Enums.RedCardStatus.IZVRSEN);
            ClearSessionsAndRedirect();
        }
        protected void btnSubmitRedCard_Click(object sender, EventArgs e)
        {
            btnConfirm_Click(sender, e);
        }

        #region Attachments
        protected void RedCardAttachments_PopulateAttachments(object sender, EventArgs e)
        {
            if (model != null && !String.IsNullOrEmpty(model.Priloge))
            {
                List<DocumentEntity> list = new List<DocumentEntity>();
                DocumentEntity document = null;
                string[] split = model.Priloge.Split('|');
                string resultExtension = "";
                foreach (var item in split)
                {
                    string[] fileData = item.Split(';');
                    document = new DocumentEntity();
                    document.Url = fileData[0];
                    document.Name = fileData[1];

                    resultExtension = Path.GetExtension(fileData[1]);
                    if (resultExtension.Equals(".png") || resultExtension.Equals(".jpg") || resultExtension.Equals(".jpeg"))
                        document.isImage = true;

                    list.Add(document);
                }
                (sender as UploadAttachment).files = list;
                HtmlGenericControl control = (HtmlGenericControl)attachmentsItem.FindControl("attachmentsBadge");
                control.InnerText = list.Count.ToString();
            }
            (sender as UploadAttachment).ActiveDropZoneID = "active-drop-zone";
        }

        protected void RedCardAttachments_UploadComplete(object sender, EventArgs e)
        {
            model = GetKVPDocumentProvider().GetRedCardDocumentModel();
            if (model != null)
            {
                string pipe = "";
                if (!String.IsNullOrEmpty(model.Priloge))
                    pipe = "|";

                kvpDocRepo = new KVPDocumentRepository(model.Session);
                model.Priloge += pipe + (sender as UploadAttachment).currentFile.Url + ";" + (sender as UploadAttachment).currentFile.Name;
                kvpDocRepo.SaveRC(model);
                GetKVPDocumentProvider().SetKVPDocumentModel(model);
                HtmlGenericControl control = (HtmlGenericControl)attachmentsItem.FindControl("attachmentsBadge");
                control.InnerText = model.Priloge.Split('|').Length.ToString();
            }
        }

        protected void RedCardAttachments_DeleteAttachments(object sender, EventArgs e)
        {
            model = GetKVPDocumentProvider().GetRedCardDocumentModel();
            if (model != null)
            {
                int hasPipe = 0;
                string fileToDelete = (sender as UploadAttachment).currentFile.Name;
                DocumentEntity obj = GetAttachmentFromDB(fileToDelete);

                if (obj != null)
                {
                    string item = obj.Url + ";" + obj.Name;
                    string strPhysicalFolder = Server.MapPath(obj.Url);
                    if (File.Exists(strPhysicalFolder))
                        File.Delete(strPhysicalFolder);

                    if (model.Priloge.Contains("|"))
                        hasPipe = 1;
                    else
                        hasPipe = 0;

                    model.Priloge = model.Priloge.Remove(model.Priloge.IndexOf(item) > 0 ? (model.Priloge.IndexOf(item) - hasPipe) : model.Priloge.IndexOf(item), item.Length + hasPipe);

                    kvpDocRepo = new KVPDocumentRepository(model.Session);
                    kvpDocRepo.SaveRC(model);
                }

            }
        }

        protected void RedCardAttachments_DownloadAttachments(object sender, EventArgs e)
        {
            model = GetKVPDocumentProvider().GetRedCardDocumentModel();
            if (model != null)
            {
                string fileName = (sender as UploadAttachment).currentFile.Name;
                DocumentEntity obj = GetAttachmentFromDB(fileName);

                AddValueToSession(Enums.CommonSession.DownloadDocument, obj);
                //Response.Redirect(Request.RawUrl);
                ASPxWebControl.RedirectOnCallback(Request.RawUrl);
            }
        }

        private DocumentEntity GetAttachmentFromDB(string fileName)
        {
            model = GetKVPDocumentProvider().GetRedCardDocumentModel();
            if (model != null)
            {
                string[] split = model.Priloge.Split('|');
                foreach (var item in split)
                {
                    string[] fileSplit = item.Split(';');
                    if (fileSplit[1].Equals(fileName))
                    {
                        return new DocumentEntity { Url = fileSplit[0], Name = fileSplit[1] };
                    }
                }
            }
            return null;
        }

        private void WriteDocumentToResponse(byte[] documentData, string format, bool isInline, string fileName)
        {
            string contentType = "application/pdf";

            if (format == "png")
                contentType = "image/png";
            else if (format == "jpg" || format == "jpeg")
                contentType = "image/jpeg";

            string disposition = (isInline) ? "inline" : "attachment";

            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
            Response.BinaryWrite(documentData);
            Response.End();
            //Response.Flush(); // Sends all currently buffered output to the client.
            //Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            //HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }
        #endregion


        private void ShowRequiredFields()
        {
            Enums.UserRole userRole = (Enums.UserRole)Enum.Parse(typeof(Enums.UserRole), PrincipalHelper.GetUserPrincipal().Role, true);

            if (action == (int)Enums.UserAction.Add)
            {
                switch (userRole)
                {
                    case Enums.UserRole.Employee:
                        ASPxGridLookupLocation.CssClass = "focus-text-box-input-error";
                        ASPxGridLookupLine.CssClass = "focus-text-box-input-error";
                        ASPxGridLookupMachine.CssClass = "focus-text-box-input-error";
                        ASPxMemoFailureDesc.CssClass = "focus-text-box-input-error";
                        break;
                    case Enums.UserRole.TpmAdmin:

                        if (RCNumberingType.SelectedItem != null && RCNumberingType.SelectedItem.Value == Enums.RedCardNumberingType.Manual.ToString())
                            txtRCNumberManual.CssClass = "focus-text-box-input-error";

                        ASPxGridLookupLocation.CssClass = "focus-text-box-input-error";
                        ASPxGridLookupLine.CssClass = "focus-text-box-input-error";
                        ASPxGridLookupMachine.CssClass = "focus-text-box-input-error";
                        ASPxMemoFailureDesc.CssClass = "focus-text-box-input-error";
                        break;
                    default:
                        ASPxGridLookupLocation.CssClass = "focus-text-box-input-error";
                        ASPxGridLookupLine.CssClass = "focus-text-box-input-error";
                        ASPxGridLookupMachine.CssClass = "focus-text-box-input-error";
                        ASPxMemoFailureDesc.CssClass = "focus-text-box-input-error";
                        break;
                }
            }
        }

        private bool IsActiveTabAllKVPs()
        {
            bool isActive = false;
            if (SessionHasValue(Enums.CommonSession.activeTab))
            {
                string activeTabName = GetStringValueFromSession(Enums.CommonSession.activeTab);

                if (activeTabName == "#AllRC")
                    isActive = true;
            }
            return isActive;
        }

        private void SetEnableAllButtons(bool enable)
        {
            btnAddComment.ClientVisible = enable;
            btnSubmitRedCard.ClientVisible = enable;
            btnCompleteRC.ClientVisible = enable;
            btnRealizeRC.ClientVisible = enable;
            btnSetRealizationRC.ClientVisible = enable;
            btnPrenosKVPForm.ClientVisible = enable;
            btnConfirm.ClientVisible = enable;
            btnSendMail.ClientVisible = enable;
        }
    }
}