using DevExpress.Web;
using DevExpress.Web.Rendering;
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
    public partial class KVPDocumentForm : ServerMasterPage
    {
        Session session = null;
        KVPDocument model = null;
        int action = -1;
        int kvpDocID = -1;
        IKVPDocumentRepository kvpDocRepo;
        IStatusRepository statusRepo;
        IKVPStatusRepository kvpStatusRepo;
        IEmployeeRepository employeeRepo;
        IKVPAuditorRepository auditorsRepo;
        IPayoutsRepository payoutRepo;
        IMessageProcessorRepository messageRepo;
        IKVPGroupsRepository kvpGroupsRepo;
        ILocationRepository locationRepo;
        ILineRepository lineRepo;
        IMachineRepository machineRepo;

        bool transferToRedCard = false;
        bool submitProposalToLeader = false;
        bool realizeKVPProposal = false;
        bool submitOpenNewKVP = false;

        bool submitProposalAndRejectNewKVP = false;
        bool forwardBackKVPs = false;

        bool submitProposalToChampion = false;
        bool completedKVP = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            Master.DisableNavBar = true;

            if (!forwardBackKVPs)
            {
                action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                kvpDocID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            session = XpoHelper.GetNewSession();
            kvpDocRepo = new KVPDocumentRepository(session);
            statusRepo = new StatusRepository(session);
            kvpStatusRepo = new KVPStatusRepository(session);
            employeeRepo = new EmployeeRepository(session);
            auditorsRepo = new KVPAuditorRepository(session);
            payoutRepo = new PayoutsRepository(session);
            messageRepo = new MessageProcessorRepository(session);
            kvpGroupsRepo = new KVPGroupsRepository(session);
            locationRepo = new LocationRepository(session);
            lineRepo = new LineRepository(session);
            machineRepo = new MachineRepository(session);

            CustommizeByUserRole();

            XpoDSEmployee.Session = session;
            XpoDSStatus.Session = session;
            XpoDSTip.Session = session;
            XpoDSDepartment.Session = session;
            XpoDataSourceKVPAuditors.Session = session;
            XpoDSProposer.Session = session;
            XpoDSKVPComment.Session = session;
            XpoDSRealizator.Session = session;
            XpoDSLocation.Session = session;
            XpoDSLine.Session = session;
            XpoDSMachine.Session = session;

            ASPxGridViewKVPComments.Settings.GridLines = GridLines.Both;

            HasPreviuosAndNextKVPs();

            ShowRequiredFields();
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack || forwardBackKVPs)
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
                    //navTabs.Attributes["class"] = navTabs.Attributes["class"].ToString() + " disabled";
                    historyStatusItem.Attributes["class"] = "disabled";
                    changeStatusItem.Attributes["class"] = "disabled";
                    overviewItem.Attributes["class"] = "disabled";
                    attachmentsItem.Attributes["class"] = "disabled";
                    SetFormDefaultValues();
                }
                UserActionConfirmBtnUpdate(btnConfirm, action);
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
            //ASPxMemoSavingsOrCosts.Text = model.PrihranekStroski;
            CheckBoxPrihranekStroski.Checked = model.PrihranekStroskiDA_NE;
            txtKVPStevilka.Text = model.StevilkaKVP;
            //ASPxMemoNotesLeader.Text = model.OpombeVodja;

            txtOpisLokacija.Text = model.OpisLokacija;
            txtLinija.Text = model.Linija;
            txtStroj.Text = model.Stroj;
            txtStrojStevilka.Text = model.StrojStevilka;

            ASPxGridLookupLocation.Value = model.LokacijaID != null ? model.LokacijaID.idLokacija : 0;
            ASPxGridLookupLine.Value = model.LinijaID != null ? model.LinijaID.idLinija : 0;
            ASPxGridLookupMachine.Value = model.StrojID != null ? model.StrojID.idStroj : 0;

            var status = kvpStatusRepo.GetLatestKVPStatus(model);
            if (status != null)
            {
                txtCurrentStatus.Text = status.idStatus.Naziv;
                ASPxGridLookupStatus.Value = status.idStatus.idStatus;
                ASPxMemoOpombe.Text = status.Opomba;

                txtElapsedDays.Text = kvpDocRepo.GetNumberOfElapsedDayFromSubmitingKVP(model).ToString();
            }
            //ASPxGridLookupRealizatorOnStatus.Value = model.Realizator != null ? model.Realizator.Id : 0;

            ASPxMemoCIPOpombe.Text = model.CIPOpombe;

            HtmlGenericControl control = (HtmlGenericControl)historyStatusItem.FindControl("historyStatusBadge");
            control.InnerText = model.KVP_Statuss.Count.ToString();

            int auditorsCount = auditorsRepo.GetKVPAuditorsCountByKVPId(kvpDocID);

            HtmlGenericControl auditorsBadgeControl = (HtmlGenericControl)overviewItem.FindControl("auditorsBadge");
            auditorsBadgeControl.InnerText = auditorsCount.ToString();

            if (status != null && status.idStatus.Koda == Enums.KVPStatuses.VNOS.ToString())
            {
                btnDelete.ClientVisible = true;
            }

            //if the kvp is in status Vnos than we can change it!
            bool isStatusVnos = (status != null && status.idStatus.Koda == Enums.KVPStatuses.VNOS.ToString());
            if (!isStatusVnos)
            {
                ASPxMemoProblemDesc.ClientEnabled = false;
                ASPxMemoImprovementProposition.ClientEnabled = false;
                ASPxMemoSavingsOrCosts.ClientEnabled = false;
                ASPxGridLookupProposer.ClientEnabled = false;
                CheckBoxPrihranekStroski.ClientEnabled = false;
                txtOpisLokacija.ClientEnabled = false;
                txtLinija.ClientEnabled = false;
                txtStroj.ClientEnabled = false;
                ASPxGridLookupLocation.ClientEnabled = false;
                ASPxGridLookupLine.ClientEnabled = false;
                ASPxGridLookupMachine.ClientEnabled = false;
                txtStrojStevilka.ClientEnabled = false;
                ASPxMemoCIPOpombe.ClientEnabled = false;
            }

            if (PrincipalHelper.IsUserEmployee() && !isStatusVnos)
                ASPxGridLookupType.ClientEnabled = false;

            if ((PrincipalHelper.IsUserLeader() || PrincipalHelper.IsUserChampion()) && isStatusVnos)
            {
                ASPxMemoNotesLeader.ClientVisible = false;
                btnAddComment.ClientVisible = false;
                ASPxGridViewKVPComments.ClientVisible = false;
                lblNoteLeader.ClientVisible = false;
            }

            // if leader or champion can check CIP option
            if ((PrincipalHelper.IsUserLeader() || PrincipalHelper.IsUserChampion()) && ((status.idStatus.Koda == Enums.KVPStatuses.ODOBRITEV_VODJA.ToString() || status.idStatus.Koda == Enums.KVPStatuses.V_PREVERJANJE.ToString())))
            {
                CheckBoxPrihranekStroski.ClientEnabled = true;
            }

            // if leader and  then memo 
            if ((PrincipalHelper.IsUserLeader() || PrincipalHelper.IsUserChampion()) && (status.idStatus.Koda == Enums.KVPStatuses.ZAVRNJEN.ToString()))
            {
                CheckBoxPrihranekStroski.ClientEnabled = false;
                ASPxMemoNotesLeader.ClientEnabled = false;
                btnAddComment.ClientEnabled = false;
            }

            if ((status.idStatus.Koda == Enums.KVPStatuses.ZAKLJUCENO.ToString()))
            {
                CheckBoxPrihranekStroski.ClientEnabled = false;
                ASPxMemoNotesLeader.ClientEnabled = false;
                btnAddComment.ClientEnabled = false;
            }

            KVPProccessButtonsVisibility(auditorsCount);

            //določimo da se lahko realizacija izvrši samo v tekočem mesecu
            if (DateEditDatumRealizacije.ClientVisible)
            {
                DateEditDatumRealizacije.Date = DateTime.Now;
            }

            //if the champion has come from gridview where he sees all kvp's than we have to disable all functionalities (read-only)
            if (IsActiveTabChampionAllKVPs())
            {
                SetEnableAllButtons(false);
            }

            if (model.Realizator != null)
            {
                if (model.LastStatusId.Koda == Enums.KVPStatuses.V_REALIZACIJI.ToString() && model.Realizator.Id == PrincipalHelper.GetUserPrincipal().ID)
                {
                    //lookup realizator show only from KVP group which is logedin
                    KVPSkupina_Users obj = kvpGroupsRepo.GetKVPGroupUserByUserID(PrincipalHelper.GetUserPrincipal().ID);
                    if (obj != null)
                    {
                        XpoDSRealizator.Criteria = "([<KVPSkupina_Users>][^.Id = IdUser.Id AND idKVPSkupina.idKVPSkupina = " + PrincipalHelper.GetUserPrincipal().GroupId + "]) OR (RoleID.Koda = '" + Enums.UserRole.Leader.ToString() + "')";
                        //XpoDSRealizator.Criteria = "[<KVPSkupina_Users>][^.Id = IdUser.Id AND idKVPSkupina.idKVPSkupina = " + PrincipalHelper.GetUserPrincipal().GroupId + "]";
                        XpoDSRealizator.Session = session;
                        ASPxGridLookupRealizator.DataBind();
                        ASPxGridLookupRealizator.Value = model.Realizator.Id;
                    }
                }
            }
        }

        #region Initialization

        private void Initialize()
        {
            ASPxGridLookupProposer.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupLeaderTeam.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupRealizator.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupType.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupDepartment.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupLocation.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupLine.GridView.Settings.GridLines = GridLines.Both;
            ASPxGridLookupMachine.GridView.Settings.GridLines = GridLines.Both;


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
            if (PrincipalHelper.IsUserChampion())
            {
                btnSubmitProposalToLeader.ClientVisible = true;
                btnSubmitProposalAndNewKVP.ClientVisible = true;
                btnSaveAndReject.ClientVisible = true;
            }
            else
                btnSubmitProposalToChampion.ClientVisible = true;

            DateEditDatumVnosa.Date = DateTime.Now;
            ASPxGridLookupProposer.Value = PrincipalHelper.GetUserPrincipal().ID;

            Departments department = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID).DepartmentId;
            if (department != null)
            {

                int departmentHeadID = employeeRepo.GetDepartmentHeadID(department);

                ASPxGridLookupLeaderTeam.Value = departmentHeadID;
                ASPxGridLookupDepartment.Value = department.Id;
            }

            Tip generalType = kvpDocRepo.GetTypeByCode(Enums.KVPType.SPL.ToString());
            if (generalType != null)
            {
                ASPxGridLookupType.Value = generalType.idTip;
            }
        }

        private void CustommizeByUserRole()
        {
            if (PrincipalHelper.IsUserEmployee())
            {
                ASPxGridLookupLeaderTeam.ClientEnabled = false;
                ASPxGridLookupRealizator.ClientEnabled = false;

                ASPxGridLookupStatus.ClientEnabled = false;
                //ASPxGridLookupRealizatorOnStatus.ClientEnabled = false;
                txtCurrentStatus.ClientEnabled = false;
                ASPxMemoOpombe.ClientEnabled = false;
                btnConfirmStatus.ClientEnabled = false;
                ASPxGridLookupProposer.ClientEnabled = false;

                btnPrenosRdeciKarton.ClientVisible = false;

                lblNoteLeader.ClientVisible = false;


                ASPxMemoNotesLeader.ClientVisible = false;
                btnAddComment.ClientVisible = false;
                ASPxGridViewKVPComments.ClientVisible = false;
            }
            else if (PrincipalHelper.IsUserLeader())
            {
                ASPxGridLookupLeaderTeam.ClientEnabled = false;
                ASPxGridLookupProposer.ClientEnabled = false;
                btnConfirmStatus.ClientEnabled = false;

                // only if add new KVP or KVP status is VNOS
                if (action == (int)Enums.UserAction.Add)
                {
                    ASPxMemoNotesLeader.ClientVisible = false;
                    lblNoteLeader.ClientVisible = false;
                    btnAddComment.ClientVisible = false;
                    ASPxGridViewKVPComments.ClientVisible = false;
                }

                // lookup realizator show only from KVP group which is logedin
                //KVPSkupina_Users obj = kvpGroupsRepo.GetKVPGroupUserByUserID(PrincipalHelper.GetUserPrincipal().ID);
                //if (obj != null)
                //{
                //    XpoDSRealizator.Criteria = "([<KVPSkupina_Users>][^.Id = IdUser.Id AND idKVPSkupina.idKVPSkupina = " + obj.idKVPSkupina.idKVPSkupina + "] AND Id <> " + PrincipalHelper.GetUserPrincipal().ID + ") OR (RoleID.Koda = '" + Enums.UserRole.Leader.ToString() + "' AND Id <> " + PrincipalHelper.GetUserPrincipal().ID + ")";
                //}
            }
            else if (PrincipalHelper.IsUserChampion())
            {
                ASPxGridLookupLeaderTeam.ClientEnabled = false;

                //Set criteria for kvpgroup users on proposer lookup
                KVPSkupina_Users obj = kvpGroupsRepo.GetKVPGroupUserByUserID(PrincipalHelper.GetUserPrincipal().ID);
                if (obj != null && obj.Champion && (action == (int)Enums.UserAction.Add))
                {
                    XpoDSProposer.Criteria = "[<KVPSkupina_Users>][^.Id = IdUser.Id AND idKVPSkupina.idKVPSkupina = " + obj.idKVPSkupina.idKVPSkupina + "]";
                }

                // only if add new KVP or KVP status is VNOS
                if (action == (int)Enums.UserAction.Add)
                {
                    ASPxMemoNotesLeader.ClientVisible = false;
                    lblNoteLeader.ClientVisible = false;
                    btnAddComment.ClientVisible = false;
                    ASPxGridViewKVPComments.ClientVisible = false;
                }
            }
            else if (PrincipalHelper.IsUserTpmAdmin())
            {
                ASPxGridLookupLeaderTeam.ClientEnabled = false;
                ASPxGridLookupRealizator.ClientEnabled = false;
            }
            else if (PrincipalHelper.IsUserAdmin())
            {
                changeStatusItem.Style.Add("display", "block !important");
            }
            else if (PrincipalHelper.IsUserSuperAdmin())
            {
                changeStatusItem.Style.Add("display", "block !important");
            }

        }
        #endregion

        private bool AddOrEditEntityObject(bool add = false, Enums.KVPProcessStatus procStat = Enums.KVPProcessStatus.Nothing)
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
                if (model != null)
                {
                    model.idKVPDocument = model.idKVPDocument;
                }
                else
                {
                    ClearSessionsAndRedirect();
                }

            }

            // if user has transferred kvp to red card
            if (transferToRedCard)
            {
                if (model.idTipRdeciKarton != null)
                    model.idTipRdeciKarton = kvpDocRepo.GetRedCardTypeByID(1, model.idTipRdeciKarton.Session);//Manjse popravilo
                else
                    model.idTipRdeciKarton = kvpDocRepo.GetRedCardTypeByID(1, model.Session);//Manjse popravilo

                model.RokOdziva = DateTime.Now.AddDays(7);
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

            //K predlagatelju še shranimo njegovo KVP skupino
            if (add)
            {
                KVPSkupina_Users kvpGroupUser = kvpGroupsRepo.GetKVPGroupUserByUserID(model.Predlagatelj.Id, model.Session);
                if (kvpGroupUser != null)
                    model.KVPSkupinaID = kvpGroupUser.idKVPSkupina;
                else
                    throw new Exception("&%messageType=2&% There is no KVPGroup for User/Proposer: " + PrincipalHelper.GetUserPrincipal().firstName + " " + PrincipalHelper.GetUserPrincipal().lastName);
            }
            //if leader reject or confirm KVP
            if (procStat.Equals(Enums.KVPProcessStatus.Confirm))
            {
                SetRealizator();

                model.LastStatusId = model.LastStatusId != null ? statusRepo.GetStatusByCode(Enums.KVPStatuses.V_REALIZACIJI.ToString(), model.LastStatusId.Session) : statusRepo.GetStatusByCode(Enums.KVPStatuses.V_REALIZACIJI.ToString());

                //Shranimo nov status V_REALIZACIJI (oz. ne shranimo nič, če ta status že obstaja za ta KVP dokument)
                if (!kvpStatusRepo.HasKVPDocumentKVPStatus(kvpDocID, Enums.KVPStatuses.V_REALIZACIJI.ToString()))
                    kvpDocRepo.ChangeStatusOnKVPDocument(kvpDocID, Enums.KVPStatuses.V_REALIZACIJI, false);
            }
            else if (procStat.Equals(Enums.KVPProcessStatus.Reject))
            {
                model.LastStatusId = model.LastStatusId != null ? statusRepo.GetStatusByCode(Enums.KVPStatuses.ZAVRNJEN.ToString(), model.LastStatusId.Session) : statusRepo.GetStatusByCode(Enums.KVPStatuses.ZAVRNJEN.ToString());

                //Shranimo nov status ZAVRNJEN (oz. ne shranimo nič, če ta status že obstaja za ta KVP dokument)
                if (!kvpStatusRepo.HasKVPDocumentKVPStatus(kvpDocID, Enums.KVPStatuses.ZAVRNJEN.ToString()) && !GetHiddenfieldSaveRejectOpenNewKVPValue())
                    kvpDocRepo.ChangeStatusOnKVPDocument(kvpDocID, Enums.KVPStatuses.ZAVRNJEN, false);

                model.ZavrnitevOpis = ASPxMemoRejectKVPArguments.Text;
                model.ZavrnitevIdUser = model.ZavrnitevIdUser != null ? employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, model.ZavrnitevIdUser.Session) : employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, model.Session);
                model.ObvescenaOsebaZavrnitve = model.ObvescenaOsebaZavrnitve != null ? employeeRepo.GetEmployeeByID(CommonMethods.ParseInt(ASPxGridLookupInformedEmployee.Value)) : employeeRepo.GetEmployeeByID(CommonMethods.ParseInt(ASPxGridLookupInformedEmployee.Value), model.Session);
                model.DatumZakljuceneIdeje = DateTime.Now;
            }
            else// da se nam nastavljanje realizatorja ne bo dvakrat shranjevalo
            {
                //Champion lahko določi realizatorja
                if (PrincipalHelper.IsUserChampion() || PrincipalHelper.IsUserLeader() || PrincipalHelper.IsUserAdmin())
                    SetRealizator();
            }


            if (realizeKVPProposal)
            {
                kvpDocRepo.ChangeStatusOnKVPDocument(kvpDocID, Enums.KVPStatuses.REALIZIRANO);
                model.LastStatusId = model.LastStatusId != null ? statusRepo.GetStatusByCode(Enums.KVPStatuses.REALIZIRANO.ToString(), model.LastStatusId.Session) : statusRepo.GetStatusByCode(Enums.KVPStatuses.REALIZIRANO.ToString());
                model.DatumRealizacije = DateEditDatumRealizacije.Date;
            }

            //if realizator changes relizator
            if ((model.LastStatusId != null && model.LastStatusId.Koda == Enums.KVPStatuses.V_REALIZACIJI.ToString() && model.Realizator.Id == PrincipalHelper.GetUserPrincipal().ID) &&
                model.Realizator.Id != CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealizator)) && procStat.Equals(Enums.KVPProcessStatus.Nothing))
            {
                SetRealizator();
            }

            if (completedKVP)
            {
                model.LastStatusId = model.LastStatusId != null ? statusRepo.GetStatusByCode(Enums.KVPStatuses.ZAKLJUCENO.ToString(), model.LastStatusId.Session) : statusRepo.GetStatusByCode(Enums.KVPStatuses.ZAKLJUCENO.ToString());
                model.DatumZakljuceneIdeje = DateTime.Now;
                CompleteKVPDocument();
            }


            model.vodja_teama = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupLeaderTeam));
            model.OpisProblem = ASPxMemoProblemDesc.Text;
            model.PredlogIzboljsave = ASPxMemoImprovementProposition.Text;
            //model.PrihranekStroski = ASPxMemoSavingsOrCosts.Text;
            model.PrihranekStroskiDA_NE = CheckBoxPrihranekStroski.Checked;
            //model.OpombeVodja = ASPxMemoNotesLeader.Text;

            model.CIPOpombe = ASPxMemoCIPOpombe.Text;

            model.OpisLokacija = txtOpisLokacija.Text;
            model.Linija = txtLinija.Text;
            model.Stroj = txtStroj.Text;
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


            Enums.SubmitProposalType submitProposalType = Enums.SubmitProposalType.OnlySaveProposal;

            if (submitProposalToLeader)
            {
                submitProposalType = Enums.SubmitProposalType.SubmitProposalToLeader;
            }
            else if (submitProposalToChampion)
                submitProposalType = Enums.SubmitProposalType.SubmitProposalToChampion;
            else if (submitProposalAndRejectNewKVP)
                submitProposalType = Enums.SubmitProposalType.SubmitProposalAndReject;

            kvpDocID = model.idKVPDocument = kvpDocRepo.SaveKVP(model, submitProposalType);
            GetKVPDocumentProvider().SetKVPDocumentModel(model);

            return true;
        }

        protected void CallbackPanelKVPDocumentForm_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "SaveStatus")
            {
                KVP_Status status = new KVP_Status(session);
                status.idKVP_Status = 0;
                status.idKVPDocument = kvpDocRepo.GetKVPByID(model.idKVPDocument, session);

                int statusID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupStatus));
                status.idStatus = statusRepo.GetStatusByID(statusID);
                status.Opomba = ASPxMemoOpombe.Text;

                kvpStatusRepo.SaveKVPStatus(status);
                kvpDocRepo.SaveLastStatusOnKVP(model, status.idStatus, model.Session);
                //Champion lahko določi realizatorja
                if (PrincipalHelper.IsUserAdmin())
                {
                    SetRealizator();
                    kvpDocRepo.SaveKVP(model);
                }

                ClearSessionsAndRedirect();
            }
            else if (e.Parameter == "GetHistoryStatuses")
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
            else if (e.Parameter == "SubmitProposalToLeader")
            {
                submitProposalToLeader = true;
                btnConfirm_Click(this, e);

                /*NotificationWindow.SetTitle = "Ali želite oddati KVP predlog vodji?";
                NotificationWindow.SetDescription = "S potrditvenim gumbom OK boste poslali KVP predlog vodji na ocenitev.";
                NotificationWindow.ShowPopUp();*/

            }
            else if (e.Parameter == "ShowAuditorPopUp")
            {
                //če je pre dodajanjem presojevalca še vpisal opombo vodje, je potrebno to opombo shranit.
                /*if (!String.IsNullOrEmpty(ASPxMemoNotesLeader.Text))
                {
                    model.OpombeVodja = ASPxMemoNotesLeader.Text;
                    kvpDocRepo.SaveKVP(model);
                }*/

                ASPxPopupControlAuditors.ShowOnPageLoad = true;
                AddValueToSession(Enums.KVPDocumentSession.KVPDocumentID, kvpDocID);
                AddValueToSession(Enums.Employee.EmployeeID, ASPxGridLookupProposer.Value);
            }
            else if (e.Parameter == "ShowRealizationPopUp")
            {
                PopUpRealizationConfirmation.SetTitle = "Ali želite realizirati KVP predlog?";
                PopUpRealizationConfirmation.SetDescription = "S klikom na gumb OK boste realizirali KVP predlog";
                PopUpRealizationConfirmation.ShowPopUp();
            }
            else if (e.Parameter == "RejectArguments")
            {
                CallbackPanelKVPDocumentForm.JSProperties["cpRejectArguments"] = "<h3>Argumenti zavrnitve</h3><p>Zavrnil: " +
                    (model.ZavrnitevIdUser != null ? model.ZavrnitevIdUser.Firstname + " " + model.ZavrnitevIdUser.Lastname : "") + "</p>" +
                    "<p>" + model.ZavrnitevOpis + "</p>";
            }
            else if (e.Parameter == "SubmitProposalToChampion")
            {
                submitProposalToChampion = true;
                btnConfirm_Click(this, e);
            }
            else if (e.Parameter == "ProposalTheSameAsLeader")
            {
                int departmentHeadID = employeeRepo.GetDepartmentHeadID();

                ASPxGridLookupLeaderTeam.Value = departmentHeadID;
            }
            else if (e.Parameter == "InfoMailPopup")
            {
                AddValueToSession(Enums.KVPDocumentSession.KVPDocumentID, model != null ? model.StevilkaKVP : "");
                ASPxPopupControlSendInfoMail.ShowOnPageLoad = true;
            }

            //preverjamo če uporabnik želi takoj ob dodajanju novega kvp dokumenta oddati predlog. v tem primeru je potrebno to preračunavanje preskočiti, kajti še nimamo kvp dokumenta shranjenega
            if ((action != (int)Enums.UserAction.Add) && (model != null))
            {
                HtmlGenericControl control = (HtmlGenericControl)historyStatusItem.FindControl("historyStatusBadge");
                control.InnerText = kvpDocRepo.GetKVPByID(model.idKVPDocument, session).KVP_Statuss.Count.ToString();
            }
        }

        #region Attachments
        protected void test_PopulateAttachments(object sender, EventArgs e)
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

        protected void test_UploadComplete(object sender, EventArgs e)
        {
            model = GetKVPDocumentProvider().GetKVPDocumentModel();
            if (model != null)
            {
                string pipe = "";
                if (!String.IsNullOrEmpty(model.Priloge))
                    pipe = "|";

                kvpDocRepo = new KVPDocumentRepository(model.Session);
                model.Priloge += pipe + (sender as UploadAttachment).currentFile.Url + ";" + (sender as UploadAttachment).currentFile.Name;
                kvpDocRepo.SaveKVP(model);
                GetKVPDocumentProvider().SetKVPDocumentModel(model);
                HtmlGenericControl control = (HtmlGenericControl)attachmentsItem.FindControl("attachmentsBadge");
                control.InnerText = model.Priloge.Split('|').Length.ToString();
            }
        }

        protected void test_DeleteAttachments(object sender, EventArgs e)
        {
            model = GetKVPDocumentProvider().GetKVPDocumentModel();
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

                    model.Priloge = model.Priloge.Remove(model.Priloge.IndexOf(item) - hasPipe, item.Length + hasPipe);
                    kvpDocRepo = new KVPDocumentRepository(model.Session);
                    kvpDocRepo.SaveKVP(model);
                }

            }
        }

        protected void test_DownloadAttachments(object sender, EventArgs e)
        {
            model = GetKVPDocumentProvider().GetKVPDocumentModel();
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
            model = GetKVPDocumentProvider().GetKVPDocumentModel();
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

        #region Helper Methods
        private void KVPProccessButtonsVisibility(int auditorsCount = 0)
        {
            bool visible = false;
            bool confirmKVP = false;
            bool rejectKVP = false;
            int kVPAuditorID = 0;
            bool realizeKVP = false;

            if (auditorsCount > 0) kVPAuditorID = auditorsRepo.GetLatestAuditorOnKVP(kvpDocID).Id;


            if ((PrincipalHelper.IsUserLeader() || PrincipalHelper.IsUserAdmin() || PrincipalHelper.IsUserSuperAdmin()) && (model.LastStatusId != null && (model.LastStatusId.Koda == Enums.KVPStatuses.ODOBRITEV_VODJA.ToString())))
            {
                visible = true;
                confirmKVP = true;
                rejectKVP = true;
            }
            else if ((model.LastStatusId != null && model.LastStatusId.Koda == Enums.KVPStatuses.POSLANO_V_PRESOJO.ToString()) && kVPAuditorID == PrincipalHelper.GetUserPrincipal().ID)
            {
                visible = true;
                confirmKVP = true;
                rejectKVP = true;
                ASPxGridLookupRealizator.ClientEnabled = true;
                lblNoteLeader.ClientVisible = true;
                ASPxMemoNotesLeader.ClientVisible = true;
                btnAddComment.ClientVisible = true;
                ASPxGridViewKVPComments.ClientVisible = true;
                CheckBoxPrihranekStroski.ClientVisible = true;
            }

            if (auditorsCount >= 3)
                visible = false;

            if (model.LastStatusId.Koda == Enums.KVPStatuses.REALIZIRANO.ToString())
            {
                btnConfirm.ClientEnabled = false;
                ASPxMemoNotesLeader.ClientEnabled = false;
                btnAddComment.ClientVisible = false;
                CheckBoxPrihranekStroski.ClientEnabled = false;
                ASPxGridLookupRealizator.ClientEnabled = false;
            }

            if (model.LastStatusId.Koda == Enums.KVPStatuses.V_REALIZACIJI.ToString())
            {
                ASPxGridLookupRealizator.ClientEnabled = false;
            }

            if (model.LastStatusId.Koda == Enums.KVPStatuses.V_REALIZACIJI.ToString() && PrincipalHelper.IsUserLeader())
            {
                ASPxGridLookupRealizator.ClientEnabled = true;
            }

            if ((kVPAuditorID > 0 && kVPAuditorID == PrincipalHelper.GetUserPrincipal().ID && (model.LastStatusId.Koda == Enums.KVPStatuses.POSLANO_V_PRESOJO.ToString())))
            {
                confirmKVP = true;
                rejectKVP = true;
            }

            //prikaz gumba za realizaicjo KVP obrazca

            if (model.Realizator != null)
            {
                if ((model.LastStatusId.Koda == Enums.KVPStatuses.V_REALIZACIJI.ToString() && model.Realizator.Id == PrincipalHelper.GetUserPrincipal().ID) || (model.LastStatusId.Koda == Enums.KVPStatuses.V_REALIZACIJI.ToString() && PrincipalHelper.IsUserChampion()))
                {
                    realizeKVP = true;
                }
            }

            //gumb aktiven samo ko je kvp v delovni verziji oz. čaka na odobritev vodje
            if (model.LastStatusId.Koda == Enums.KVPStatuses.VNOS.ToString() || model.LastStatusId.Koda == Enums.KVPStatuses.ODOBRITEV_VODJA.ToString())
                btnPrenosRdeciKarton.ClientVisible = true;

            if (model.LastStatusId.Koda == Enums.KVPStatuses.ZAVRNJEN.ToString())
            {
                KVPBasicPanel.Style.Add("border-color", "Tomato");
                KVPBasicPanelHeading.Style.Add("background-color", "Tomato");
                btnRejectArgumentsNotification.ClientVisible = true;
                rejectKVP = false;
            }
            else if (model.LastStatusId.Koda == Enums.KVPStatuses.REALIZIRANO.ToString())
            {
                KVPBasicPanel.Style.Add("border-color", "#FF9A33");
                KVPBasicPanelHeading.Style.Add("background-color", "#FF9A33");

                //only champion can complete KVP
                if (PrincipalHelper.IsUserChampion())
                    btnCompleteKVP.ClientVisible = true;
            }
            else if (model.LastStatusId.Koda == Enums.KVPStatuses.ZAKLJUCENO.ToString())
            {
                KVPBasicPanel.Style.Add("border-color", "LightGreen");
                KVPBasicPanelHeading.Style.Add("background-color", "LightGreen");
            }

            if (model.LastStatusId != null && model.LastStatusId.Koda == Enums.KVPStatuses.VNOS.ToString())
            {
                if (PrincipalHelper.IsUserChampion())
                {
                    btnSubmitProposalToLeader.ClientVisible = true;
                    btnSubmitProposalAndNewKVP.ClientVisible = true;
                    btnSaveAndReject.ClientVisible = true;
                }
                else
                    btnSubmitProposalToChampion.ClientVisible = true;
            }
            else if (model.LastStatusId != null && model.LastStatusId.Koda == Enums.KVPStatuses.V_PREVERJANJE.ToString())
            {
                if (PrincipalHelper.IsUserChampion())
                {
                    btnSubmitProposalToLeader.ClientVisible = true;
                    btnSubmitProposalAndNewKVP.ClientVisible = true;
                    btnSaveAndReject.ClientVisible = true;
                    confirmKVP = false;
                    rejectKVP = true;
                }


            }

            if (model != null)
            {
                if (PrincipalHelper.IsUserLeader() && model.LastStatusId.Koda == Enums.KVPStatuses.ODOBRITEV_VODJA.ToString() && model.Predlagatelj.Id == PrincipalHelper.GetUserPrincipal().ID)
                {
                    visible = false;
                    confirmKVP = false;
                    rejectKVP = false;
                    ASPxGridLookupRealizator.ClientEnabled = false;
                }
            }
            btnConfrimKVP.ClientVisible = confirmKVP;
            btnRejectKVP.ClientVisible = rejectKVP;
            btnAddAuditor.ClientVisible = visible;
            btnRealize.ClientVisible = realizeKVP;
            DateEditDatumRealizacije.ClientVisible = realizeKVP;
            lblDatumRealizacija.ClientVisible = realizeKVP;
        }

        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool newKVPDoc = false, int? nextKVPdocId = null)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = kvpDocID.ToString() }
            };


            if (submitProposalToLeader || submitProposalToChampion)
            {
                queryStrings.Add(new QueryStrings() { Attribute = Enums.QueryStringName.successMessage.ToString(), Value = "true" });
            }

            if (SessionHasValue(Enums.CommonSession.activeTab) && !newKVPDoc)
            {
                queryStrings.Add(new QueryStrings() { Attribute = Enums.QueryStringName.activeTab.ToString(), Value = GetValueFromSession(Enums.CommonSession.activeTab).ToString() });
                RemoveSession(Enums.CommonSession.activeTab);
            }


            if (isIDDeleted)
                redirectString = "../Dashboard.aspx";
            else
                redirectString = GenerateURI("../Dashboard.aspx", queryStrings);

            if (newKVPDoc)
            {
                queryStrings.Add(new QueryStrings() { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Add).ToString() });
                queryStrings.Add(new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = "0" });
                redirectString = GenerateURI("KVPDocumentForm.aspx", queryStrings);
            }

            if (nextKVPdocId.HasValue)
                redirectString = GenerateURI("KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, nextKVPdocId.Value);

            List<Enums.KVPDocumentSession> list = Enum.GetValues(typeof(Enums.KVPDocumentSession)).Cast<Enums.KVPDocumentSession>().ToList();
            ClearAllSessions(list, redirectString, CommonMethods.IsCallbackRequest(this.Request));
        }

        private void CompleteKVPDocument()
        {
            string month = CommonMethods.GetDateTimeMonthByNumber(model.DatumRealizacije.Date.Month);

            //Pridobimo izplačila za mesec in leto realizacije
            Izplacila userProposerPayoutRecord = payoutRepo.UserPayoutRecordForMonthAndYear(model.Predlagatelj.Id, month, model.DatumRealizacije.Date.Year);
            Izplacila userRealizatorPayoutRecord = payoutRepo.UserPayoutRecordForMonthAndYear(model.Realizator.Id, month, model.DatumRealizacije.Date.Year);
            //će obstaja izplačilo posodobimo njegove vrednosti
            if (model.Predlagatelj.UpravicenDoKVP)
            {
                if (userProposerPayoutRecord != null)
                {
                    decimal vsotaTProposer = 0;
                    userProposerPayoutRecord.PredlagateljT += model.idTip.TockePredlagatelj;
                    vsotaTProposer = userProposerPayoutRecord.PredlagateljT + userProposerPayoutRecord.RealizatorT + userProposerPayoutRecord.PrenosIzPrejsnjegaMeseca;
                    userProposerPayoutRecord.VsotaT = vsotaTProposer;
                    //userProposerPayoutRecord.PrenosTvNaslednjiMesec += vsotaTProposer;

                    payoutRepo.SavePayout(userProposerPayoutRecord);
                }
                else//če ne obstaja ustvarimo novo izplačilo - vrednosti prenesemo iz prejšnjega meseca in jih prištejemo novim
                {
                    CreateNewPayout();
                }
            }

            //isto kot za predlagatelja (zgoraj) velja tudi za realizatorja
            if (model.Realizator.UpravicenDoKVP)
            {
                if (userRealizatorPayoutRecord != null)
                {
                    decimal vsotaTRealizator = 0;
                    userRealizatorPayoutRecord.RealizatorT += model.idTip.TockeRealizator;
                    vsotaTRealizator = userRealizatorPayoutRecord.PredlagateljT + userRealizatorPayoutRecord.RealizatorT + userRealizatorPayoutRecord.PrenosIzPrejsnjegaMeseca;
                    userRealizatorPayoutRecord.VsotaT = vsotaTRealizator;
                    //userRealizatorPayoutRecord.PrenosTvNaslednjiMesec += vsotaTRealizator;

                    payoutRepo.SavePayout(userRealizatorPayoutRecord);
                }
                else
                {
                    CreateNewPayout(true);
                }
            }
            kvpDocRepo.ChangeStatusOnKVPDocument(kvpDocID, Enums.KVPStatuses.ZAKLJUCENO, false);
        }

        private void CreateNewPayout(bool isRealizator = false)
        {
            if (model.DatumRealizacije < new DateTime(2000, 1, 1)) model.DatumRealizacije = DateTime.Now;

            DateTime previousMonthDate = model.DatumRealizacije.Date.AddMonths(-1);
            string previousMonth = CommonMethods.GetDateTimeMonthByNumber(previousMonthDate.Date.Month);
            Izplacila payoutPreviousMonth = payoutRepo.UserPayoutRecordForMonthAndYear((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id), previousMonth, previousMonthDate.Year);


            Izplacila payout = new Izplacila(session);
            payout.DatumOd = CommonMethods.GetFirstDayOfMonth(model.DatumRealizacije);
            payout.DatumDo = CommonMethods.GetLastDayOfMonth(model.DatumRealizacije);
            //payout.DatumIzracuna
            payout.IdUser = employeeRepo.GetEmployeeByID((!isRealizator ? model.Predlagatelj.Id : model.Realizator.Id));
            payout.IzplaciloVMesecu = 0;
            payout.Leto = model.DatumRealizacije.Date.Year;
            payout.Mesec = CommonMethods.GetDateTimeMonthByNumber(model.DatumRealizacije.Date.Month);
            payout.PredlagateljT = !isRealizator ? model.idTip.TockePredlagatelj : 0;
            payout.PrenosIzPrejsnjegaMeseca = payoutPreviousMonth != null ? payoutPreviousMonth.PrenosTvNaslednjiMesec : 0;
            payout.PrenosTvNaslednjiMesec = 0;
            payout.RealizatorT = !isRealizator ? 0 : model.idTip.TockeRealizator;
            payout.VsotaT = (payout.PrenosIzPrejsnjegaMeseca + payout.PredlagateljT + payout.RealizatorT);

            payoutRepo.SavePayout(payout);

        }

        private void SetRealizator()
        {
            int realizatorID = CommonMethods.ParseInt(GetGridLookupValue(ASPxGridLookupRealizator));
            if (model.Realizator != null)
                model.Realizator = kvpDocRepo.GetEmployeeByID(realizatorID, model.Realizator.Session);
            else
                model.Realizator = kvpDocRepo.GetEmployeeByID(realizatorID, model.Session);
        }

        private void HasPreviuosAndNextKVPs()
        {
            if (GetKVPDocumentProvider().GetKVPDocumentsIDsList() != null)
            {
                int nextKvpDocID = GetKVPDocumentProvider().GetKVPDocumentsIDsList().SkipWhile(i => i != kvpDocID).Skip(1).FirstOrDefault();
                int previousKvpdocID = GetKVPDocumentProvider().GetKVPDocumentsIDsList().TakeWhile(i => i != kvpDocID).LastOrDefault();

                if (nextKvpDocID <= 0)
                    btnNextKVP.ClientEnabled = false;
                else
                    btnNextKVP.ClientEnabled = true;

                if (previousKvpdocID <= 0)
                    btnPreviousKVP.ClientEnabled = false;
                else
                    btnPreviousKVP.ClientEnabled = true;
            }
            else
            {
                btnNextKVP.ClientVisible = false;
                btnPreviousKVP.ClientVisible = false;
            }
        }

        #endregion

        #region Events

        #region ButtonEvents

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
                ClearSessionsAndRedirect(isDeleteing, submitOpenNewKVP);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveSession(Enums.CommonSession.KVPDocumentIDsToShow);
            RemoveSession(Enums.CommonSession.KVPDocumentNextPreviousID);
            ClearSessionsAndRedirect();
        }

        protected void btnPrenosRdeciKarton_Click(object sender, EventArgs e)
        {
            transferToRedCard = true;
            btnConfirm_Click(null, EventArgs.Empty);
        }
        protected void btnConfrimKVP_Click(object sender, EventArgs e)
        {
            bool userActionAdd = (action == (int)Enums.UserAction.Add);
            AddOrEditEntityObject(userActionAdd, Enums.KVPProcessStatus.Confirm);
            //kvpDocRepo.ChangeStatusOnKVPDocument(kvpDocID, Enums.KVPStatuses.ODOBRENO_VODJA);
            ClearSessionsAndRedirect();
        }

        //When KVP is rejected
        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            bool userActionAdd = (action == (int)Enums.UserAction.Add);
            ASPxPopupControlRejectKVPArguments.ShowOnPageLoad = false;


            //Če je bil pritisnjen gumb shrani in zavrni KVP
            bool submitRejectAndAddNewKVP = GetHiddenfieldSaveRejectOpenNewKVPValue();

            submitProposalAndRejectNewKVP = submitRejectAndAddNewKVP;
            submitOpenNewKVP = submitRejectAndAddNewKVP;

            AddOrEditEntityObject(userActionAdd, Enums.KVPProcessStatus.Reject);
            messageRepo.ProcessRejectedKVPToSend(CommonMethods.ParseInt(ASPxGridLookupInformedEmployee.Value), model.idKVPDocument, model.Session);

            //kvpDocRepo.ChangeStatusOnKVPDocument(kvpDocID, Enums.KVPStatuses.ZAVRNJEN);
            ClearSessionsAndRedirect();
        }

        protected void btnSubmitProposalAndNewKVP_Click(object sender, EventArgs e)
        {
            submitOpenNewKVP = true;
            submitProposalToLeader = true;
            btnConfirm_Click(this, e);
        }

        protected void btnSaveAndReject_Click(object sender, EventArgs e)
        {
            submitProposalAndRejectNewKVP = true;
            submitOpenNewKVP = true;

            ASPxPopupControlRejectKVPArguments.ShowOnPageLoad = false;

            bool userActionAdd = (action == (int)Enums.UserAction.Add);
            AddOrEditEntityObject(userActionAdd, Enums.KVPProcessStatus.Reject);
            messageRepo.ProcessRejectedKVPToSend(CommonMethods.ParseInt(ASPxGridLookupInformedEmployee.Value), model.idKVPDocument, model.Session);

            //kvpDocRepo.ChangeStatusOnKVPDocument(kvpDocID, Enums.KVPStatuses.ZAVRNJEN);
            ClearSessionsAndRedirect(false, submitOpenNewKVP);
        }

        protected void btnNextKVP_Click(object sender, EventArgs e)
        {
            int? nextKvpDocID = GetKVPDocumentProvider().GetKVPDocumentsIDsList().SkipWhile(i => i != kvpDocID).Skip(1).FirstOrDefault();
            ClearSessionsAndRedirect(false, false, nextKvpDocID);
        }

        protected void btnPreviousKVP_Click(object sender, EventArgs e)
        {

            int? previousKvpdocID = GetKVPDocumentProvider().GetKVPDocumentsIDsList().TakeWhile(i => i != kvpDocID).LastOrDefault();
            ClearSessionsAndRedirect(false, false, previousKvpdocID);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (model.LastStatusId.Koda != Enums.KVPStatuses.VNOS.ToString()) return;

            kvpDocRepo.DeleteKVP(model);
            ClearSessionsAndRedirect(true);
        }

        protected void btnCompleteKVP_Click(object sender, EventArgs e)
        {
            completedKVP = true;
            btnConfirm_Click(this, EventArgs.Empty);
        }
        #endregion

        #region Popup's events
        protected void NotificationWindow_ConfirmBtnClick(object sender, EventArgs e)
        {
            submitProposalToLeader = true;
            btnConfirm_Click(this, e);
        }

        protected void NotificationWindow_CancelBtnClick(object sender, EventArgs e)
        {

        }

        protected void ASPxPopupControlAuditors_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.KVPDocumentSession.KVPDocumentID);
            RemoveSession(Enums.Employee.EmployeeID);
        }

        protected void PopUpRealizationConfirmation_ConfirmBtnClick(object sender, EventArgs e)
        {
            realizeKVPProposal = true;
            btnConfirm_Click(this, e);
        }

        protected void PopUpRealizationConfirmation_CancelBtnClick(object sender, EventArgs e)
        {

        }
        #endregion

        #endregion

        protected void ASPxPopupControlRejectKVPArguments_WindowCallback(object source, PopupWindowCallbackArgs e)
        {

        }

        protected void ASPxGridViewKVPComments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "CommentAdd")
            {
                bool redirect = false;
                KVPKomentarji comment = new KVPKomentarji(session);

                if (action == (int)Enums.UserAction.Add)
                {
                    AddOrEditEntityObject(true);
                    redirect = true;
                }

                comment.Koda = Enums.KVPCommentCode.LeaderNotes.ToString();
                comment.KVPDocId = kvpDocRepo.GetKVPByID(kvpDocID, session);
                comment.KVPKomentarjiID = 0;
                comment.Opombe = ASPxMemoNotesLeader.Text;
                comment.UserId = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, session);
                kvpDocRepo.SaveKVPComment(comment);
                ASPxGridViewKVPComments.DataBind();

                if (redirect)
                    ASPxWebControl.RedirectOnCallback(GenerateURI("KVPDocumentForm.aspx", (int)Enums.UserAction.Edit, kvpDocID));
            }
        }

        protected void ASPxGridViewKVPComments_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            int employeeId = CommonMethods.ParseInt(ASPxGridViewKVPComments.GetRowValues(e.VisibleIndex, "UserId.Id"));
            GridViewTableCommandCell control = (GridViewTableCommandCell)e.Row.Cells[1];

            if (employeeId != PrincipalHelper.GetUserPrincipal().ID || model.LastStatusId.Koda == Enums.KVPStatuses.ZAKLJUCENO.ToString())
                control.Enabled = false;

        }

        protected void ASPxGridViewKVPComments_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            int commentID = CommonMethods.ParseInt(ASPxGridViewKVPComments.GetRowValues(e.VisibleIndex, "KVPKomentarjiID"));
            kvpDocRepo.DeleteKVPComment(commentID);
            ASPxGridViewKVPComments.DataBind();
        }

        private bool IsActiveTabChampionAllKVPs()
        {
            bool isActive = false;
            if (SessionHasValue(Enums.CommonSession.activeTab))
            {
                string activeTabName = GetStringValueFromSession(Enums.CommonSession.activeTab);

                if (activeTabName == "#ChampionAllKVP")
                    isActive = true;
            }
            return isActive;
        }

        private void SetEnableAllButtons(bool enable)
        {
            btnAddComment.ClientVisible = enable;
            btnDelete.ClientVisible = enable;
            btnConfirmStatus.ClientVisible = enable;
            btnConfirmPopUp.ClientVisible = enable;
            btnPrenosRdeciKarton.ClientVisible = enable;
            btnRejectKVP.ClientVisible = enable;
            btnConfrimKVP.ClientVisible = enable;
            btnSubmitProposalToLeader.ClientVisible = enable;
            btnSubmitProposalToChampion.ClientVisible = enable;
            btnSubmitProposalAndNewKVP.ClientVisible = enable;
            btnSaveAndReject.ClientVisible = enable;
            btnAddAuditor.ClientVisible = enable;
            btnRealize.ClientVisible = enable;
            btnCompleteKVP.ClientVisible = enable;
            btnConfirm.ClientVisible = enable;
        }

        private bool GetHiddenfieldSaveRejectOpenNewKVPValue()
        {
            if (hfSaveAndReject.Contains("SubmitKVPRejectAndOpenNew"))
                return (bool)hfSaveAndReject["SubmitKVPRejectAndOpenNew"];
            return false;
        }

        protected void ASPxPopupControlSendInfoMail_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.KVPDocumentSession.KVPDocumentID);
        }

        private void ShowRequiredFields()
        {
            Enums.UserRole userRole = (Enums.UserRole)Enum.Parse(typeof(Enums.UserRole), PrincipalHelper.GetUserPrincipal().Role, true);

            if (action == (int)Enums.UserAction.Add)
            {
                switch (userRole)
                {
                    case Enums.UserRole.Employee:
                        ASPxMemoProblemDesc.CssClass = "focus-text-box-input-error";
                        ASPxMemoImprovementProposition.CssClass = "focus-text-box-input-error";
                        break;
                    case Enums.UserRole.Leader:
                        ASPxMemoProblemDesc.CssClass = "focus-text-box-input-error";
                        ASPxMemoImprovementProposition.CssClass = "focus-text-box-input-error";
                        break;
                    default:
                        ASPxMemoProblemDesc.CssClass = "focus-text-box-input-error";
                        ASPxMemoImprovementProposition.CssClass = "focus-text-box-input-error";
                        ASPxGridLookupProposer.CssClass = "focus-text-box-input-error";
                        //ASPxGridLookupRealizator.CssClass = "focus-text-box-input-error";
                        break;
                }
            }
        }
    }
}