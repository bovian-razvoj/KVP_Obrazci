using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Resources;
using KVP_Obrazci.Domain.Models;
namespace KVP_Obrazci.Domain.Concrete
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        Session session;

        public ErrorLogRepository(Session session)
        {
            this.session = session;
        }

        public ErrorLog SaveErrorLog(string sErrorMsg, int idPrijava)
        {
            try
            {
                ErrorLog model = new ErrorLog(session);

                model.ErrorMsg = sErrorMsg;
                model.idPrijava = idPrijava;
                model.ts = DateTime.Now;
                model.Save();

                return model;

                
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }

        }
    }
}