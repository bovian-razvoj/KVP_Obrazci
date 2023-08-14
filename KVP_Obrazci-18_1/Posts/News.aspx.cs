using DevExpress.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Posts
{
    public partial class News : ServerMasterPage
    {
        Session session;
        IPostRepository postRepo;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            postRepo = new PostRepository(session);

            XPODSPosts.Session = session;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            RedirectWithCustomURI("NewsForm.aspx", (int)Enums.UserAction.Add, 0);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            List<object> obj = ASPxCardViewPosts.GetSelectedFieldValues("PrispevkiID");
            RedirectWithCustomURI("NewsForm.aspx", (int)Enums.UserAction.Edit, obj[0]);
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<object> obj = ASPxCardViewPosts.GetSelectedFieldValues("PrispevkiID");
            RedirectWithCustomURI("NewsForm.aspx", (int)Enums.UserAction.Delete, obj[0]);
        }

        protected void ASPxCardViewPosts_CustomCallback(object sender, DevExpress.Web.ASPxCardViewCustomCallbackEventArgs e)
        {
            List<object> obj = ASPxCardViewPosts.GetSelectedFieldValues("PrispevkiID");
            ASPxWebControl.RedirectOnCallback(GenerateURI("NewsForm.aspx", (int)Enums.UserAction.Edit, obj[0]));
        }

        protected void ASPxCardViewPosts_HtmlCardPrepared(object sender, ASPxCardViewHtmlCardPreparedEventArgs e)
        {
            bool isPublished = CommonMethods.ParseBool(ASPxCardViewPosts.GetCardValues(e.VisibleIndex, "Objavljen"));
            if (isPublished)
            {
                e.Card.ForeColor = Color.LightGreen;
            }
        }

        protected void ASPxCardViewPosts_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Objavljen")
            {
                e.DisplayText = CommonMethods.ParseBool(e.Value) == true ? "DA" : "NE";
            }
        }
    }
}