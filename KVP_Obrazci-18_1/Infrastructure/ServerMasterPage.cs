using DevExpress.Web;
using KVP_Obrazci.Common;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Helpers.DataProvder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace KVP_Obrazci.Infrastructure
{
    public class ServerMasterPage : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        #region Session handeling

        protected void HandlePreviousPageSessions()
        {
            string pageName = Path.GetFileName(Request.PhysicalPath);

            if (GetValueFromSession(Enums.CommonSession.PreviousPageName) != null && !GetValueFromSession(Enums.CommonSession.PreviousPageName).ToString().Equals(pageName))
            {
                if (!SessionHasValue(Enums.CommonSession.PreviousPageSessions)) return;

                List<string> sessions = GetValueFromSession(Enums.CommonSession.PreviousPageSessions).ToString().Split(';').ToList();
                foreach (var item in sessions)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        Session.Remove(item);
                    }
                }
                AddValueToSession(Enums.CommonSession.PreviousPageSessions, null);

            }
            AddValueToSession(Enums.CommonSession.PreviousPageName, pageName);
        }
        protected void SetPreviousSessions(string sessionName)
        {
            if (SessionHasValue(Enums.CommonSession.PreviousPageSessions))
            {
                string sessionValues = GetValueFromSession(Enums.CommonSession.PreviousPageSessions).ToString();
                sessionValues += sessionName + ";";
                AddValueToSession(Enums.CommonSession.PreviousPageSessions, sessionValues);
            }
            else
                AddValueToSession(Enums.CommonSession.PreviousPageSessions, sessionName + ";");
        }

        protected void AddValueToSession(object sesionName, object value)
        {
            Session[sesionName.ToString()] = value;
        }

        protected object GetValueFromSession(object sessionName)
        {
            return Session[sessionName.ToString()];
        }

        protected string GetStringValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return "";

            return Session[sessionName.ToString()].ToString();
        }

        protected int GetIntValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return -1;

            return CommonMethods.ParseInt(GetStringValueFromSession(sessionName));
        }

        protected decimal GetDecimalValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return -1;

            return CommonMethods.ParseDecimal(GetStringValueFromSession(sessionName));
        }

        protected double GetDoubleValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return -1.0;

            return CommonMethods.ParseDouble(GetStringValueFromSession(sessionName));
        }

        protected bool GetBoolValueFromSession(object sessionName)
        {
            if (Session[sessionName.ToString()] == null)
                return false;

            return CommonMethods.ParseBool(Session[sessionName.ToString()].ToString());
        }

        protected bool SessionHasValue(object sessionName)
        {
            if (Session[sessionName.ToString()] != null)
                return true;

            return false;
        }

        protected void RemoveAllSesions()
        {
            Session.RemoveAll();
        }

        protected void RemoveSession(object sessionName)
        {
            Session.Remove(sessionName.ToString());
        }

        protected void ClearAllSessions<T>(List<T> sessionList)
        {
            foreach (var item in sessionList)
            {
                RemoveSession(item.ToString());
            }
        }

        protected void ClearAllSessions<T>(List<T> sessionList, string redirectPageUrl, bool isCallback = false)
        {
            foreach (var item in sessionList)
            {
                RemoveSession(item.ToString());
            }

            if (isCallback)
                ASPxWebControl.RedirectOnCallback(redirectPageUrl);
            else
                Response.Redirect(redirectPageUrl);
        }
        #endregion

        #region Generating URI and redirection
        protected void RedirectWithCustomURI(string pageName, int userAction, object recordID)
        {
            Response.Redirect(GenerateURI(pageName, userAction, recordID));
        }

        protected void RedirectWithCustomURI(string pageName, List<QueryStrings> queryList)
        {
            Response.Redirect(GenerateURI(pageName, queryList));
        }

        /// <summary>
        /// Method using for generating uri based on user action(add, edit, delete) and which entity record we want to manipulate.
        /// </summary>
        /// <param name="pageName">Page name.</param>
        /// <param name="userAction">Enums user action (add, edit, delete).</param>
        /// <param name="recordID">Entity record we want to manipulate.</param>
        /// <returns>Returns url for redirection.</returns>
        protected string GenerateURI(string pageName, int userAction, object recordID)
        {
            return pageName + "?" + Enums.QueryStringName.action.ToString() + "=" + userAction.ToString() + "&" + Enums.QueryStringName.recordId.ToString() + "=" + recordID.ToString();
        }

        /// <summary>
        /// Method using for generating uri with custom attributes.
        /// </summary>
        /// <param name="pageName">Page name.</param>
        /// <param name="item">QuerString item which contains atttribute and value.</param>
        /// <returns>Return query string.</returns>
        protected string GenerateURI(string pageName, QueryStrings item)
        {
            string querystring = "";
            if (item != null)
                querystring = GetQueryStringBuilderInstance().AddQueryItem(item);
            return pageName + "?" + querystring;
        }
        /// <summary>
        /// Method using for generating uri with custom multiple attributes.
        /// </summary>
        /// <param name="pageName">Page name.</param>
        /// <param name="item">QuerString list which contains atttribute and value.</param>
        /// <returns>Return query string.</returns>
        protected string GenerateURI(string pageName, List<QueryStrings> queryList)
        {
            string querystring = "";
            if (queryList.Count > 0)
                querystring = GetQueryStringBuilderInstance().AddQueryList(queryList);
            return pageName + "?" + querystring;
        }

        /// <summary>
        /// If the user doesn't have rights for opening page than we redirect user to Home page
        /// </summary>
        protected void RedirectHome()
        {
            bool isCallback = CommonMethods.IsCallbackRequest(this.Request);
            if (isCallback)
                ASPxWebControl.RedirectOnCallback("~/Home.aspx?sessionExpired=true");
            else
                Response.Redirect("~/Home.aspx?sessionExpired=true");
        }
        protected void RedirectHome(int messagetype)
        {
            bool isCallback = CommonMethods.IsCallbackRequest(this.Request);
            if (isCallback)
                ASPxWebControl.RedirectOnCallback("~/Home.aspx?messageType=" + messagetype);
            else
                Response.Redirect("~/Home.aspx?messageType=" + messagetype);
        }
        #endregion

        #region Instance Extractor

        protected QueryStringBuilder GetQueryStringBuilderInstance()
        {
            QueryStringBuilder queryStringBuilder = null;

            if (queryStringBuilder == null)
                return new QueryStringBuilder();

            return queryStringBuilder;
        }

        protected KVPDocumentProvider GetKVPDocumentProvider()
        {
            KVPDocumentProvider kVPDocument = null;

            if (kVPDocument == null)
                return new KVPDocumentProvider();

            return kVPDocument;
        }

        protected KVPGroupsProvider GetKVPGroupsProvider()
        {
            KVPGroupsProvider kVPGroup = null;

            if (kVPGroup == null)
                return new KVPGroupsProvider();

            return kVPGroup;
        }

        protected EmployeeProvider GetEmployeeProvider()
        {
            EmployeeProvider employee = null;

            if (employee == null)
                return new EmployeeProvider();

            return employee;
        }

        protected PostProvider GetPostProvider()
        {
            PostProvider post = null;

            if (post == null)
                return new PostProvider();

            return post;
        }
        #endregion

        #region User Action Buttons Handeling
        protected void UserActionConfirmBtnUpdate(ASPxButton button, int userAction, bool popUpBtn = false)
        {
            if (userAction == (int)Enums.UserAction.Delete)
            {
                button.ImageUrl = popUpBtn ? "~/Images/trashPopUp.png" : "~/Images/trash.png";
                button.Image.UrlHottracked = popUpBtn ? "" : "~/Images/trashHover.png";
                button.Text = "Izbriši";
            }
            else if (userAction == (int)Enums.UserAction.Add)
            {
                button.ImageUrl = popUpBtn ? "~/Images/addPopUp.png" : "~/Images/add.png";
                button.Image.UrlHottracked = popUpBtn ? "" : "~/Images/addHover.png";
                button.Text = "Shrani";
            }
            else
            {
                button.ImageUrl = popUpBtn ? "~/Images/editPopup.png" : "~/Images/edit.png";
                button.Image.UrlHottracked = popUpBtn ? "" : "~/Images/editHover.png";
                button.Text = "Shrani";
            }
        }

        protected void EnabledDeleteAndEditBtnPopUp(ASPxButton buttonEdit, ASPxButton buttonDelete, bool disable = true)
        {
            if (disable)
            {
                buttonEdit.ImageUrl = "~/Images/btnPopUpEditDisabled.png";
                buttonEdit.Text = "Spremeni";
                buttonEdit.ClientEnabled = false;

                buttonDelete.ImageUrl = "~/Images/btnPopUpDeleteDisabled.png";
                buttonDelete.Text = "Izbrisi";
                buttonDelete.ClientEnabled = false;
            }
            else
            {
                buttonEdit.ImageUrl = "~/Images/editForPopup.png";
                buttonEdit.Text = "Spremeni";
                buttonEdit.ClientEnabled = true;

                buttonDelete.ImageUrl = "~/Images/trashForPopUp.png";
                buttonDelete.Text = "Izbrisi";
                buttonDelete.ClientEnabled = true;
            }
        }
        protected void EnabledAddBtnPopUp(ASPxButton buttonAdd, bool disable = true)
        {
            if (disable)
            {
                buttonAdd.ImageUrl = "~/Images/addPopupDisabled.png";
                buttonAdd.Text = "Spremeni";
                buttonAdd.ClientEnabled = false;
            }
            else
            {
                buttonAdd.ImageUrl = "~/Images/addPopUp.png";
                buttonAdd.Text = "Spremeni";
                buttonAdd.ClientEnabled = true;
            }
        }
        #endregion

        #region Client POP UP handeling
        protected void ShowClientPopUp(string message, int popUpWindow = 0)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CommonJS", String.Format("ShowErrorPopUp('{0}', '{1}');", message, popUpWindow), true);
        }

        /// <summary>
        /// We are using this popup if the session showWarning contains value
        /// </summary>
        /// <param name="message">Message that will show on popup.</param>
        protected void ShowWarningPopUp(int popUpWindow = 0)
        {
            if (SessionHasValue(Enums.CommonSession.ShowWarning))
            {
                if (GetBoolValueFromSession(Enums.CommonSession.ShowWarning))
                    ShowClientPopUp(GetStringValueFromSession(Enums.CommonSession.ShowWarningMessage));

                RemoveSession(Enums.CommonSession.ShowWarning);
                RemoveSession(Enums.CommonSession.ShowWarningMessage);
            }
        }
        #endregion

        protected object GetGridLookupValue(ASPxGridLookup lookup)
        {
            try
            {
                return lookup.Value;
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);
                return null;
            }
        }

        protected void ASPxGridLookupLoad_WidthMedium(object sender, EventArgs e)
        {
            (sender as ASPxGridLookup).GridView.Width = new Unit(500, UnitType.Pixel);
        }
        protected void ASPxGridLookupLoad_WidthLarge(object sender, EventArgs e)
        {
            (sender as ASPxGridLookup).GridView.Width = new Unit(700, UnitType.Pixel);
        }
        protected void ASPxGridLookupLoad_WidthSmall(object sender, EventArgs e)
        {
            (sender as ASPxGridLookup).GridView.Width = new Unit(300, UnitType.Pixel);
        }

        protected DataTable SerializeToDataTable<T>(List<T> list, string keyFieldName = "", string visibleColumn = "")
        {
            DataTable dt = new DataTable();
            string json = JsonConvert.SerializeObject(list);
            dt = JsonConvert.DeserializeObject<DataTable>(json);

            if (keyFieldName != "" && visibleColumn != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.NewRow();
                row[keyFieldName] = -1;
                row[visibleColumn] = "Izberi... ";
                dt.Rows.InsertAt(row, 0);
            }

            return dt;
        }

        protected void SetFocusedRowInGridView(ASPxGridView grid, object sessionName = null)
        {
            int index = 0;
            if (sessionName != null)
            {
                if (SessionHasValue(sessionName))
                {
                    index = grid.FindVisibleIndexByKeyValue(GetIntValueFromSession(sessionName));
                    RemoveSession(sessionName);
                }
            }
            grid.Settings.GridLines = GridLines.Both;
            grid.FocusedRowIndex = index;
            grid.ScrollToVisibleIndexOnClient = index;
        }

        public bool SetSessionsAndOpenPopUp(string eventParameter, object sessionToWrite, object entityID)
        {
            int callbackResult = 0;
            int.TryParse(eventParameter, out callbackResult);
            if (callbackResult > 0 && callbackResult <= 3)
            {
                if (callbackResult != (int)Enums.UserAction.Add && entityID == null) return false;

                switch (callbackResult)
                {
                    case (int)Enums.UserAction.Add:
                        AddValueToSession(Enums.CommonSession.UserActionPopUp, callbackResult);
                        AddValueToSession(sessionToWrite, 0);
                        break;

                    default://For editing and deleting is the same code.
                        AddValueToSession(Enums.CommonSession.UserActionPopUp.ToString(), callbackResult);
                        AddValueToSession(sessionToWrite, entityID);
                        break;

                }
                return true;
            }

            return false;
        }

        public void TabsVisible(List<string> tabs, bool show = false, string activeTab = "")
        {
            string json = JsonConvert.SerializeObject(tabs);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CommonJS", String.Format("ConfigureTabs('{0}', '{1}', '{2}');", json, show, activeTab), true);
        }

        /*protected T CheckModelValidation<T>(WebResponseContentModel<T> instance)
        {
            object obj = default(T);

            if (!instance.IsRequestSuccesful)
            {
                string requestFailedError = "";

                if (!String.IsNullOrEmpty(instance.ValidationError))
                {
                    instance.ValidationError = instance.ValidationError.Replace("'", "");
                    //instance.ValidationError = instance.ValidationError.Insert(0, "'");
                    //instance.ValidationError += "'";
                    instance.ValidationError = instance.ValidationError.Replace("\\", "\\\\");
                    instance.ValidationError = instance.ValidationError.Replace("\r\n", "");
                    requestFailedError = instance.ValidationError;
                }
                else if (!String.IsNullOrEmpty(instance.ValidationErrorAppSide))
                    requestFailedError = instance.ValidationErrorAppSide.Replace("\r\n", "");

                ShowClientPopUp(requestFailedError);

                return (T)obj;
            }
            else
            {
                obj = instance.Content;
            }

            return (T)obj;
        }*/
    }
}