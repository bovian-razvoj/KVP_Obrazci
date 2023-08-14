using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Common
{
    public class Enums
    {
        public enum UserAction : int
        {
            Add = 1,
            Edit = 2,
            Delete = 3
        }

        public enum UserRole
        {
            SuperAdmin,
            Admin,
            Leader,
            Champion,
            Employee,
            TpmAdmin
        }

        public enum CommonSession
        {
            ShowWarning,
            ShowWarningMessage,
            UserActionPopUp,
            activeTab,
            PrintModel,
            PreviousPageName,
            PreviousPageSessions,
            DownloadDocument,
            KVPDocumentIDsToShow,
            KVPDocumentNextPreviousID
        }

        public enum QueryStringName
        {
            action,
            recordId,
            printReport,
            printId,
            showPreviewReport,
            successMessage,
            activeTab
        }

        public enum KVPDocumentSession
        {
            KVPDocumentModel,
            KVPDocumentID
        }

        public enum KVPGroups
        {
            KVPGroupModel,
            SelectedEmplyeesForGroup,
            KVPGroupID,
            KVPGroupUsersCriteriaPopUp
        }

        public enum Employee
        {
            EmployeeModel,
            EmployeeID
        }

        public enum RedCardType
        {
            MANJSE,
            VECJE,
            VARNOST
        }

        public enum KVPStatuses
        {
            VNOS,
            ODOBRITEV_VODJA,
            ODOBRENO_VODJA,
            V_REALIZACIJI,
            REALIZIRANO,
            IZRACUN_PRIHRANKA,
            IZPLACANO,
            POSLANO_V_PRESOJO,
            ZAVRNJEN,
            V_PREVERJANJE,
            ZAKLJUCENO
        }

        public enum RedCardStatus
        {
            ODPRTO,
            IZVRSEN,
            CEZ_TERMIN,
            TERMIN_V_ZAPADU
        }

        public enum KVPProcessStatus
        {
            Confirm,
            Reject,
            Nothing
        }

        public enum KVPType
        {
            VAR,
            STR,
            QS,
            ZAL,
            SPL,
            EKO,
            ERG,
            EN
        }

        public enum SystemServiceSatus
        {
            UnProcessed = 0,
            Processed = 1,
            Error = 2,
            RecipientError = 3
        }

        public enum PlanRealizationPlanMonth
        { 
            Plan_Jan = 1,
            Plan_Feb= 2,
            Plan_Mar = 3,
            Plan_Apr = 4,
            Plan_Maj = 5,
            Plan_Jun = 6,
            Plan_Jul = 7,
            Plan_Avg = 8,
            Plan_Sep = 9,
            Plan_Okt = 10,
            Plan_Nov = 11,
            Plan_Dec = 12
        }
        public enum PlanRealizationRealMonth
        {
            Real_Jan = 1,
            Real_Feb = 2,
            Real_Mar = 3,
            Real_Apr = 4,
            Real_Maj = 5,
            Real_Jun = 6,
            Real_Jul = 7,
            Real_Avg = 8,
            Real_Sep = 9,
            Real_Okt = 10,
            Real_Nov = 11,
            Real_Dec = 12
        }
        public enum PlanRealizationOdstMonth
        {
            Odst_Jan = 1,
            Odst_Feb = 2,
            Odst_Mar = 3,
            Odst_Apr = 4,
            Odst_Maj = 5,
            Odst_Jun = 6,
            Odst_Jul = 7,
            Odst_Avg = 8,
            Odst_Sep = 9,
            Odst_Okt = 10,
            Odst_Nov = 11,
            Odst_Dec = 12
        }

        public enum SubmitProposalType
        { 
            SubmitProposalToLeader,
            SubmitProposalToChampion,
            SubmitProposalAndReject,
            OnlySaveProposal,
            Nothing
        }

        public enum KVPCommentCode
        { 
            LeaderNotes
        }

        public enum Department
        { 
            DepartmentID,
            DepartmentModel
        }

        public enum Post
        { 
            PostID,
            PostModel
        }

        public enum Cookies
        {
            UserLastRequest,
            SessionExpires
        }

        public enum CookieCommonValue
        {
            STOP
        }
    }
}