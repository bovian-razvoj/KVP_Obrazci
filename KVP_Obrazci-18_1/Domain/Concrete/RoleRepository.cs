using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class RoleRepository : IRoleRepository
    {
        Session session;

        public RoleRepository(Session session)
        {
            this.session = session;
        }


        public List<Vloga> GetAllRoles()
        {
            try
            {
                XPQuery<Vloga> roles = session.Query<Vloga>();
                return roles.Where(v => v.VlogaID == v.VlogaID).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_16, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public Vloga GetRoleByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Vloga> role = null;

                if (currentSession != null)
                    role = currentSession.Query<Vloga>();
                else
                    role = session.Query<Vloga>();

                return role.Where(v => v.VlogaID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_16, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}