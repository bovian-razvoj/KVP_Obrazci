using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KVP_Obrazci
{
    public partial class UploadAttachment : System.Web.UI.UserControl
    {
        public event EventHandler PopulateAttachments;
        public event EventHandler UploadComplete;
        public event EventHandler DeleteAttachments;
        public event EventHandler DownloadAttachments;

        const string UploadDirectory = "~/UploadControl/UploadDocuments/";
        public List<DocumentEntity> files { get; set; }
        public DocumentEntity currentFile { get; set; }

        public string ActiveDropZoneID { get; set; }
        public string ActiveDialogTriggerID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateAttachments(this, EventArgs.Empty);
                UploadControl.AdvancedModeSettings.ExternalDropZoneID = ActiveDropZoneID;
                //For opening file dialog
                UploadControl.DialogTriggerID = !String.IsNullOrEmpty(ActiveDialogTriggerID) ? ActiveDialogTriggerID : "externalDropZone";
                hfDropZone["DropZone"] = ActiveDropZoneID;
                if (files != null)
                {
                    foreach (var item in files)
                    {

                        documentContainer.InnerHtml += "<a class='uploaded-link' href='#' data-toggle='popover'" +
                            " data-placement='left' data-trigger='focus' title='Možnosti' data-content='" +
                            "<a class=\"documentOpen\" href=\"" + item.Url + "\" target=\"_blank\">Odpri</a>" +
                            "<div class=\"documentDelete\" data-document=\"" + item.Name + "\">Izbriši</div>" +
                            "<div class=\"documentDownload\" data-document=\"" + item.Name + "\">Prenesi</div>'>" +
                            "<div class='uploaded-doc" + (item.isImage ? " uploaded-image'" : "'") + "><span title=" + item.Name + ">" + item.Name + "</span></div></a>";
                    }

                }
            }
        }

        protected void UploadControl_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            bool isPic = false;
            if (resultExtension.Equals(".png") || resultExtension.Equals(".jpg") || resultExtension.Equals(".jpeg"))
                isPic = true;

            string path = Server.MapPath(UploadDirectory);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
            string resultFileUrl = UploadDirectory + resultFileName;
            string resultFilePath = MapPath(resultFileUrl);
            e.UploadedFile.SaveAs(resultFilePath);
            string url = ResolveClientUrl(resultFileUrl);
            currentFile = new DocumentEntity { Url = url, Name = resultFileName, isImage = isPic };
            e.CallbackData = JsonConvert.SerializeObject(currentFile);
            UploadComplete(this, EventArgs.Empty);
        }

        protected void CallbackUploadControl_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            string[] split = e.Parameter.Split(';');

            if (split[0] == "Delete")
            {
                currentFile = new DocumentEntity { Url = "", Name = split[1] };
                DeleteAttachments(this, EventArgs.Empty);
            }
            else if (split[0] == "Download")
            {
                currentFile = new DocumentEntity { Url = "", Name = split[1] };
                DownloadAttachments(this, EventArgs.Empty);
            }
        }
    }

    public class DocumentEntity
    {
        public string Url { get; set; }
        public string Name { get; set; }

        public bool isImage { get; set; }
    }
}