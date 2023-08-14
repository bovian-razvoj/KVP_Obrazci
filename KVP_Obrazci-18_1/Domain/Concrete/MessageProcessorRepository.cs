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

            this.session = session;
        }

        public List<KVPEmployee> GetEmployeesToSendMail()
        {
            try
            {
                XPQuery<KVPDocument> kvDocument = session.Query<KVPDocument>();
                XPQuery<KVPPresoje> auditors = session.Query<KVPPresoje>();
                List<KVPEmployee> returnList = new List<KVPEmployee>();
                var group1 = from c in kvDocument
                             where c.LastStatusId.idStatus == 2
                             group c by c.vodja_teama into kvp
                             select new KVPEmployee
                             {
                                 employeeID = kvp.Key,
                                 KVPToConfirm = kvp.Count()
                             };
                returnList = ConcatenateList(returnList, group1.ToList());

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

                var group3 = from a in auditors
                             where a.idKVPDocument.LastStatusId.idStatus == 8
                             group a by a.idKVPDocument into gKvp
                             select gKvp;

                var presoje = group3.ToList();

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

                return returnList;
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
                }
                else
                {
                    KVPEmployee model = new KVPEmployee();
                    model.employee = employee;
                    model.employeeID = item.employeeID;
                    model.KVPToAudit = item.KVPToAudit;
                    model.KVPToConfirm = item.KVPToConfirm;
                    model.KVPToRealize = item.KVPToRealize;
                    listTo.Add(model);
                }
            }

            return listTo;
        }

        public void ProceesKVPsToSendEmployeeStatistic()
        {
            try
            {
                List<KVPEmployee> list = GetEmployeesToSendMail();
                string serverTag = ConfigurationManager.AppSettings["ServerTag"].ToString();
                string templatePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["MESSAGE"].ToString()).Replace("\"", "\\");
                StreamReader reader = new StreamReader(templatePath);

                string templateString = reader.ReadToEnd();
                string generatedTemplate = "";
                foreach (KVPEmployee item in list)
                {
                    if (item.employee != null)
                    {
                        List<string> emails = GetEmployeeEmails(item.employee);
                        foreach (var obj in emails)
                        {
                            item.FirstName = item.employee.Firstname;
                            item.Lastname = item.employee.Lastname;
                            item.Email = obj;
                            item.ServerTag = serverTag;

                            generatedTemplate = ReplaceDefaultValuesInTemplate(item, templateString);
                            SaveToSystemEmailMessage(item.employee.Email, generatedTemplate, null, "KVP Sistem");
                        }
                    }
                    else
                        CommonMethods.LogThis("Zaposlen, Id " + item.employeeID + " ne obstaja v podatkovni bazi!");
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

        public void ProcessInfoKVPMailToSend(int recieverUserID, InfoMailModel model, Session currentSession = null)
        {
            try
            {
                string serverTag = ConfigurationManager.AppSettings["ServerTag"].ToString();
                string templatePath = (AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["INFO_MAIL_KVP_MESSAGE"].ToString()).Replace("\"", "\\");
                StreamReader reader = new StreamReader(templatePath);

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
                    value = value.Replace("$%" + myFields[i].Name + "%$", myFields[i].GetValue(o, null) == null ? "" : myFields[i].GetValue(o, null).ToString());
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
    }
}