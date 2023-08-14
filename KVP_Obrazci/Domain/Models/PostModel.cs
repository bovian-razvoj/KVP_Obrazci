using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Models
{
    public class PostModel
    {
        public string Naslov { get; set; }
        public string Avtor { get; set; }
        public string Kategorija { get; set; }
        public string Besedilo { get; set; }
        public DateTime DatumVnosa { get; set; }
        public string PrikaznaSlika { get; set; }
        public DateTime DatumSpremembe { get; set; }
    }
}