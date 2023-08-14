using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class MessageProcessorRepository : IMessageProcessorRepository
    {
        Session session;

        public MessageProcessorRepository(Session session = null)
        {

            if (session == null)
                session = XpoHelper.GetNewSession();

            //System.Threading.Thread thread1 = new System.Threading.Thread(() => { session.UpdateSchema(Assembly.GetExecutingAssembly()); });

            this.session = session;
        }

        public List<KVPEmployee> GetEmployeesToSendMail(Session session = null)
        {
            try
            {
                //if (session == null)
                //    session = XpoHelper.GetNewSession();

                CommonMethods.LogThis(session.ToString());
                CommonMethods.LogThis("GetEmployeesToSendMail - 01");
                XPQuery<KVPDocument> kvDocument = session.Query<KVPDocument>();
                XPQuery<KVPSkupina_Users> kvpGroupUsers = session.Query<KVPSkupina_Users>();
                XPQuery<KVPPresoje> auditors = session.Query<KVPPresoje>();
                XPQuery<KVPDocument> RKRealizator = session.Query<KVPDocument>();
                XPQuery<KVPDocument> RKToConfirm = session.Query<KVPDocument>();
                List<KVPEmployee> returnList = new List<KVPEmployee>();

                //CommonMethods.LogThis("GetEmployeesToSendMail - 02");
                var champions = kvpGroupUsers.Where(kgu => kgu.Champion).ToList();

                foreach (var item in champions)
                {
                    int idKVPSkupine = item.idKVPSkupina.idKVPSkupina;

                    int kvptoComplete = kvDocument.Where(k => kvpGroupUsers.Any(ku => (ku.IdUser.Id == k.Predlagatelj.Id && ku.idKVPSkupina.idKVPSkupina == idKVPSkupine) ||
                    kvpGroupUsers.Any(kgu => kgu.IdUser.Id == k.Realizator.Id && kgu.idKVPSkupina.idKVPSkupina == idKVPSkupine)) &&
                    k.LastStatusId.idStatus == 5).Count();

                    returnList.Add(new KVPEmployee { employeeID = item.IdUser.Id, isUserChampion = true, KVPToComplete = kvptoComplete, employee = item.IdUser });
                }
                //CommonMethods.LogThis("GetEmployeesToSendMail - 03");

                var group1 = from c in kvDocument
                             where c.LastStatusId.idStatus == 2
                             group c by c.vodja_teama into kvp
                             select new KVPEmployee
                             {
                                 employeeID = kvp.Key,
                                 KVPToConfirm = kvp.Count()
                             };
                returnList = ConcatenateList(returnList, group1.ToList());
                //CommonMethods.LogThis("GetEmployeesToSendMail - 04");
                var group2 = from c in kvDocument
                             where c.LastStatusId.idStatus == 4
                             group c by c.Realizator into kvp
                             select new KVPEmployee
                             {
                                 employee = kvp.Key,
                                 employeeID = kvp.Key.Id,
                                 KVPToRealize = kvp.Count()
                             };
                returnList = ConcatenateList(returnList, group2.ToList());
                //CommonMethods.LogThis("GetEmployeesToSendMail - 05");
                var group3 = from a in auditors
                             where a.idKVPDocument.LastStatusId.idStatus == 8
                             group a by a.idKVPDocument into gKvp
                             select gKvp;

                var presoje = group3.ToList();
                //CommonMethods.LogThis("GetEmployeesToSendMail - 06");
                foreach (var item in presoje)
                {
                    KVPPresoje obj = item.OrderByDescending(p => p.ts).FirstOrDefault();
                    if (obj != null)
                    {
                        KVPEmployee kvpE = returnList.Where(a => a.employeeID == obj.Presojevalec.Id).FirstOrDefault();
                        if (kvpE != null)
                        {
                            kvpE.KVPToAudit += 1;
                        }
                        else
                        {
                            KVPEmployee kvpEmp = new KVPEmployee();
                            kvpEmp.employee = obj.Presojevalec;
                            kvpEmp.employeeID = obj.Presojevalec.Id;
                            kvpEmp.KVPToAudit = 1;
                            kvpEmp.KVPToConfirm = 0;
                            kvpEmp.KVPToRealize = 0;
                            returnList.Add(kvpEmp);
                        }
                    }
                }
                //CommonMethods.LogThis("GetEmployeesToSendMail - 07");
                var groupRKRealizator = from a in RKRealizator
                                        where a.LastStatusId.idStatus == 16
                                        group a by a.Realizator into gRKRealizator
                                        select new KVPEmployee
                                        {
                                            employee = gRKRealizator.Key,
                                            employeeID = gRKRealizator.Key.Id,
                                            RKToRealize = gRKRealizator.Count()
                                        };
                //CommonMethods.LogThis("GetEmployeesToSendMail - 08");
                returnList = ConcatenateList(returnList, groupRKRealizator.ToList());
                //CommonMethods.LogThis("GetEmployeesToSendMail - 09");

                var rKToConfirmCount = from c in RKToConfirm
                                       where c.LastStatusId.idStatus == 10
                                       select c;
                //CommonMethods.LogThis("GetEmployeesToSendMail - 1");
                int count = rKToConfirmCount.Count();
                CommonMethods.LogThis("rKToConfirmCount.Count: " + count.ToString());
                CommonMethods.LogThis("returnList: " + returnList.ToString());
                //CommonMethods.LogThis("GetEmployeesToSendMail - 2");
                
                List<KVPEmployee> retList2 = returnList.Where(rk => rk.employee.RoleID.VlogaID == 6 ||  (rk.employee.SecondRoleID != null && rk.employee.SecondRoleID.VlogaID == 6) ).ToList();
                //CommonMethods.LogThis("GetEmployeesToSendMail - 02");
                if (retList2 != null)
                {
                    returnList.Where(rk => rk.employee.RoleID.VlogaID == 6 || rk.employee.SecondRoleID.VlogaID == 6).ToList().ForEach(x => x.RKToConfirm = count);
                }
                //CommonMethods.LogThis("GetEmployeesToSendMail - 3");
                return returnList;
            }
            catch (Exception ex)
            {
                //CommonMethods.LogThis("GetEmployeesToSendMail - 4");
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
                return null;
            }
        }

        private List<KVPEmployee> ConcatenateList(List<KVPEmployee> listTo, List<KVPEmployee> listFrom)
        {
            XPQuery<Users> employees = session.Query<Users>();
            foreach (var item in listFrom)
            {
                Users employee = employees.Where(e => e.Id == item.employeeID).FirstOrDefault();
                KVPEmployee obj = listTo.Where(lt => lt.employeeID == item.employeeID).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.employee == null) obj.employee = employee;
                    obj.KVPToAudit += item.KVPToAudit;
                    obj.KVPToConfirm += item.KVPToConfirm;
                    obj.KVPToRealize += item.KVPToRealize;
                    obj.RKToRealize += item.RKToRealize;
                }
                else
                {
                    KVPEmployee model = new KVPEmployee();
                    model.employee = employee;
                    model.employeeID = item.employeeID;
                    model.KVPToAudit = item.KVPToAudit;
                    model.KVPToConfirm = item.KVPToConfirm;
                    model.KVPToRealize = item.KVPToRealize;
                    model.RKToRealize = item.RKToRealize;
                    listTo.Add(model);
                }
            }

            return listTo;
        }

        public void ProceesKVPsToSendEmployeeStatistic(Session session = null)
        {
            try
            {
                if (session == null)
                    session = XpoHelper.GetNewSession();

                CommonMethods.LogThis("Start GetEmployeesToSendMail");
                List<KVPEmployee> list = GetEmployeesToSendMail(session);
                CommonMethods.LogThis("End GetEmployeesToSendMail");
                string serverTag = ConfigurationManager.AppSettings["ServerTag"].ToString();
                string templatePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["MESSAGE"].ToString()).Replace("\"", "\\");
                StreamReader reader = new StreamReader(templatePath);
                CommonMethods.LogThis("GetEmployeesToSendMail: " + templatePath);
                string templateString = reader.ReadToEnd();
                //CommonMethods.LogThis("ProceesKVPsToSendEmployeeStatistic - 1");
                string generatedTemplate = "";
                if (list != null)
                {
                    foreach (KVPEmployee item in list)
                    {
                        //CommonMethods.LogThis("ProceesKVPsToSendEmployeeStatistic - 2");
                        if (item.employee != null)
                        {
                            List<string> emails = GetEmployeeEmails(item.employee);
                            foreach (var obj in emails)
                            {
                                //CommonMethods.LogThis("Name : " + item.employee.Firstname + ", " + item.employee.Lastname);
                                item.FirstName = item.employee.Firstname;
                                item.Lastname = item.employee.Lastname;
                                item.Email = obj;
                                item.ServerTag = serverTag;
                                item.ServerTagLogin = serverTag + "/Home.aspx";
                                //item.DisplayListItemStyle = item.isUserChampion ? "display: block;" : "display: none;";
                                item.DisplayChampionItem = item.isUserChampion ? "<li>KVP za zaključitev: $%KVPToComplete%$</li>" : "";
                                item.DisplayRKItems = item.RKToRealize > 0 ? "<li>RK za realizacijo: " + item.RKToRealize + "</li>" : "";
                                //CommonMethods.LogThis("ProceesKVPsToSendEmployeeStatistic - 3");
                                item.DisplayRKItemsToConfirm = item.RKToConfirm > 0 ? "<li>RK za potrditev: " + item.RKToConfirm + "</li>" : "";
                                //CommonMethods.LogThis("ProceesKVPsToSendEmployeeStatistic - 4");

                                generatedTemplate = ReplaceDefaultValuesInTemplate(item, templateString);
                                SaveToSystemEmailMessage(item.employee.Email, generatedTemplate, null, "KVP Sistem");
                                //CommonMethods.LogThis("ProceesKVPsToSendEmployeeStatistic - 5");
                            }
                        }
                        else
                            CommonMethods.LogThis("Zaposlen, Id " + item.employeeID + " ne obstaja v podatkovni bazi!");
                    }
                }
                //CommonMethods.LogThis("ProceesKVPsToSendEmployeeStatistic - 6");

            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
                throw new Exception(errorToThrow);
            }
        }

        public void ProcessRejectedKVPToSend(int informedEmployeeID, int kvpDocID, Session currentSession = null)
        {
            try
            {
                string serverTag = ConfigurationManager.AppSettings["ServerTag"].ToString();
                string templatePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["REJECTED_KVP_MESSAGE"].ToString()).Replace("\"", "\\");
                StreamReader reader = new StreamReader(templatePath);

                string templateString = reader.ReadToEnd();
                string generatedTemplate = "";
                XPQuery<KVPDocument> kvDocument = session.Query<KVPDocument>();
                XPQuery<Users> emp = session.Query<Users>();

                if (currentSession != null)
                    kvDocument = currentSession.Query<KVPDocument>();

                KVPDocument kvp = kvDocument.Where(k => k.idKVPDocument == kvpDocID).FirstOrDefault();
                Users employee = emp.Where(e => e.Id == informedEmployeeID).FirstOrDefault();

                if (employee != null && kvp != null)
                {
                    List<string> employeeEmails = GetEmployeeEmails(employee);

                    foreach (var item in employeeEmails)
                    {
                        KVPEmployee obj = new KVPEmployee()
                        {
                            FirstName = employee.Firstname,
                            Lastname = employee.Lastname,
                            ServerTag = serverTag,
                            Email = item,
                            Arguments = kvp.ZavrnitevOpis,
                            StevilkaKVP = kvp.StevilkaKVP,
                            KVPRejectFirstName = kvp.ZavrnitevIdUser.Firstname,
                            KVPRejectLastname = kvp.ZavrnitevIdUser.Lastname
                        };

                        generatedTemplate = ReplaceDefaultValuesInTemplate(obj, templateString);
                        SaveToSystemEmailMessage(item, generatedTemplate, null, "Zavrnitev KVP predloga");
                    }
                }

            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
                throw new Exception(errorToThrow);
            }
        }

        public void ProcessNewKVPCredentialsToSend(int kvpUserID, Session currentSession = null)
        {
            try
            {
                string serverTag = ConfigurationManager.AppSettings["ServerTag"].ToString();
                string templatePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["CREDENTIALS_KVP_MESSAGE"].ToString()).Replace("\"", "\\");
                StreamReader reader = new StreamReader(templatePath);

                string templateString = reader.ReadToEnd();
                string generatedTemplate = "";

                XPQuery<Users> emp = session.Query<Users>();

                if (currentSession != null)
                    emp = currentSession.Query<Users>();


                Users employee = emp.Where(e => e.Id == kvpUserID).FirstOrDefault();

                if (employee != null)
                {
                    List<string> employeeEmails = GetEmployeeEmails(employee);

                    foreach (var item in employeeEmails)
                    {
                        KVPEmployee obj = new KVPEmployee()
                        {
                            FirstName = employee.Firstname,
                            Lastname = employee.Lastname,
                            ServerTag = serverTag,
                            Email = item,
                            Username = employee.Username,
                            Password = employee.Password,
                            ChangePassEmployeeUrl = serverTag + "/Credentials/ChangePassword.aspx?" + Enums.QueryStringName.recordId.ToString() + "=" + CommonMethods.Base64Encode(employee.Id.ToString())
                        };

                        generatedTemplate = ReplaceDefaultValuesInTemplate(obj, templateString);
                        SaveToSystemEmailMessage(item, generatedTemplate, null, "Dodelitev dostopa v KVP sistem");
                    }
                }

            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
                throw new Exception(errorToThrow);
            }
        }

        public void ProcessInfoKVPMailToSend(int recieverUserID, InfoMailModel model, Session currentSession = null, bool SendEmailToEmployee = false)
        {
            try
            {
                string serverTag = ConfigurationManager.AppSettings["ServerTag"].ToString();
                string serverTagDoc = "";
                string templatePath = "";
                templatePath = SendEmailToEmployee ? (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["INFO_MAIL_KVP_MESSAGE_USER"].ToString()).Replace("\"", "\\") : (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["INFO_MAIL_KVP_MESSAGE"].ToString()).Replace("\"", "\\");
                StreamReader reader = new StreamReader(templatePath);
                if (model.DocumentID != 0) serverTagDoc = serverTag + "/KVPDocuments/KVPDocumentForm.aspx?action=2&recordId=" + model.DocumentID;

                string templateString = reader.ReadToEnd();
                string generatedTemplate = "";

                XPQuery<Users> emp = session.Query<Users>();

                if (currentSession != null)
                    emp = currentSession.Query<Users>();


                Users employee = emp.Where(e => e.Id == recieverUserID).FirstOrDefault();

                if (employee != null)
                {
                    List<string> employeeEmails = GetEmployeeEmails(employee);

                    foreach (var item in employeeEmails)
                    {
                        model.Email = item;
                        model.FirstName = employee.Firstname;
                        model.Lastname = employee.Lastname;
                        model.ServerTag = serverTag;
                        model.ServerTagDoc = serverTagDoc;
                        generatedTemplate = ReplaceDefaultValuesInTemplate(model, templateString);
                        SaveToSystemEmailMessage(item, generatedTemplate, null, "eKVP sporočilo");
                    }
                }

            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
                throw new Exception(errorToThrow);
            }
        }

        public int ProcessChangedUserNameToSend(int recieverUserID, InfoMailModel model, Session currentSession = null, bool SendEmailToEmployee = false)
        {
            try
            {
                string serverTag = ConfigurationManager.AppSettings["ServerTag"].ToString();
                string serverTagDoc = "";
                string templatePath = "";
                templatePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["CHANGED_USERNAME_MAIL"].ToString().Replace("\"", "\\");
                StreamReader reader = new StreamReader(templatePath);
                if (model.DocumentID != 0) serverTagDoc = serverTag;

                string templateString = reader.ReadToEnd();
                string generatedTemplate = "";

                XPQuery<Users> emp = session.Query<Users>();

                if (currentSession != null)
                    emp = currentSession.Query<Users>();


                Users employee = emp.Where(e => e.Id == recieverUserID).FirstOrDefault();

                if (employee != null)
                {
                    List<string> employeeEmails = GetEmployeeEmails(employee);

                    foreach (var item in employeeEmails)
                    {
                        model.Email = item;
                        model.ServerTag = serverTag;
                        model.ServerTagDoc = serverTagDoc;
                        generatedTemplate = ReplaceDefaultValuesInTemplate(model, templateString);
                        SaveToSystemEmailMessage(item, generatedTemplate, null, "eKVP sporočilo");
                        return 1;
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
                throw new Exception(errorToThrow);
            }
        }

        public void ProcessSecurityInfoRedCardMailToSend(KVPDocument redCardModel, Session currentSession = null)
        {
            try
            {
                string serverTag = ConfigurationManager.AppSettings["ServerTag"].ToString();
                string templatePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["RED_CARD_SECURITY_MAIL"].ToString()).Replace("\"", "\\");
                StreamReader reader = new StreamReader(templatePath);

                string templateString = reader.ReadToEnd();
                string generatedTemplate = "";

                XPQuery<Users> emp = session.Query<Users>();

                if (currentSession != null)
                    emp = currentSession.Query<Users>();

                InfoMailModel model = new InfoMailModel();

                //dodamo varnostne inženirje
                List<Users> employees = emp.Where(e => e.PozarniReferent).ToList();
                //dodamo vodjo predlagatelja
                employees.Add(emp.Where(e => e.Id == redCardModel.vodja_teama).FirstOrDefault());
                //dodamo vse tpm administratorje
                string tpmAdminCode = Enums.UserRole.TpmAdmin.ToString();
                employees.AddRange(emp.Where(e => e.RoleID.Koda.Equals(tpmAdminCode) || e.SecondRoleID.Koda.Equals(tpmAdminCode)).ToList());

                foreach (var obj in employees)
                {
                    if (redCardModel != null)
                    {
                        List<string> employeeEmails = GetEmployeeEmails(obj);

                        foreach (var item in employeeEmails)
                        {
                            model.Email = item;
                            model.FirstName = obj.Firstname;
                            model.Lastname = obj.Lastname;
                            model.ServerTag = serverTag;
                            model.SenderFirstName = redCardModel.Predlagatelj.Firstname;
                            model.SenderLastname = redCardModel.Predlagatelj.Lastname;
                            model.Notes = redCardModel.OpisNapakeRK;
                            model.StevilkaKVP = redCardModel.StevilkaKVP;

                            generatedTemplate = ReplaceDefaultValuesInTemplate(model, templateString);
                            SaveToSystemEmailMessage(item, generatedTemplate, null, "eKVP Rdeči karton - varnost");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_28, error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
                throw new Exception(errorToThrow);
            }
        }

        private string ReplaceDefaultValuesInTemplate(Object o, string template)
        {
            string result = "";
            string value = template;
            Type type = o.GetType();
            object[] indexArgs = { 0 };

            PropertyInfo[] myFields = type.GetProperties(BindingFlags.Public
                | BindingFlags.Instance);

            for (int i = 0; i < myFields.Length; i++)
            {
                try
                {
                    string sRepVal = myFields[i].GetValue(o, null) == null ? "" : myFields[i].GetValue(o, null).ToString();
                    sRepVal = sRepVal.Replace("\n", "<br>");
                    value = value.Replace("$%" + myFields[i].Name + "%$", sRepVal);
                }
                catch (Exception ex)
                {
                    CommonMethods.LogThis(ex.Message);
                }
            }

            result = value;
            return result;
        }

        private void SaveToSystemEmailMessage(string emailTo, string bodyMessage, int? userID, string emailSubject = "Novo sporočilo")
        {
            try
            {
                SystemEmailMessage emailConstruct = null;

                emailConstruct = new SystemEmailMessage(session);
                //DataTypesHelper.LogThis("*****in for loop SaveToSystemEmailMessage*****");
                emailConstruct.EmailFrom = ConfigurationManager.AppSettings["EmailFromForDB"].ToString();
                emailConstruct.EmailSubject = emailSubject;
                emailConstruct.EmailBody = bodyMessage;
                emailConstruct.Status = (int)Enums.SystemServiceSatus.UnProcessed;
                emailConstruct.ts = DateTime.Now;
                emailConstruct.tsIDOsebe = userID.HasValue ? userID.Value : 0;
                emailConstruct.EmailTo = emailTo;

                emailConstruct.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Method SaveToSystemEmailMessage ERROR: " + ex);
            }
        }

        private List<string> GetEmployeeEmails(Users employee)
        {
            List<string> employeeEmails = new List<string>();

            if (!employee.UpravicenDoEPoste)
            {
                CommonMethods.LogThis("Zaposlen, " + employee.Firstname + " " + employee.Lastname + " ni upravičen do pošiljanje elektronsek pošte!");
                return employeeEmails;
            }
            if (String.IsNullOrEmpty(employee.Email))
            {
                CommonMethods.LogThis("Zaposlen, " + employee.Firstname + " " + employee.Lastname + " nima vpisanega elektronskega naslova. Vpiši ga!");
                return employeeEmails;
            }

            employeeEmails = employee.Email.Split(',').ToList();

            if (employeeEmails.Count <= 0)
                CommonMethods.LogThis("Zaposlen, " + employee.Firstname + " " + employee.Lastname + " nima vpisanega elektronskega naslova. Vpiši ga!");

            return employeeEmails;
        }

        public void DisposeSession()
        {
            if (session != null)
                session.Dispose();
        }
    }
}