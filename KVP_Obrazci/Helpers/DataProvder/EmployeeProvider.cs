using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Helpers.DataProvder
{
    public class EmployeeProvider : ServerMasterPage
    {
        /// <summary>
        /// Add Users instance to session
        /// </summary>
        /// <param name="model"></param>
        public void SetEmployeeModel(Users model)
        {
            AddValueToSession(Enums.Employee.EmployeeModel, model);
        }

        /// <summary>
        /// Returns Users data From session. If session does not exist it returs null.
        /// </summary>
        /// <returns></returns>
        public Users GetEmployeeModel()
        {
            if (SessionHasValue(Enums.Employee.EmployeeModel))
                return (Users)GetValueFromSession(Enums.Employee.EmployeeModel);

            return null;
        }
    }
}