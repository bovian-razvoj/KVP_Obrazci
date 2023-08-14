using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Helpers.DataProvder
{
    public class PostProvider : ServerMasterPage
    {
        /// <summary>
        /// Add Post instance to session
        /// </summary>
        /// <param name="model"></param>
        public void SetPostModel(Prispevek model)
        {
            AddValueToSession(Enums.Post.PostModel, model);
        }

        /// <summary>
        /// Returns Post data From session. If session does not exist it returs null.
        /// </summary>
        /// <returns></returns>
        public Prispevek GetPostModel()
        {
            if (SessionHasValue(Enums.Post.PostModel))
                return (Prispevek)GetValueFromSession(Enums.Post.PostModel);

            return null;
        }
    }
}