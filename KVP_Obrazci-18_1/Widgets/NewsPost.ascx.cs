using KVP_Obrazci.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Widgets
{
    public partial class NewsPost : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void FillPostControl(PostModel model)
        {
            postTitle.InnerText = model.Naslov;
            metaDataParagraph.InnerHtml = "<strong>" + model.DatumVnosa.ToString("dd. MMMM yyyy") + " - " + model.Avtor + "</strong>";
            bodyPost.InnerHtml = model.Besedilo;
        }
    }
}