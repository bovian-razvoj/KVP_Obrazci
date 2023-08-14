using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace KVP_Obrazci.Domain.KVPOdelo
{

    public partial class Tip
    {
        public Tip(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
