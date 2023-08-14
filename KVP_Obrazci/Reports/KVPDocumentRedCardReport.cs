using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.Xpo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Domain.KVPOdelo;
using System.Linq;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Common;

namespace KVP_Obrazci.Reports
{
    public partial class KVPDocumentRedCardReport : DevExpress.XtraReports.UI.XtraReport
    {
        public KVPDocumentRedCardReport(int idKVPDocumentRedCard, string reportOpenedBy = "")
        {
            InitializeComponent();

            Session xpoSession = XpoHelper.GetNewSession();

            KVPXpCollection.Session = xpoSession;
            session1 = xpoSession;

            kvpDocumentID_param.Value = idKVPDocumentRedCard;
            string imageFile = AppDomain.CurrentDomain.BaseDirectory + "Images\\Logo_Odelo_Siv.png";

            xrPictureBox1.Image = new Bitmap(imageFile);
            xrPictureBox1.ImageUrl = imageFile;
            IKVPDocumentRepository kvpRepo = new KVPDocumentRepository(xpoSession);
            IEmployeeRepository employeeRepo = new EmployeeRepository(xpoSession);

            var obj = kvpRepo.GetKVPByID(idKVPDocumentRedCard);

            if (obj != null && obj.vodja_teama > 0)
            {
                var employee = employeeRepo.GetEmployeeByID(CommonMethods.ParseInt(obj.vodja_teama));

                if (employee != null)
                    lblVodjaTeama.Text = employee.Firstname + " " + employee.Lastname;
            }

            lblUserFirstAndLastname.Text = reportOpenedBy;
            lblPrintDate.Text = DateTime.Now.ToString("dd. MMMM yyyy - hh:mm:ss");
        }

    }
}
