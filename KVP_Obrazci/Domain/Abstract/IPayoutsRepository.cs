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
    public interface IPayoutsRepository
    {
        List<Izplacila> GetPayoutsForMonthAndYear(string month, int year);
        Izplacila GetPayoutsForMonthAndYearAndUserId(string month, int year, Users usrUser);
        int SavePayout(Izplacila model);
        void SavePayoutsForNewMonth(List<Izplacila> payouts, DateTime date, bool bAddBlankrecord, bool bSetIzplacilo = false);
        bool ExistPayoutsForCurrentMonthAndYear(string month, int year);

        Izplacila UserPayoutRecordForMonthAndYear(int userID, string month, int year, Session currentSession = null);
        void UpdatePayoutsForNewMonth(List<Izplacila> payouts, DateTime date);
        Izplacila GetPayoutByID(int id, Session currentSession);
        Izplacila GetLastPayoutByUserID(int userID);
        decimal GetActualPointsForUser(int userID);

        List<PayoutOverviewByPeriodModel> GetPayoutsByPeriod(DateTime firstDate, DateTime lastDate, string employee);

        void CreateNewPayout(KVPDocument model, bool isRealizator = false);
    }
}
