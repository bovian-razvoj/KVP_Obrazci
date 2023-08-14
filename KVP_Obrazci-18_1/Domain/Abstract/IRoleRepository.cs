using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IRoleRepository
    {
        List<Vloga> GetAllRoles();
        Vloga GetRoleByID(int id, Session currentSession = null);
    }
}
