using DevExpress.Xpo;
using KVP_Obrazci.Common;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using KVP_Obrazci.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class PostRepository : IPostRepository
    {
        Session session;

        public PostRepository(Session session)
        {
            this.session = session;
        }

        public Prispevek GetPostByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<Prispevek> post = null;

                if (currentSession != null)
                    post = currentSession.Query<Prispevek>();
                else
                    post = session.Query<Prispevek>();

                return post.Where(p => p.PrispevkiID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_40, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public PrispevekKategorija GetPostCategoryByID(int id, Session currentSession = null)
        {
            try
            {
                XPQuery<PrispevekKategorija> category = null;

                if (currentSession != null)
                    category = currentSession.Query<PrispevekKategorija>();
                else
                    category = session.Query<PrispevekKategorija>();

                return category.Where(c => c.PrispevekKategorijaID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_40, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<PrispevekKategorija> GetCategories()
        {
            try
            {
                XPQuery<PrispevekKategorija> post = session.Query<PrispevekKategorija>();

                return post.Where(c => c.PrispevekKategorijaID == c.PrispevekKategorijaID).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_40, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SavePost(Prispevek model)
        {
            try
            {
                model.DatumSpremembe = DateTime.Now;
                if (model.PrispevkiID == 0)
                {
                    model.DatumVnosa = DateTime.Now;
                }

                model.Save();
                return model.PrispevkiID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_41, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveCategory(PrispevekKategorija categorie)
        {
            try
            {
                if (categorie.PrispevekKategorijaID == 0)
                {
                    categorie.ts = DateTime.Now;
                    categorie.IDPrijave = PrincipalHelper.GetUserPrincipal().ID;
                }

                categorie.Save();
                return categorie.PrispevekKategorijaID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_41, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeletePost(int id)
        {
            try
            {
                Prispevek model = GetPostByID(id);

                if (model != null)
                    model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_42, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeletePost(Prispevek model)
        {
            try
            {
                if (model != null)
                    model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_42, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                PrispevekKategorija model = GetPostCategoryByID(id);

                if (model != null)
                    model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_42, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public void DeleteCategory(PrispevekKategorija model)
        {
            try
            {
                if (model != null)
                    model.Delete();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_42, error, CommonMethods.GetCurrentMethodName()));
            }
        }


        public List<PostModel> GetLastPosts(int count)
        {
            try
            {
                XPQuery<Prispevek> posts = session.Query<Prispevek>();

                return posts.Where(p => p.Objavljen).OrderByDescending(p => p.DatumVnosa).Take(count).Select(p => new PostModel
                {
                    Besedilo = p.Besedilo,
                    Avtor = p.AvtorID.Firstname + " " + p.AvtorID.Lastname,
                    DatumVnosa = p.DatumVnosa,
                    Kategorija = p.KategorijaID.Naziv,
                    Naslov = p.Naslov,
                    PrikaznaSlika = p.PrikaznaSlika
                }).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_40, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}