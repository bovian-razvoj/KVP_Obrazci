using DevExpress.Data.Filtering;
using DevExpress.Web.Data;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class PlaningAndRealizationRepository : IPlaningAndRealizationRepository
    {
        Session session = null;
        IKVPGroupsRepository kvpGroupRepo;

        public PlaningAndRealizationRepository(Session session)
        {
            this.session = session;
            kvpGroupRepo = new KVPGroupsRepository(session);
        }

        public PlanRealizacija GetPlanRealizationByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<PlanRealizacija> planRealization = null;

                if (currentSession != null)
                    planRealization = currentSession.Query<PlanRealizacija>();
                else
                    planRealization = session.Query<PlanRealizacija>();

                return planRealization.Where(p => p.idPlanRealizacija == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void SavePlaningFromBatchUpdate(List<ASPxDataUpdateValues> updateValues)
        {
            try
            {
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    PlanRealizacija planRealization = null;
                    Type myType = typeof(PlanRealizacija);
                    List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                    foreach (ASPxDataUpdateValues item in updateValues)
                    {
                        planRealization = new PlanRealizacija(session);

                        foreach (DictionaryEntry obj in item.Keys)//we set table ID
                        {
                            PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                            if (info != null)
                            {
                                planRealization = GetPlanRealizationByID((int)obj.Value, uow);
                                break;
                            }
                        }

                        foreach (DictionaryEntry obj in item.NewValues)
                        {

                            PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                            if (info != null)
                                info.SetValue(planRealization, obj.Value);
                        }
                    }
                    uow.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public PlanRealizacija GetPlanRealizationByKVPGroupAndYear(int kvpGroupID, int year, Session currentSession = null)
        {
            try
            {
                XPQuery<PlanRealizacija> planRealization = null;

                if (currentSession != null)
                    planRealization = currentSession.Query<PlanRealizacija>();
                else
                    planRealization = session.Query<PlanRealizacija>();

                return planRealization.Where(p => p.idKVPSkupina.idKVPSkupina == kvpGroupID && p.Leto == year).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void UpdatePlanRealizationByKvpSubmit(int userID, int year, int month, int quantity, Session currentSession = null)
        {
            try
            {
                KVPSkupina_Users kvpGroupUsers = kvpGroupRepo.GetKVPGroupUserByUserID(userID, currentSession);
                if (kvpGroupUsers != null)
                {
                    PlanRealizacija planRealization = GetPlanRealizationByKVPGroupAndYear(kvpGroupUsers.idKVPSkupina.idKVPSkupina, year, currentSession);

                    Type myType = typeof(PlanRealizacija);
                    List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();
                    string columnToUpdate = Enum.GetName(typeof(Enums.PlanRealizationRealMonth), month);

                    PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(columnToUpdate)).FirstOrDefault();

                    if (info != null)
                    {
                        decimal oldValue = CommonMethods.ParseDecimal(info.GetValue(planRealization));
                        decimal newValue = oldValue + quantity;
                        info.SetValue(planRealization, newValue);

                        //TODO Preračunaj odstotke
                        string columnToUpdatePlan = Enum.GetName(typeof(Enums.PlanRealizationPlanMonth), month);
                        PropertyInfo planInfo = myPropInfo.Where(prop => prop.Name.Equals(columnToUpdatePlan)).FirstOrDefault();
                        decimal plan = CommonMethods.ParseDecimal(planInfo.GetValue(planRealization));
                        decimal newPercentage = (newValue / plan) * 100;

                        string columnToUpdatePercentage = Enum.GetName(typeof(Enums.PlanRealizationOdstMonth), month);
                        PropertyInfo percentageInfo = myPropInfo.Where(prop => prop.Name.Equals(columnToUpdatePercentage)).FirstOrDefault();
                        percentageInfo.SetValue(planRealization, newPercentage);
                    }

                    planRealization.Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_26, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public XPCollection<PlanRealizacija> GetPlanRealizationByKVPGroupAndYearWithSumAndYTD(int year, Session currentSession = null)
        {
            try
            {
                decimal dSumP_Jan = 0, dSumP_Feb = 0, dSumP_Mar = 0, dSumP_Apr = 0, dSumP_Maj = 0, dSumP_Jun = 0, dSumP_Jul = 0, dSumP_Avg = 0, dSumP_Sep = 0, dSumP_Okt = 0, dSumP_Nov = 0, dSumP_Dec = 0;
                decimal dYtdP_Jan = 0, dYtdP_Feb = 0, dYtdP_Mar = 0, dYtdP_Apr = 0, dYtdP_Maj = 0, dYtdP_Jun = 0, dYtdP_Jul = 0, dYtdP_Avg = 0, dYtdP_Sep = 0, dYtdP_Okt = 0, dYtdP_Nov = 0, dYtdP_Dec = 0;

                decimal dSumR_Jan = 0, dSumR_Feb = 0, dSumR_Mar = 0, dSumR_Apr = 0, dSumR_Maj = 0, dSumR_Jun = 0, dSumR_Jul = 0, dSumR_Avg = 0, dSumR_Sep = 0, dSumR_Okt = 0, dSumR_Nov = 0, dSumR_Dec = 0;
                decimal dYtdR_Jan = 0, dYtdR_Feb = 0, dYtdR_Mar = 0, dYtdR_Apr = 0, dYtdR_Maj = 0, dYtdR_Jun = 0, dYtdR_Jul = 0, dYtdR_Avg = 0, dYtdR_Sep = 0, dYtdR_Okt = 0, dYtdR_Nov = 0, dYtdR_Dec = 0;

                decimal dSumO_Jan = 0, dSumO_Feb = 0, dSumO_Mar = 0, dSumO_Apr = 0, dSumO_Maj = 0, dSumO_Jun = 0, dSumO_Jul = 0, dSumO_Avg = 0, dSumO_Sep = 0, dSumO_Okt = 0, dSumO_Nov = 0, dSumO_Dec = 0;
                decimal dYtdO_Jan = 0, dYtdO_Feb = 0, dYtdO_Mar = 0, dYtdO_Apr = 0, dYtdO_Maj = 0, dYtdO_Jun = 0, dYtdO_Jul = 0, dYtdO_Avg = 0, dYtdO_Sep = 0, dYtdO_Okt = 0, dYtdO_Nov = 0, dYtdO_Dec = 0;

                CriteriaOperator filterCriteria = CriteriaOperator.Parse("Leto =" + year);

                XPCollection<PlanRealizacija> collection_KVP = new XPCollection<PlanRealizacija>(session, filterCriteria);
                foreach (PlanRealizacija pln in collection_KVP)
                {
                    dSumP_Jan += pln.Plan_Jan;
                    dSumP_Feb += pln.Plan_Feb;
                    dSumP_Mar += pln.Plan_Mar;
                    dSumP_Apr += pln.Plan_Apr;
                    dSumP_Maj += pln.Plan_Maj;
                    dSumP_Jun += pln.Plan_Jun;
                    dSumP_Jul += pln.Plan_Jul;
                    dSumP_Avg += pln.Plan_Avg;
                    dSumP_Sep += pln.Plan_Sep;
                    dSumP_Okt += pln.Plan_Okt;
                    dSumP_Nov += pln.Plan_Nov;
                    dSumP_Dec += pln.Plan_Dec;

                    dSumR_Jan += pln.Real_Jan;
                    dSumR_Feb += pln.Real_Feb;
                    dSumR_Mar += pln.Real_Mar;
                    dSumR_Apr += pln.Real_Apr;
                    dSumR_Maj += pln.Real_Maj;
                    dSumR_Jun += pln.Real_Jun;
                    dSumR_Jul += pln.Real_Jul;
                    dSumR_Avg += pln.Real_Avg;
                    dSumR_Sep += pln.Real_Sep;
                    dSumR_Okt += pln.Real_Okt;
                    dSumR_Nov += pln.Real_Nov;
                    dSumR_Dec += pln.Real_Dec;

                    dSumO_Jan += pln.Odst_Jan;
                    dSumO_Feb += pln.Odst_Feb;
                    dSumO_Mar += pln.Odst_Mar;
                    dSumO_Apr += pln.Odst_Apr;
                    dSumO_Maj += pln.Odst_Maj;
                    dSumO_Jun += pln.Odst_Jun;
                    dSumO_Jul += pln.Odst_Jul;
                    dSumO_Avg += pln.Odst_Avg;
                    dSumO_Sep += pln.Odst_Sep;
                    dSumO_Okt += pln.Odst_Okt;
                    dSumO_Nov += pln.Odst_Nov;
                    dSumO_Dec += pln.Odst_Dec;


                }

                dYtdP_Jan = dSumP_Jan;
                dYtdP_Feb = dYtdP_Jan + dSumP_Feb;
                dYtdP_Mar = dYtdP_Feb + dSumP_Mar;
                dYtdP_Apr = dYtdP_Mar + dSumP_Apr;
                dYtdP_Maj = dYtdP_Apr + dSumP_Maj;
                dYtdP_Jun = dYtdP_Maj + dSumP_Jun;
                dYtdP_Jul = dYtdP_Jun + dSumP_Jul;
                dYtdP_Avg = dYtdP_Jul + dSumP_Avg;
                dYtdP_Sep = dYtdP_Avg + dSumP_Sep;
                dYtdP_Okt = dYtdP_Sep + dSumP_Okt;
                dYtdP_Nov = dYtdP_Okt + dSumP_Nov;
                dYtdP_Dec = dYtdP_Nov + dSumP_Dec;

                dYtdR_Jan = dSumR_Jan;
                dYtdR_Feb = dYtdR_Jan + dSumR_Feb;
                dYtdR_Mar = dYtdR_Feb + dSumR_Mar;
                dYtdR_Apr = dYtdR_Mar + dSumR_Apr;
                dYtdR_Maj = dYtdR_Apr + dSumR_Maj;
                dYtdR_Jun = dYtdR_Maj + dSumR_Jun;
                dYtdR_Jul = dYtdR_Jun + dSumR_Jul;
                dYtdR_Avg = dYtdR_Jul + dSumR_Avg;
                dYtdR_Sep = dYtdR_Avg + dSumR_Sep;
                dYtdR_Okt = dYtdR_Sep + dSumR_Okt;
                dYtdR_Nov = dYtdR_Okt + dSumR_Nov;
                dYtdR_Dec = dYtdR_Nov + dSumR_Dec;

                dYtdO_Jan = Math.Round((dYtdR_Jan / dYtdP_Jan) * 100,2);
                dYtdO_Feb = Math.Round((dYtdR_Feb / dYtdP_Feb) * 100,2);
                dYtdO_Mar = Math.Round((dYtdR_Mar / dYtdP_Mar) * 100,2);
                dYtdO_Apr = Math.Round((dYtdR_Apr / dYtdP_Apr) * 100,2);
                dYtdO_Maj = Math.Round((dYtdR_Maj / dYtdP_Maj) * 100,2);
                dYtdO_Jun = Math.Round((dYtdR_Jun / dYtdP_Jun) * 100,2);
                dYtdO_Jul = Math.Round((dYtdR_Jul / dYtdP_Jul) * 100,2);
                dYtdO_Avg = Math.Round((dYtdR_Avg / dYtdP_Avg) * 100,2);
                dYtdO_Sep = Math.Round((dYtdR_Sep / dYtdP_Sep) * 100,2);
                dYtdO_Okt = Math.Round((dYtdR_Okt / dYtdP_Okt) * 100,2);
                dYtdO_Nov = Math.Round((dYtdR_Nov / dYtdP_Nov) * 100,2);
                dYtdO_Dec = Math.Round((dYtdR_Dec / dYtdP_Dec) * 100,2);

                PlanRealizacija plnSumMonth = new PlanRealizacija(session);
                plnSumMonth.idPlanRealizacija = -1;
                KVPSkupina kvpSkSumMonth = new KVPSkupina(session);
                kvpSkSumMonth.idKVPSkupina = -1;
                kvpSkSumMonth.Koda = "SUMMONTH";
                kvpSkSumMonth.Naziv = "Mesečno skupaj";

                plnSumMonth.Leto = DateTime.Now.Year;
                plnSumMonth.Plan_Jan = dSumP_Jan;
                plnSumMonth.Plan_Feb = dSumP_Feb;
                plnSumMonth.Plan_Mar = dSumP_Mar;
                plnSumMonth.Plan_Apr = dSumP_Apr;
                plnSumMonth.Plan_Maj = dSumP_Maj;
                plnSumMonth.Plan_Jun = dSumP_Jun;
                plnSumMonth.Plan_Jul = dSumP_Jul;
                plnSumMonth.Plan_Avg = dSumP_Avg;
                plnSumMonth.Plan_Sep = dSumP_Sep;
                plnSumMonth.Plan_Okt = dSumP_Okt;
                plnSumMonth.Plan_Nov = dSumP_Nov;
                plnSumMonth.Plan_Dec = dSumP_Dec;

                plnSumMonth.Real_Jan = dSumR_Jan;
                plnSumMonth.Real_Feb = dSumR_Feb;
                plnSumMonth.Real_Mar = dSumR_Mar;
                plnSumMonth.Real_Apr = dSumR_Apr;
                plnSumMonth.Real_Maj = dSumR_Maj;
                plnSumMonth.Real_Jun = dSumR_Jun;
                plnSumMonth.Real_Jul = dSumR_Jul;
                plnSumMonth.Real_Avg = dSumR_Avg;
                plnSumMonth.Real_Sep = dSumR_Sep;
                plnSumMonth.Real_Okt = dSumR_Okt;
                plnSumMonth.Real_Nov = dSumR_Nov;
                plnSumMonth.Real_Dec = dSumR_Dec;

                plnSumMonth.Odst_Jan = Math.Round(((dSumR_Jan / dSumP_Jan) * 100), 2);
                plnSumMonth.Odst_Feb = Math.Round(((dSumR_Feb / dSumP_Feb) * 100), 2);
                plnSumMonth.Odst_Mar = Math.Round(((dSumR_Mar / dSumP_Mar) * 100), 2);
                plnSumMonth.Odst_Apr = Math.Round(((dSumR_Apr / dSumP_Apr) * 100), 2);
                plnSumMonth.Odst_Maj = Math.Round(((dSumR_Maj / dSumP_Maj) * 100), 2);
                plnSumMonth.Odst_Jun = Math.Round(((dSumR_Jun / dSumP_Jun) * 100), 2);
                plnSumMonth.Odst_Jul = Math.Round(((dSumR_Jul / dSumP_Jul) * 100), 2);
                plnSumMonth.Odst_Avg = Math.Round(((dSumR_Avg / dSumP_Avg) * 100), 2);
                plnSumMonth.Odst_Sep = Math.Round(((dSumR_Sep / dSumP_Sep) * 100), 2);
                plnSumMonth.Odst_Okt = Math.Round(((dSumR_Okt / dSumP_Okt) * 100), 2);
                plnSumMonth.Odst_Nov = Math.Round(((dSumR_Nov / dSumP_Nov) * 100), 2);
                plnSumMonth.Odst_Dec = Math.Round(((dSumR_Dec / dSumP_Dec) * 100), 2);

                plnSumMonth.idKVPSkupina = kvpSkSumMonth;
                collection_KVP.Add(plnSumMonth);

                PlanRealizacija plnYtd = new PlanRealizacija(session);
                plnYtd.idPlanRealizacija = -2;
                KVPSkupina kvpYtdP = new KVPSkupina(session);
                kvpYtdP.idKVPSkupina = -2;
                kvpYtdP.Koda = "YTDMONTH";
                kvpYtdP.Naziv = "YTD";

                plnYtd.Leto = DateTime.Now.Year;
                plnYtd.Plan_Jan = dYtdP_Jan;
                plnYtd.Plan_Feb = dYtdP_Feb;
                plnYtd.Plan_Mar = dYtdP_Mar;
                plnYtd.Plan_Apr = dYtdP_Apr;
                plnYtd.Plan_Maj = dYtdP_Maj;
                plnYtd.Plan_Jun = dYtdP_Jun;
                plnYtd.Plan_Jul = dYtdP_Jul;
                plnYtd.Plan_Avg = dYtdP_Avg;
                plnYtd.Plan_Sep = dYtdP_Sep;
                plnYtd.Plan_Okt = dYtdP_Okt;
                plnYtd.Plan_Nov = dYtdP_Nov;
                plnYtd.Plan_Dec = dYtdP_Dec;

                plnYtd.Real_Jan = dYtdR_Jan;
                plnYtd.Real_Feb = dYtdR_Feb;
                plnYtd.Real_Mar = dYtdR_Mar;
                plnYtd.Real_Apr = dYtdR_Apr;
                plnYtd.Real_Maj = dYtdR_Maj;
                plnYtd.Real_Jun = dYtdR_Jun;
                plnYtd.Real_Jul = dYtdR_Jul;
                plnYtd.Real_Avg = dYtdR_Avg;
                plnYtd.Real_Sep = dYtdR_Sep;
                plnYtd.Real_Okt = dYtdR_Okt;
                plnYtd.Real_Nov = dYtdR_Nov;
                plnYtd.Real_Dec = dYtdR_Dec;

                plnYtd.Odst_Jan = dYtdO_Jan;
                plnYtd.Odst_Feb = dYtdO_Feb;
                plnYtd.Odst_Mar = dYtdO_Mar;
                plnYtd.Odst_Apr = dYtdO_Apr;
                plnYtd.Odst_Maj = dYtdO_Maj;
                plnYtd.Odst_Jun = dYtdO_Jun;
                plnYtd.Odst_Jul = dYtdO_Jul;
                plnYtd.Odst_Avg = dYtdO_Avg;
                plnYtd.Odst_Sep = dYtdO_Sep;
                plnYtd.Odst_Okt = dYtdO_Okt;
                plnYtd.Odst_Nov = dYtdO_Nov;
                plnYtd.Odst_Dec = dYtdO_Dec;

                plnYtd.idKVPSkupina = kvpYtdP;
                collection_KVP.Add(plnYtd);


                return collection_KVP;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void RefreshKVPRealizationPercentage(Session currentSession = null)
        {
            try
            {
                CriteriaOperator filterCriteria = CriteriaOperator.Parse("Leto =" + DateTime.Now.Year);

                XPCollection<PlanRealizacija> collection_KVP = new XPCollection<PlanRealizacija>(session, filterCriteria);
                foreach (PlanRealizacija pln in collection_KVP)
                {
                    pln.Odst_Jan = Math.Round((pln.Plan_Jan > 0 ? (pln.Real_Jan / pln.Plan_Jan) * 100 : 0),2);
                    pln.Odst_Feb = Math.Round((pln.Plan_Feb > 0 ? (pln.Real_Feb / pln.Plan_Feb) * 100 : 0),2);
                    pln.Odst_Mar = Math.Round((pln.Plan_Mar > 0 ? (pln.Real_Mar / pln.Plan_Mar) * 100 : 0),2);
                    pln.Odst_Apr = Math.Round((pln.Plan_Apr > 0 ? (pln.Real_Apr / pln.Plan_Apr) * 100 : 0),2);
                    pln.Odst_Maj = Math.Round((pln.Plan_Maj > 0 ? (pln.Real_Maj / pln.Plan_Maj) * 100 : 0),2);
                    pln.Odst_Jun = Math.Round((pln.Plan_Jun > 0 ? (pln.Real_Jun / pln.Plan_Jun) * 100 : 0),2);
                    pln.Odst_Jul = Math.Round((pln.Plan_Jul > 0 ? (pln.Real_Jul / pln.Plan_Jul) * 100 : 0),2);
                    pln.Odst_Avg = Math.Round((pln.Plan_Avg > 0 ? (pln.Real_Avg / pln.Plan_Avg) * 100 : 0),2);
                    pln.Odst_Sep = Math.Round((pln.Plan_Sep > 0 ? (pln.Real_Sep / pln.Plan_Sep) * 100 : 0),2);
                    pln.Odst_Okt = Math.Round((pln.Plan_Okt > 0 ? (pln.Real_Okt / pln.Plan_Okt) * 100 : 0),2);
                    pln.Odst_Nov = Math.Round((pln.Plan_Nov > 0 ? (pln.Real_Nov / pln.Plan_Nov) * 100 : 0),2);
                    pln.Odst_Dec = Math.Round((pln.Plan_Dec > 0 ? (pln.Real_Dec / pln.Plan_Dec) * 100 : 0),2);

                    pln.Save();
                }
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_25, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}