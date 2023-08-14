using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentHeadId { get; set; }
        public string DepartmentHeadName { get; set; }
        public int DepartmentHeadDeputyId { get; set; }
        public string DepartmentHeadDeputyName { get; set; }
        public int ParentId { get; set; }
        public string DepartmentSupName { get; set; }
    }
}