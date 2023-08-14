using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Common
{
    public class EmployeeCredentialsException : Exception
    {
        public EmployeeCredentialsException(string message)
            : base(message)
        {
        }

        public EmployeeCredentialsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}