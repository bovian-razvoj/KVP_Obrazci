using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class KodeksDepartmentsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DepartmentHeadId { get; set; }
        public int DepartmentHeadDeputyId { get; set; }
        public int ParentId { get; set; }
    }
}