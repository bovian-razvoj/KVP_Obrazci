using DevExpress.Xpo;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KVP_Obrazci.Domain.Abstract
{
    public interface IPostRepository
    {
        Prispevek GetPostByID(int id, Session currentSession = null);
        PrispevekKategorija GetPostCategoryByID(int id, Session currentSession = null);
        List<PrispevekKategorija> GetCategories();
        int SavePost(Prispevek model);
        int SaveCategory(PrispevekKategorija categorie);
        void DeletePost(int id);
        void DeletePost(Prispevek model);

        void DeleteCategory(int id);
        void DeleteCategory(PrispevekKategorija model);

        List<PostModel> GetLastPosts(int count);
    }
}
