using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class KVPGroupsRepository : IKVPGroupsRepository
    {
        Session session;
        IEmployeeRepository employeeRepo;
        public KVPGroupsRepository(Session session)
        {
            this.session = session;
            employeeRepo = new EmployeeRepository(this.session);
        }
        public KVPSkupina GetKVPGroupByID(int kvpGroupID, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPSkupina> kvpGroup = null;

                if (currentSession != null)
                    kvpGroup = currentSession.Query<KVPSkupina>();
                else
                    kvpGroup = session.Query<KVPSkupina>();

                return kvpGroup.Where(kvpS => kvpS.idKVPSkupina == kvpGroupID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveKVPGroup(KVPSkupina model)
        {
            try
            {
                bool bIsInsert = false;
                // if (model.idKVPSkupina == 0)
                //   model.ts = DateTime.Now;
                if (model.idKVPSkupina == 0)
                {
                    model.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                    model.ts = DateTime.Now;
                    bIsInsert = true; 
                }

                model.Save();
                
                if (bIsInsert)
                {
                    PlanRealizacija modelPlanRealizacija = null;
                    modelPlanRealizacija = new PlanRealizacija(session);
                    modelPlanRealizacija.Leto = DateTime.Now.Year;
                    modelPlanRealizacija.idKVPSkupina = model;
                    modelPlanRealizacija.Save();
                }


                return model.idKVPSkupina;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_11, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteKVPGroup(KVPSkupina model)
        {
            try
            {
                model.Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_12, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteKVPGroup(int kvpGroupID)
        {
            try
            {
                GetKVPGroupByID(kvpGroupID).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_12, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public void SaveEmployeesToKVPGroup(List<object> selectedRows, int kvpGroupID, bool isChampions = false, bool isNewEmployee=false)
        {
            try
            {
                using (UnitOfWork uow = XpoHelper.GetNewUnitOfWork())
                {
                    KVPSkupina_Users users = null;

                    foreach (var item in selectedRows)
                    {
                        int id = CommonMethods.ParseInt(item);
                        users = GetKVPGroupUserByUserID(id, uow);
                        if (users != null && isChampions)
                        {
                            users.Champion = true;
                            users.IdUser.RoleID = employeeRepo.GetRoleByID(4, uow);
                        }
                        else
                        {
                            users = new KVPSkupina_Users(uow);
                            users.idKVPSkupina = GetKVPGroupByID(kvpGroupID, uow);

                            users.IdUser = employeeRepo.GetEmployeeByID(id, uow);
                            users.Champion = isChampions;
                            users.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                            users.ts = DateTime.Now;

                            if (isChampions) users.IdUser.RoleID = employeeRepo.GetRoleByID(4, uow);
                        }

                        if (isNewEmployee) users.IdUser.NewEmployee = 0;
                    }
                    uow.CommitChanges();
                }

            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_11, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public KVPSkupina_Users GetKVPGroupUserByUserID(int userID, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPSkupina_Users> kvpGroupUsers = null;

                if (currentSession != null)
                    kvpGroupUsers = currentSession.Query<KVPSkupina_Users>();
                else
                    kvpGroupUsers = session.Query<KVPSkupina_Users>();

                return kvpGroupUsers.Where(kvpS => kvpS.IdUser.Id == userID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteEmployeesFromKVPGroupUsers(List<int> selectedItems)
        {
            try
            {
                IList<KVPSkupina_Users> list = new XPCollection<KVPSkupina_Users>(session).ToList().Where(l => selectedItems.Contains(l.idKVPSkupina_Zaposleni)).ToList();
                XPCollection<KVPSkupina_Users> collection = new XPCollection<KVPSkupina_Users>(session, list);
                session.Delete(collection);
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_12, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<KVPSkupina_Users> GetKVPGroupUsersChampionsByGroupID(int kvpGroupID)
        {
            try
            {
                XPQuery<KVPSkupina_Users> kvpGroupUsers = session.Query<KVPSkupina_Users>();

                return kvpGroupUsers.Where(kvpS => kvpS.idKVPSkupina.idKVPSkupina == kvpGroupID && kvpS.Champion == true).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }
        public List<Users> GetUsersFromKVPGroupByID(int kvpGroupID)
        {
            try
            {
                XPQuery<KVPSkupina_Users> users = session.Query<KVPSkupina_Users>();

                List<KVPSkupina_Users> list = users.Where(kvpS => kvpS.idKVPSkupina.idKVPSkupina == kvpGroupID).ToList();

                return list.Select(u => u.IdUser).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetUserCountWithNoKVPGroup()
        {
            try
            {
                XPQuery<KVPSkupina_Users> kvpGroupUsers = session.Query<KVPSkupina_Users>();
                XPQuery<Users> users = session.Query<Users>();

                return users.Where(u => !kvpGroupUsers.Any(ku => ku.IdUser.Id == u.Id)).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int GetDeletedUserCountWithKVPGroup()
        {
            try
            {
                XPQuery<KVPSkupina_Users> kvpGroupUsers = session.Query<KVPSkupina_Users>();
                XPQuery<Users> users = session.Query<Users>();

                return users.Where(u => kvpGroupUsers.Any(ku => ku.IdUser.Id == u.Id) && u.Deleted).Count();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<Users> GetKVPGroupChampionsByKVPGroupID(int id)
        {
            try
            {
                XPQuery<KVPSkupina_Users> kvpGroupUsers = session.Query<KVPSkupina_Users>();


                return kvpGroupUsers.Where(g => g.idKVPSkupina.idKVPSkupina == id && g.Champion == true).Select(g => g.IdUser).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public KVPSkupina GetKVPGroupByCode(string kvpCode, Session currentSession = null)
        {
            try
            {
                XPQuery<KVPSkupina> kvpGroup = null;

                if (currentSession != null)
                    kvpGroup = currentSession.Query<KVPSkupina>();
                else
                    kvpGroup = session.Query<KVPSkupina>();

                return kvpGroup.Where(kvpS => kvpS.Koda == kvpCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}