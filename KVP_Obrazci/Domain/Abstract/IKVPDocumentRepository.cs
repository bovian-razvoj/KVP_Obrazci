using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IKVPDocumentRepository
    {
        KVPDocument GetKVPByID(int kvpID, Session currentSession = null);
        /*int SaveKVP(KVPDocument model, bool submitProposalDirect = false);*/
        void DeleteKVP(int id);
        void DeleteKVP(KVPDocument model);

        Users GetEmployeeByID(int employeeID, Session currentSession = null);
        Tip GetTypeByID(int typeID, Session currentSession = null);
        TipRdeciKarton GetRedCardTypeByID(int typeID, Session currentSession = null);

        int GetKVPCountByUserID(int userID, Session currentSession = null);
        int GetKVPRealizedCountByUserID(int userID, Session currentSession = null);
        int GetKVPRedCardsCountByUserID(int userID, Session currentSession = null);
        int GetNotFinishedKVPsForUser(int userID, Session currentSession = null);
        TipRdeciKarton GetRedCardTypeByCode(string recCardTypeCode, Session currentSession = null);
        void ChangeStatusOnKVPDocument(int kvpID, Enums.KVPStatuses status, bool changeStatusOnKVP = false);

        void ChangeStatusOnKVPDocument(KVPDocument model, Enums.KVPStatuses status, bool changeStatusOnKVP = false, Session currentSession = null);
        Tip GetTypeByCode(string code, Session currentSession = null);
        void AutomaticRealizationKVPDocument(List<object> selectedKVPs);
        decimal GetRealizedKVPsForUser(int userID);
        int SaveKVP(KVPDocument model, Enums.SubmitProposalType kvpProposalTypeSubmit = Enums.SubmitProposalType.OnlySaveProposal, bool transferToRedCard = false);
        decimal GetNumberOfElapsedDayFromSubmitingKVP(KVPDocument model);
        void AutomaticUpdateStatusKVPDocument(List<object> selectedKVPs, Enums.KVPStatuses kStatus = Enums.KVPStatuses.REALIZIRANO);

        KVPKomentarji GetKVPCommentByID(int commentID, Session currentSession = null);
        void SaveKVPComment(KVPKomentarji model);
        void DeleteKVPComment(KVPKomentarji comment);
        void DeleteKVPComment(int commentID);

        int GetNewKVPNumber();

        void SaveLastStatusOnKVP(KVPDocument model, Status status, Session currentSession = null);

        int GetSubmittedKVPCountByUserIdAndYear(int userId, int year);
        int GetSubmittedKVPCountByUserId(int userId);
        decimal GetCompletedKVPsForUser(int userID);

        void ChangeLeaderOnKVPDocument(int CurrentLeaderID, int ChangedLeaderID);
        KVPInfoUserModel GetKVPDocForMonthByGroupID(int idKVPGroup, Session currentSession = null, bool onlyCompletedKVP = false);

        //Red cards
        int SaveRC(KVPDocument model, Enums.SubmitRedCardType redCardProposalTypeSubmit = Enums.SubmitRedCardType.OnlySaveProposal, bool isNumberingTypeSystem = true);
        void ChangeStatusOnRedCardDocument(KVPDocument model, Enums.RedCardStatus status, bool changeStatusOnRedCard = false, Session currentSession = null);
        void ChangeStatusOnRedCardDocument(int kvpID, Enums.RedCardStatus status, bool changeStatusOnRedCard = false);
        decimal GetNumberOfElapsedDayFromSubmitingRC(KVPDocument model);
        bool ExistManualRCNumber(string rcNumber);
        int GetRCCountByUserID(int userID, Session currentSession = null);
        decimal GetCompletedRCsForUser(int userID);
        void AutomaticOverDueRKChangeStatus();

        // User sync
        int SyncPayment(int CurrentUserID, int ChangedUserID);
        int ChangePresojaOnKVPDocument(int CurrentPresojaID, int ChangedPresojaID);
        int ChangeRealizatorOnKVPDocument(int CurrentRealizatorID, int ChangedRealizatorID);
        int ChangePredlagateljOnKVPDocument(int CurrentPredlagateljID, int ChangedPredlagateljID);
        int ChangeKVPSkupinaAndDepartmentSync(int SourceUserID, int DestinationUserID);
    }
}
