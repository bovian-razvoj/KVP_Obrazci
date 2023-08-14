using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace KVP_Obrazci.Common
{
    public static class CommonMethods
    {
        public static int ParseInt(object param)
        {
            int num = 0;

            if (param != null)
            {
                int.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static Nullable<int> ParseNullableInt(object param)
        {
            int num = 0;

            if (param != null)
            {
                int.TryParse(param.ToString(), out num);

                if (num < 0)
                    return null;

                return num;
            }
            else
                return null;
        }

        public static decimal ParseDecimal(object param)
        {
            decimal num = 0;
            if (param != null)
            {
                decimal.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static double ParseDouble(object param)
        {
            double num = 0;
            if (param != null)
            {
                double.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static bool ParseBool(object param)
        {
            bool value = false;

            if (param != null)
                bool.TryParse(param.ToString(), out value);
           
            return value;
        }

        public static long ParseLong(object param)
        {
            long num = 0;
            if (param != null)
            {
                long.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static string PreveriZaSumnike(string _crka)
        {
            char crkaC = ' ';
            string novS = "";

            _crka = _crka.ToUpper();

            foreach (char item in _crka)
            {
                switch (item)
                {
                    case 'Č':
                        crkaC = 'C';
                        break;
                    case 'Š':
                        crkaC = 'S';
                        break;
                    case 'Ž':
                        crkaC = 'Z';
                        break;
                    case 'Đ':
                        crkaC = 'D';
                        break;
                    default:
                        crkaC = item;
                        break;
                }

                novS += crkaC.ToString();
            }

            return novS;
        }

        public static string ReplaceSumniki(string sName)
        {
            sName = sName.Replace("č", "c");
            sName = sName.Replace("š", "s");
            sName = sName.Replace("ć", "c");
            sName = sName.Replace("ž", "z");
            sName = sName.Replace("đ", "dz");
            sName = sName.Replace("ö", "o");

            return sName;
        }

        public static string Trim(string sTrim)
        {
            return String.IsNullOrEmpty(sTrim) ? "" : sTrim.Trim();
        }

        public static void LogThis(string message)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            File.AppendAllText(directory + "log.txt", DateTime.Now + " " + message + Environment.NewLine);
        }

        public static bool SendEmailToDeveloper(string displayName, string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;//Port 465 (SSL required)
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("bovianplus@gmail.com", "Geslo123.");
                client.Timeout = 6000;

                MailMessage message;

                message = new MailMessage();
                message.To.Add(new MailAddress("martin@bovianplus.si"));
                message.To.Add(new MailAddress("boris.dolinsek@bovianplus.si"));

                message.Sender = new MailAddress("bovianplus@gmail.com");
                message.From = new MailAddress("bovianplus@gmail.com", displayName);
                message.Subject = subject;
                message.IsBodyHtml = false;
                message.Body = body;
                message.BodyEncoding = Encoding.UTF8;

                client.Send(message);

            }
            catch (SmtpFailedRecipientsException ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }
            catch (SmtpException ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }

            return true;
        }

        public static void getError(Exception e, ref string errors)
        {
            if (e.GetType() != typeof(HttpException)) errors += " -------- " + e.ToString();
            if (e.InnerException != null) getError(e.InnerException, ref errors);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethodName()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public static string ConcatenateErrorIN_DB(string resource, string error, string methodName)
        {
            return resource + " in method : " + methodName + " Error : " + error;
        }

        public static string ConvertEmployeeTokenToJantarCode(string token)
        {
            long parsedToken = ParseLong(token);
            string binaryString = Convert.ToString(parsedToken, 2);
            string cutBinaryString = binaryString.Substring(binaryString.Length - 24);
            string first = cutBinaryString.Substring(0, 8);
            string second = cutBinaryString.Substring(8, 8);
            string third = cutBinaryString.Substring(16, 8);

            string rotateBinary = Rotate(first) + Rotate(second) + Rotate(third);


            return Convert.ToInt32(rotateBinary, 2).ToString();
        }
        private static string Rotate(string characters)
        {
            string output = "";
            for (int i = characters.Length; i > 0; i--)
            {
                output += characters[i - 1];
            }
            return output;
        }

        public static bool IsCallbackRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            var context = HttpContext.Current;
            var isCallbackRequest = false;// callback requests are ajax requests
            if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page)
            {
                isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
            }
            return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest") || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }

        public static String GetDateTimeMonthByNumber(int month)
        {
            switch (month)
            {
                case 1: return "Januar";
                case 2: return "Februar";
                case 3: return "Marec";
                case 4: return "April";
                case 5: return "Maj";
                case 6: return "Junij";
                case 7: return "Julij";
                case 8: return "Avgust";
                case 9: return "September";
                case 10: return "Oktober";
                case 11: return "November";
                case 12: return "December";
            }

            return "";
        }

        public static DateTime GetFirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime GetLastDayOfMonth(DateTime date)
        {
            DateTime firstDay = new DateTime(date.Year, date.Month, 1);

            return firstDay.AddMonths(1).AddDays(-1);
        }

        public static List<string> GetListOfMonths(DateTime startDate, DateTime endDate, bool addYear = false)
        {
            int monthDifference = GetMonthDifference(startDate, endDate);
            int startMonth = startDate.Month;

            List<string> meseci = new List<string>();
            for (int i = 0; i <= monthDifference; i++)
            {
                if (startMonth > 12)
                    startMonth = 1;
                meseci.Add(CommonMethods.GetDateTimeMonthByNumber(startMonth) + (addYear ? "_" + startDate.AddMonths(i).Year.ToString() : ""));
                startMonth++;
            }
            return meseci;
        }

        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GetNameByUser(Users usr)
        {
            string sName = "";
            string sLastName = "";
            string sFirstName = "";
            
            if (usr == null) return "";

            if ((usr.Lastname != null) && (usr.Lastname.Length > 0))
            {
                sLastName = usr.Lastname;
            }
            if ((usr.Firstname != null) && (usr.Firstname.Length > 0))
            {
                sFirstName = usr.Firstname;
            }

            sName = sLastName + " " + sFirstName;

            return sName;

        }

        public static string GetTimeStamp()
        {
            return PrincipalHelper.GetUserPrincipal().firstName + "_" + PrincipalHelper.GetUserPrincipal().lastName + "_" + DateTime.Now.ToString("dd.MM.yyyy - hh:mm");
        }
    }
}