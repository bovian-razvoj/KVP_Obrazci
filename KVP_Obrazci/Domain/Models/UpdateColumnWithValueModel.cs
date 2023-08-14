using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class UpdateColumnWithValueModel
    {
        public string ColumnName { get; set; }
        public object ColumnValue { get; set; }
    }
}