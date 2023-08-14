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
    public class UserRepository : IUserRepository
    {
        Session session;
        IKVPGroupsRepository groupRepo;

        public UserRepository(Session session)
        {
            this.session = session;
            groupRepo = new KVPGroupsRepository(this.session);
        }

        public UserModel UserLogIn(string userName, string password)
        {
            try
            {
                XPQuery<Users> list = session.Query<Users>();

                Users user = list.Where(u => (u.Username != null && u.Username.CompareTo(userName) == 0) && (u.Password != null && u.Password.CompareTo(password) == 0)).FirstOrDefault();
                UserModel model = null;

                if (user != null)
                {
                    if(!user.eKVPPrijava)
                        throw new EmployeeCredentialsException(AuthenticationValidation_Exception.res_04);

                    if (String.Compare(user.Username, userName, false) != 0 && String.Compare(user.Password, password) != 0)
                        return null;

                    model = FillUserModel(user);

                }

                return model;
            }
            catch (EmployeeCredentialsException ex)
            {
                throw new EmployeeCredentialsException(ex.Message);
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public UserModel UserLogInCard(string token)
        {
            try
            {
                XPQuery<Users> list = session.Query<Users>();
                Users user = list.Where(u => u.Card.CompareTo(token) == 0).FirstOrDefault();
                UserModel model = null;

                if (user != null)
                {
                    if (!user.eKVPPrijava)
                        throw new EmployeeCredentialsException(AuthenticationValidation_Exception.res_04);

                    if (String.Compare(user.Card, token, false) != 0)
                        return null;

                    model = FillUserModel(user);

                }

                return model;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        private UserModel FillUserModel(Users user)
        {
            UserModel model = new UserModel();

            model.ID = user.Id;

            model.firstName = user.Firstname;
            model.lastName = user.Lastname;
            model.email = user.Email;
            model.profileImage = user.Picture;
            model.username = user.Username;
            model.DepartmentName = user.DepartmentId.Name;
            model.Card = user.Card;

            var supervisor = GetEmployeeByID(user.DepartmentId.DepartmentHeadId);

            model.Supervisor = supervisor != null ? supervisor.Firstname + " " + supervisor.Lastname : "";
            // model.SupervisorId = supervisor != null ? supervisor.Id : 0;

            var groupUser = groupRepo.GetKVPGroupUserByUserID(user.Id);

            if (groupUser != null)
            {
                List<KVPSkupina_Users> list = groupRepo.GetKVPGroupUsersChampionsByGroupID(groupUser.idKVPSkupina.idKVPSkupina).Where(ch => ch.Champion).ToList();

                foreach (var item in list)
                {
                    model.Champion += item.IdUser.Firstname + " " + item.IdUser.Lastname + ", ";
                }
                model.Champion = !String.IsNullOrEmpty(model.Champion) ? model.Champion.Remove(model.Champion.LastIndexOf(", ")) : "";
                model.GroupName = groupUser.idKVPSkupina.Naziv;
                model.GroupID = groupUser.idKVPSkupina.idKVPSkupina;
            }
            else//če zaposlen ni v KVP skupine se ne sme prijaviti v sistem
            {
                if(user.RoleID == null)
                    throw new EmployeeCredentialsException(AuthenticationValidation_Exception.res_02);

                if (user.RoleID.Koda != Enums.UserRole.Admin.ToString() && user.RoleID.Koda != Enums.UserRole.SuperAdmin.ToString())
                    throw new EmployeeCredentialsException(AuthenticationValidation_Exception.res_03);
            }

            model.RoleName = user.RoleID.Naziv;

            if (user.RoleID != null)
            {
                model.RoleID = user.RoleID.VlogaID;
                model.Role = user.RoleID.Koda;
                model.RoleName = user.RoleID.Naziv;
            }

            return model;
        }

        /*public Vloga_OTP GetRoleByID(int id)
        {
            try
            {
                return new XPCollection<Vloga_OTP>(session).Where(r => r.idVloga == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }*/

        /*public string GetRoleNameByID(int id)
        {
            try
            {
                Vloga_OTP role = GetRoleByID(id);
                return role != null ? role.Naziv : "";
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_01, error, CommonMethods.GetCurrentMethodName()));
            }
        }*/

        public Users GetEmployeeByID(int id)
        {
            try
            {
                XPQuery<Users> user = user = session.Query<Users>();

                return user.Where(e => e.Id == id).FirstOrDefault();
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