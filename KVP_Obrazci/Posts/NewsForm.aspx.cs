using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Posts
{
    public partial class NewsForm : ServerMasterPage
    {
        Session session;
        int action = -1;
        int postID = -1;
        Prispevek model;

        IPostRepository postRepo;
        IEmployeeRepository employeeRepo;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
            postID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            session = XpoHelper.GetNewSession();

            postRepo = new PostRepository(session);
            employeeRepo = new EmployeeRepository(session);

            this.Master.DisableNavBar = true;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();

                if (action != (int)Enums.UserAction.Add)
                {
                    if (postID > 0)
                    {
                        if (GetPostProvider().GetPostModel() != null)
                            model = GetPostProvider().GetPostModel();
                        else
                            model = postRepo.GetPostByID(postID);

                        if (model != null)
                        {
                            GetPostProvider().SetPostModel(model);
                            FillForm();
                        }
                    }
                }
                else //User action => Add
                {
                    SetFormDefaultValues();
                }
                UserActionConfirmBtnUpdate(btnConfirm, action);
            }
            else
            {
                if (GetEmployeeProvider().GetEmployeeModel() != null)
                    model = GetPostProvider().GetPostModel();
            }
        }

        private void FillForm()
        {
            txtNewsTitle.Text = model.Naslov;
            ASPxMemoExcerpt.Text = model.Izvlecek;
            ASPxHtmlEditorPost.Html = model.Besedilo;
            CheckBoxPublish.Checked = model.Objavljen;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new Prispevek(session);
                model.PrispevkiID = 0;
                model.PrikaznaSlika = "~/Images/post_note.png";
            }
            else if (model == null && !add)
            {
                model = GetPostProvider().GetPostModel();
                model.PrispevkiID = model.PrispevkiID;
            }

            model.Naslov = txtNewsTitle.Text;
            model.Besedilo = ASPxHtmlEditorPost.Html;
            model.Izvlecek = ASPxMemoExcerpt.Text;

            model.AvtorID = employeeRepo.GetEmployeeByID(PrincipalHelper.GetUserPrincipal().ID, model.Session);
            model.KategorijaID = postRepo.GetPostCategoryByID(1, model.Session);
            model.Objavljen = CheckBoxPublish.Checked;

            model.PrispevkiID = postRepo.SavePost(model);
            GetPostProvider().SetPostModel(model);

            return true;
        }

        #region Initialization

        private void Initialize()
        { }

        private void SetFormDefaultValues()
        { }

        #endregion

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            bool isValid = false;
            bool isDeleteing = false;

            switch (action)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    break;
                case (int)Enums.UserAction.Delete:
                    postRepo.DeletePost(postID);
                    isValid = true;
                    isDeleteing = true;
                    break;
            }

            if (isValid)
            {
                ClearSessionsAndRedirect(isDeleteing);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSessionsAndRedirect();
        }


        private void ClearSessionsAndRedirect(bool isIDDeleted = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = postID.ToString() } 
            };

            if (isIDDeleted)
                redirectString = "News.aspx";
            else
                redirectString = GenerateURI("News.aspx", queryStrings);

            List<Enums.Post> list = Enum.GetValues(typeof(Enums.Post)).Cast<Enums.Post>().ToList();
            ClearAllSessions(list, redirectString);
        }
    }
}