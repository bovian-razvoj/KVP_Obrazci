using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IMessageSenderRepository
    {
        void UpdateFailedMessges();
        List<SystemEmailMessage> GetUnprocessedEmails();
        void SaveEmail(SystemEmailMessage model);
    }
}
