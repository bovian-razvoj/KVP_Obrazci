using DevExpress.Xpo;
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
        List<KVPEmployee> GetEmployeesToSendMail();

        void ProceesKVPsToSendEmployeeStatistic();
        void ProcessRejectedKVPToSend(int informedEmployeeID, int kvpDocID, Session currentSession = null);
        void ProcessNewKVPCredentialsToSend(int kvpUserID, Session currentSession = null);
        void ProcessInfoKVPMailToSend(int recieverUserID, InfoMailModel model, Session currentSession = null);
    }
}
