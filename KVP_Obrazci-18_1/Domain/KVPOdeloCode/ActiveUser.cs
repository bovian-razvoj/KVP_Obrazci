﻿using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace KVP_Obrazci.Domain.KVPOdelo
{

    public partial class ActiveUser
    {
        public ActiveUser(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
