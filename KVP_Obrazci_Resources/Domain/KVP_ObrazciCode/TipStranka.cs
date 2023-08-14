using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace KVP_Obrazci.Domain.GrafolitOTP
{

    public partial class TipStranka
    {
        public TipStranka(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
