using DevExpress.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Posts
{
    public partial class News : ServerMasterPage
    {
        Session session;
        IPostRepository postRepo;
        ICompanySettingsRepository companySettingsRepo;
        Nastavitve model;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            session = XpoHelper.GetNewSession();

            postRepo = new PostRepository(session);
            companySettingsRepo = new CompanySettingsRepository(session);

            XPODSPosts.Session = session;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            model = companySettingsRepo.GetCompanySettings();

            if (SessionHasValue(Enums.CommonSession.DownloadDocument))
            {
                DocumentEntity obj = (DocumentEntity)GetValueFromSession(Enums.CommonSession.DownloadDocument);

                byte[] byteFile = File.ReadAllBytes(Server.MapPath(obj.Url));
                string resultExtension = Path.GetExtension(obj.Name);
                string format = "pdf";
                if (resultExtension.Equals(".jpg"))
                    format = "jpg";
                else if (resultExtension.Equals(".jpeg"))
                    format = "jpeg";
                else if (resultExtension.Equals(".png"))
                    format = "png";
                RemoveSession(Enums.CommonSession.DownloadDocument);
                WriteDocumentToResponse(byteFile, format, false, obj.Name);
            }
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

        #region UserManual
        protected void UserManualUploadControl_PopulateAttachments(object sender, EventArgs e)
        {
            companySettingsRepo = new CompanySettingsRepository(XpoHelper.GetNewSession());
            model = companySettingsRepo.GetCompanySettings();
            if (model != null && !String.IsNullOrEmpty(model.PotNavodilaZaUporabo))
            {
                List<DocumentEntity> list = GetDocumentsFromCompanySettings();

                (sender as UploadAttachment).files = list;
                HtmlGenericControl control = (HtmlGenericControl)userManual.FindControl("UserManualBadge");
                control.InnerText = list.Count.ToString();
            }
            (sender as UploadAttachment).ActiveDropZoneID = "active-drop-zone";
        }

        protected void UserManualUploadControl_UploadComplete(object sender, EventArgs e)
        {
            companySettingsRepo = new CompanySettingsRepository(XpoHelper.GetNewSession());
            model = companySettingsRepo.GetCompanySettings();
            if (model != null)
            {
                string pipe = "";
                if (!String.IsNullOrEmpty(model.PotNavodilaZaUporabo))
                    pipe = "|";

                if (!model.PotNavodilaZaUporabo.Contains((sender as UploadAttachment).currentFile.Name))
                {
                    companySettingsRepo = new CompanySettingsRepository(model.Session);
                    model.PotNavodilaZaUporabo += pipe + (sender as UploadAttachment).currentFile.Url + ";" + (sender as UploadAttachment).currentFile.Name;
                    companySettingsRepo.SaveCompanySettings(model);

                    HtmlGenericControl control = (HtmlGenericControl)userManual.FindControl("UserManualBadge");
                    control.InnerText = model.PotNavodilaZaUporabo.Split('|').Length.ToString();
                }
            }
        }

        protected void UserManualUploadControl_DeleteAttachments(object sender, EventArgs e)
        {
            companySettingsRepo = new CompanySettingsRepository(XpoHelper.GetNewSession());
            model = companySettingsRepo.GetCompanySettings();
            if (model != null)
            {
                int hasPipe = 0;
                string fileToDelete = (sender as UploadAttachment).currentFile.Name;
                DocumentEntity obj = GetAttachmentFromDB(fileToDelete);

                if (obj != null)
                {
                    string item = obj.Url + ";" + obj.Name;
                    string strPhysicalFolder = Server.MapPath(obj.Url);
                    if (File.Exists(strPhysicalFolder))
                        File.Delete(strPhysicalFolder);

                    if (model.PotNavodilaZaUporabo.Contains("|"))
                        hasPipe = 1;
                    else
                        hasPipe = 0;

                    model.PotNavodilaZaUporabo = model.PotNavodilaZaUporabo.Remove(model.PotNavodilaZaUporabo.IndexOf(item) <= 0 ? model.PotNavodilaZaUporabo.IndexOf(item) : model.PotNavodilaZaUporabo.IndexOf(item) - hasPipe, item.Length + hasPipe);
                    companySettingsRepo = new CompanySettingsRepository(model.Session);
                    companySettingsRepo.SaveCompanySettings(model);
                }

            }
        }

        protected void UserManualUploadControl_DownloadAttachments(object sender, EventArgs e)
        {
            companySettingsRepo = new CompanySettingsRepository(XpoHelper.GetNewSession());
            model = companySettingsRepo.GetCompanySettings();
            if (model != null)
            {
                string fileName = (sender as UploadAttachment).currentFile.Name;
                DocumentEntity obj = GetAttachmentFromDB(fileName);

                AddValueToSession(Enums.CommonSession.DownloadDocument, obj);
                //Response.Redirect(Request.RawUrl);
                ASPxWebControl.RedirectOnCallback(Request.RawUrl);
            }
        }

        private DocumentEntity GetAttachmentFromDB(string fileName)
        {
            if (model != null)
            {
                string[] split = model.PotNavodilaZaUporabo.Split('|');
                foreach (var item in split)
                {
                    string[] fileSplit = item.Split(';');
                    if (fileSplit[1].Equals(fileName))
                    {
                        return new DocumentEntity { Url = fileSplit[0], Name = fileSplit[1] };
                    }
                }
            }
            return null;
        }

        private void WriteDocumentToResponse(byte[] documentData, string format, bool isInline, string fileName)
        {
            string contentType = "application/pdf";

            if (format == "png")
                contentType = "image/png";
            else if (format == "jpg" || format == "jpeg")
                contentType = "image/jpeg";

            string disposition = (isInline) ? "inline" : "attachment";

            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
            Response.BinaryWrite(documentData);
            Response.End();
        }

        private List<DocumentEntity> GetDocumentsFromCompanySettings()
        {
            List<DocumentEntity> list = new List<DocumentEntity>();
            DocumentEntity document = null;
            string[] split = model.PotNavodilaZaUporabo.Split('|');
            string resultExtension = "";
            foreach (var item in split)
            {
                string[] fileData = item.Split(';');
                document = new DocumentEntity();
                document.Url = fileData[0];
                document.Name = fileData[1];

                resultExtension = Path.GetExtension(fileData[1]);
                if (resultExtension.Equals(".png") || resultExtension.Equals(".jpg") || resultExtension.Equals(".jpeg"))
                    document.isImage = true;

                list.Add(document);
            }

            return list;
        }
        #endregion
    }
}