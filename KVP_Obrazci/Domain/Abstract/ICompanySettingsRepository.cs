using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface ICompanySettingsRepository
    {
        Nastavitve GetCompanySettings();

        void SaveCompanySettings(Nastavitve model);

        bool IsEmailSendingEnabled();

        decimal GetPayoutAmount();
        decimal GetQuotientAmount();
        int GetStartKVPNumber();
        void SetNewKVPNumber(int kvpNum);
        int GetAndSetNewKVPNumber();
        int GetStartRedCardNumber();
        void SetNewRedCardNumber(int redCardNum);
        int GetAndSetNewRedCardNumber();
    }
}
