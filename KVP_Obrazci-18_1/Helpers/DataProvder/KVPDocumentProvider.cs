using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Helpers.DataProvder
{
    public class KVPDocumentProvider : ServerMasterPage
    {
        /// <summary>
        /// Add KVPDocument instance to session
        /// </summary>
        /// <param name="model"></param>
        public void SetKVPDocumentModel(KVPDocument model)
        {
            AddValueToSession(Enums.KVPDocumentSession.KVPDocumentModel, model);
        }

        /// <summary>
        /// Returns KVPDocument data From session. If session does not exist it returs null.
        /// </summary>
        /// <returns></returns>
        public KVPDocument GetKVPDocumentModel()
        {
            if (SessionHasValue(Enums.KVPDocumentSession.KVPDocumentModel))
                return (KVPDocument)GetValueFromSession(Enums.KVPDocumentSession.KVPDocumentModel);

            return null;
        }

        /// <summary>
        /// Used when user is clicking back and forward to see KVP documents
        /// </summary>
        /// <param name="kvpIDs"></param>
        public void SetKVPDocumentsIDsList(List<int> kvpIDs)
        {
            //List<int> listOfKVPIDs = kvpIDs.OfType<int>().ToList();
            AddValueToSession(Enums.CommonSession.KVPDocumentIDsToShow, kvpIDs);
        }


        /// <summary>
        /// Used when user is clicking back and forward to see KVP documents
        /// </summary>
        /// <returns></returns>
        public List<int> GetKVPDocumentsIDsList()
        {
            if (SessionHasValue(Enums.CommonSession.KVPDocumentIDsToShow))
                return (List<int>)GetValueFromSession(Enums.CommonSession.KVPDocumentIDsToShow);
            
            return null;
        }

        /// <summary>
        /// Used when user is clicking back and forward to see KVP documents
        /// </summary>
        /// <param name="kvpIDs"> set current kvp ID</param>
        public void SetKVPDocumentIDPreviuosNext(int kvpID)
        {
            AddValueToSession(Enums.CommonSession.KVPDocumentNextPreviousID, kvpID);
        }


        /// <summary>
        /// Used when user is clicking back and forward to see KVP documents
        /// </summary>
        /// <returns>Get current KVP id</returns>
        public int GetKVPDocumentIDPreviuosNext()
        {
            if (SessionHasValue(Enums.CommonSession.KVPDocumentNextPreviousID))
                return (int)GetValueFromSession(Enums.CommonSession.KVPDocumentNextPreviousID);

            return 0;
        }
    }
}