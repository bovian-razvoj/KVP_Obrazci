using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.Xpo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Domain.KVPOdelo;

namespace KVP_Obrazci.Reports
{
    public partial class MonthPayout : DevExpress.XtraReports.UI.XtraReport
    {
        public MonthPayout()
        {
            InitializeComponent();

            Session xpoSession = XpoHelper.GetNewSession();

            PayoutXpCollection.Session = xpoSession;
            session1 = xpoSession;

            XPCollection<Izplacila> collection_nastavi = new XPCollection<Izplacila>(session1);
        }

    }
}
