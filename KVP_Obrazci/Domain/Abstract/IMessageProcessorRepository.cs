using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IMessageProcessorRepository
    {
        List<KVPEmployee> GetEmployeesToSendMail(Session session = null);

        void ProceesKVPsToSendEmployeeStatistic(Session session = null);
        void ProcessRejectedKVPToSend(int informedEmployeeID, int kvpDocID, Session currentSession = null);
        void ProcessNewKVPCredentialsToSend(int kvpUserID, Session currentSession = null);
        void ProcessInfoKVPMailToSend(int recieverUserID, InfoMailModel model, Session currentSession = null, bool sendToSender = false);
        int ProcessChangedUserNameToSend(int recieverUserID, InfoMailModel model, Session currentSession = null, bool sendToSender = false);
        void DisposeSession();
        void ProcessSecurityInfoRedCardMailToSend(KVPDocument redCardModel, Session currentSession = null);
    }
}
