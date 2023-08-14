using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Helpers.DataProvder
{
    public class KVPGroupsProvider : ServerMasterPage
    {
        /// <summary>
        /// Add KVPGroup instance to session
        /// </summary>
        /// <param name="model"></param>
        public void SetKVPGroupModel(KVPSkupina model)
        {
            AddValueToSession(Enums.KVPGroups.KVPGroupModel, model);
        }

        /// <summary>
        /// Returns KVPGroup data From session. If session does not exist it returs null.
        /// </summary>
        /// <returns></returns>
        public KVPSkupina GetKVPGroupModel()
        {
            if (SessionHasValue(Enums.KVPGroups.KVPGroupModel))
                return (KVPSkupina)GetValueFromSession(Enums.KVPGroups.KVPGroupModel);

            return null;
        }
    }
}