using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.KVPOdelo;
using KVP_Obrazci.Domain.Models;
using KVP_Obrazci.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace KVP_Obrazci.Domain.Concrete
{
    public class KodeksToEKVPRepository : IKodeksToEKVPRepository
    {
        IDepartmentRepository departmentRepo;
        IEmployeeRepository employeeRepo;

        SqlConnection connection;
        string connString;
        public KodeksToEKVPRepository(Session session = null)
        {
            if (session == null)
                session = XpoHelper.GetNewSession();

            connString = ConfigurationManager.ConnectionStrings["KodeksOdelo"].ToString();
            departmentRepo = new DepartmentRepository(session);
            employeeRepo = new EmployeeRepository(session);
        }

        private bool OpenConnection()
        {
            connection = new SqlConnection(connString);
            connection.Open();

            return true;
        }

        private bool CloseConnection()
        {
            connection.Close();
            connection.Dispose();
            return true;
        }


        public List<KodeksUsersModel> GetKodeksUsers()
        {
            List<KodeksUsersModel> list = new List<KodeksUsersModel>();

            OpenConnection();
            string sSQL = "SELECT * FROM Users";
            sSQL += " where Deleted <> 1";
            //sSQL += " where Id = '1145'";
            //sSQL += " and  ExternalId = '12284'";
            //sSQL += "and Firstname ='Kovše' or Firstname = 'Majster'";
            //sSQL += "and Lastname ='Bayraktar' or Lastname = 'Kurtaj' or Lastname = 'Yildiz' or Lastname = 'Gauch' or Lastname = 'Subašić' or Lastname = 'Rilind' ";
            //sSQL += " and Id > 5173 and Id < 5184";
            SqlCommand command = new SqlCommand(sSQL, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new KodeksUsersModel
                {
                    Id = reader.IsDBNull(0) ? -1 : reader.GetInt32(0),
                    Firstname = reader.IsDBNull(3) ? "NULL" : reader.GetString(3),
                    Lastname = reader.IsDBNull(2) ? "NULL" : reader.GetString(2),
                    DepartmentId = reader.IsDBNull(7) ? -1 : reader.GetInt32(7),
                    ExternalId = reader.IsDBNull(9) ? "NULL" : reader.GetString(9),
                    IsTimeAttendance = reader.IsDBNull(28) ? false : reader.GetBoolean(28),
                    Deleted = reader.IsDBNull(2) ? false : reader.GetBoolean(1),
                    Card = reader.IsDBNull(11) ? "NULL" : reader.GetString(11),
                    Username = reader.IsDBNull(30) ? "NULL" : reader.GetString(30),
                    Password = reader.IsDBNull(31) ? "NULL" : reader.GetString(31),
                    Email = reader.IsDBNull(33) ? "NULL" : reader.GetString(33),
                    ValidFrom = reader.GetDateTime(35)
                });
            }
            CloseConnection();

            return list;
        }

        public void UpdateUser(KodeksUsersModel newValues, params string[] columnsToUpdate)
        {
            SqlCommand command = new SqlCommand();
            string query = "UPDATE Users SET ";

            foreach (var item in columnsToUpdate)
            {
                System.Data.SqlDbType type = System.Data.SqlDbType.NVarChar;
                if (item == "Id" || item == "DepartmentId")
                    type = System.Data.SqlDbType.Int;

                query += item + "=@" + item;
                command.Parameters.Add("@" + item, type).Value = GetValueByFieldName(newValues, item);
            }

            query += " WHERE Id=@Id";
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = newValues.Id;

            command.CommandText = query;

            OpenConnection();
            command.Connection = connection;
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public void UpdateUser(KodeksUsersModel newValues)
        {
            SqlCommand command = new SqlCommand("UPDATE Users SET Firstname=@Firstname, Lastname=@Lastname, DepartmentId=@DepartmentId, ExternalId=@ExternalId  WHERE Id=@Id", connection);
            command.Parameters.Add("@Firstname", System.Data.SqlDbType.NVarChar).Value = newValues.Firstname;
            command.Parameters.Add("@Lastname", System.Data.SqlDbType.NVarChar).Value = newValues.Lastname;
            command.Parameters.Add("@DepartmentId", System.Data.SqlDbType.Int).Value = newValues.DepartmentId;
            command.Parameters.Add("@ExternalId", System.Data.SqlDbType.NVarChar).Value = newValues.ExternalId;
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = newValues.Id;


            OpenConnection();
            command.Connection = connection;
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public void SaveNewUser(KodeksUsersModel newUser)
        {
            SqlCommand command = new SqlCommand("INSERT INTO  Users VALUES(@Firstname, @Lastname, @DepartmentId, @ExternalId)", connection);

            command.Parameters.Add("@Firstname", System.Data.SqlDbType.NVarChar).Value = newUser.Firstname;
            command.Parameters.Add("@Lastname", System.Data.SqlDbType.NVarChar).Value = newUser.Lastname;
            command.Parameters.Add("@DepartmentId", System.Data.SqlDbType.Int).Value = newUser.DepartmentId;
            command.Parameters.Add("@ExternalId", System.Data.SqlDbType.NVarChar).Value = newUser.ExternalId;



            OpenConnection();
            command.Connection = connection;
            command.ExecuteNonQuery();
            CloseConnection();
        }

        /***KODKS - DEPARTMENTS****/
        public List<KodeksDepartmentsModel> GetKodeksDepartments()
        {
            List<KodeksDepartmentsModel> list = new List<KodeksDepartmentsModel>();

            OpenConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM Departments", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new KodeksDepartmentsModel
                {
                    Id = reader.IsDBNull(0) ? -1 : reader.GetInt32(0),
                    Name = reader.IsDBNull(2) ? "NULL" : reader.GetString(2),
                    Code = reader.IsDBNull(3) ? "NULL" : reader.GetString(3),
                    DepartmentHeadId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4),
                    DepartmentHeadDeputyId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5),
                    ParentId = reader.IsDBNull(6) ? -1 : reader.GetInt32(6),
                });
            }
            CloseConnection();

            return list;
        }

        public KodeksDepartmentsModel GetKodeksDepartmentsById(Int32 iDepID)
        {
            List<KodeksDepartmentsModel> list = new List<KodeksDepartmentsModel>();

            OpenConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM Departments where Id=" + iDepID, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new KodeksDepartmentsModel
                {
                    Id = reader.IsDBNull(0) ? -1 : reader.GetInt32(0),
                    Name = reader.IsDBNull(2) ? "NULL" : reader.GetString(2),
                    Code = reader.IsDBNull(3) ? "NULL" : reader.GetString(3),
                    DepartmentHeadId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4),
                    DepartmentHeadDeputyId = reader.IsDBNull(5) ? -1 : reader.GetInt32(5),
                    ParentId = reader.IsDBNull(6) ? -1 : reader.GetInt32(6),
                });
            }
            CloseConnection();

            if (list.Count > 0)
            {
                return list[0];
            }

            return null;
        }

        public void UpdateDepartment(KodeksDepartmentsModel newValues, params string[] columnsToUpdate)
        {
            SqlCommand command = new SqlCommand();
            string query = "UPDATE Departments SET ";

            foreach (var item in columnsToUpdate)
            {
                System.Data.SqlDbType type = System.Data.SqlDbType.Int;
                if (item == "Name" || item == "Code")
                    type = System.Data.SqlDbType.NVarChar;

                query += item + "=@" + item;
                command.Parameters.Add("@" + item, type).Value = GetValueByFieldName(newValues, item);
            }

            query += " WHERE Id=@Id";
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = newValues.Id;

            command.CommandText = query;

            OpenConnection();
            command.Connection = connection;
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public void UpdateDepartment(KodeksDepartmentsModel newValues)
        {
            SqlCommand command = new SqlCommand("UPDATE Users SET Name=@Name, Code=@Code, DepartmentHeadId=@DepartmentHeadId, DepartmentHeadDeputyId=@DepartmentHeadDeputyId, ParentId=@ParentId  WHERE Id=@Id", connection);
            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = newValues.Name;
            command.Parameters.Add("@Code", System.Data.SqlDbType.NVarChar).Value = newValues.Code;
            command.Parameters.Add("@DepartmentHeadId", System.Data.SqlDbType.Int).Value = newValues.DepartmentHeadId;
            command.Parameters.Add("@DepartmentHeadDeputyId", System.Data.SqlDbType.Int).Value = newValues.DepartmentHeadDeputyId;
            command.Parameters.Add("@ParentId", System.Data.SqlDbType.Int).Value = newValues.ParentId;
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = newValues.Id;


            OpenConnection();
            command.Connection = connection;
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public void SaveNewDepartment(KodeksDepartmentsModel newDepartment)
        {
            SqlCommand command = new SqlCommand("INSERT INTO  Departments VALUES(@Name, @Code, @DepartmentHeadId, @DepartmentHeadDeputyId, @ParentId)", connection);

            command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = newDepartment.Name;
            command.Parameters.Add("@Code", System.Data.SqlDbType.NVarChar).Value = newDepartment.Code;
            command.Parameters.Add("@DepartmentHeadId", System.Data.SqlDbType.Int).Value = newDepartment.DepartmentHeadId;
            command.Parameters.Add("@DepartmentHeadDeputyId", System.Data.SqlDbType.Int).Value = newDepartment.DepartmentHeadDeputyId;
            command.Parameters.Add("@ParentId", System.Data.SqlDbType.Int).Value = newDepartment.ParentId;



            OpenConnection();
            command.Connection = connection;
            command.ExecuteNonQuery();
            CloseConnection();
        }

        private object GetValueByFieldName<T>(T values, string columnName)
        {
            Type myType = typeof(T);

            PropertyInfo[] infoProperties = myType.GetProperties();
            PropertyInfo prop = infoProperties.Where(i => i.Name.Equals(columnName)).FirstOrDefault();
            return prop.GetValue(values);
        }

        public void MergeKodeks_eKVP(Session currentSession = null)
        {

            if (currentSession == null)
                currentSession = XpoHelper.GetNewSession();

            MergeDepartments(currentSession);

            MergeUsers(currentSession);
        }

        #region "Merge users"


        private void MergeUsers(Session currentSession)
        {
            
            List<KodeksUsersModel> kodeksUsers = GetKodeksUsers();
            foreach (KodeksUsersModel kum in kodeksUsers)
            {
                // check if there is ExternalID and 

                // set employee which changed names
                SetNameChangedByEternalID(kum.ExternalId, kum, currentSession);

                //// set employees change name or internalId
                //SetEmployeeChangedNameOrExternalId(kum.ExternalId, kum.Firstname, kum.Lastname, kum, currentSession);

                // set employees which left company
                SetLeftCompanyUserByFirstnameAndLastnameExternalId(kum.ExternalId, kum.Firstname, kum.Lastname, kum, currentSession);

                // set change department on employees
                SetChangeDepartmentByFirstnameAndLastnameExternalId(kum.ExternalId, kum.Firstname, kum.Lastname, kum, currentSession);

                // set startdate employees
                SetStartDateByFirstnameAndLastnameExternalId(kum.ExternalId, kum.Firstname, kum.Lastname, kum, currentSession);

                // add new employee to eKVP
                AddOrUpdateNameEmployeetoeKVPFirstnameAndLastnameExternalId(kum.ExternalId, kum.Firstname, kum.Lastname, kum, currentSession);

                // set employees change name or internalId
                CheckIfPersonalInfoIsChanged(kum.ExternalId, kum.Firstname, kum.Lastname, kum, currentSession);
            }

            // Check for duplicates
            SetDuplicatesUsers(currentSession);
        }




        /// <summary>
        /// Find all employees who has duplicate values 
        /// </summary>
        /// <param name="currentSession"></param>
        private void SetDuplicatesUsers(Session currentSession)
        {
            XPCollection<Users> usrColl = new XPCollection<Users>(currentSession);
            foreach (Users usr in usrColl)
            {
                SetDuplicateValuesByFirstnameAndLastname("", usr.Firstname, usr.Lastname, currentSession);
                SetDuplicateValuesByFirstnameAndLastname(usr.ExternalId, "", "", currentSession);
            }
        }

        /// <summary>
        /// Set all users if they have duplicate values ExternalID, Firstname, Lastname marked as usr.IsDuplicated = 1;
        /// </summary>
        /// <param name="sExternalId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastname"></param>
        /// <param name="currentSession"></param>
        private void SetDuplicateValuesByFirstnameAndLastname(string sExternalId, string sFirstName, string sLastname, Session currentSession)
        {
            CriteriaOperator filterCriteria = null;
            if (sExternalId != null)
            {
                if (sExternalId.Length > 0)
                {
                    filterCriteria = CriteriaOperator.Parse("Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "' and IsDuplicated = 0");
                }
                else
                {
                    filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "' and IsDuplicated = 0");
                }
                XPCollection<Users> usrColl = new XPCollection<Users>(currentSession, filterCriteria);

                if (usrColl.Count > 1)
                {
                    foreach (Users usr in usrColl)
                    {
                        usr.IsDuplicated = 1;
                        usr.LastEventTime = DateTime.Now;
                        usr.Save();

                    }
                }
            }
        }


        /// <summary>
        /// Check if there ware changes in Fistname, Lastname marked as usr.NameChanged = 1;
        /// </summary>
        /// <param name="sExternalId"></param>
        /// <param name="kUsers"></param>
        /// <param name="currentSession"></param>
        private void SetNameChangedByEternalID(string sExternalId, KodeksUsersModel kum, Session currentSession)
        {
            CriteriaOperator filterCriteria = null;

            if (sExternalId.Length > 0)
            {
                filterCriteria = CriteriaOperator.Parse("Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "'");


                XPCollection<Users> usrColl = new XPCollection<Users>(currentSession, filterCriteria);

                if (usrColl.Count > 0)
                {
                    foreach (Users usr in usrColl)
                    {
                        if ((usr.Firstname.ToUpper() != kum.Firstname.ToUpper()) || (usr.Lastname.ToUpper() != kum.Lastname.ToUpper()) && usr.NameChanged != 1)
                        {
                            if (usr.Deleted == true)
                            {
                                usr.Firstname = kum.Firstname;
                                usr.Lastname = kum.Lastname;

                                usr.NameChanged = 1;
                                usr.LastEventTime = DateTime.Now;
                                usr.Save();
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Marked emplyees who left company
        /// </summary>
        /// <param name="sExternalId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastname"></param>
        /// <param name="kum"></param>
        /// <param name="currentSession"></param>
        private void SetLeftCompanyUserByFirstnameAndLastnameExternalId(string sExternalId, string sFirstName, string sLastname, KodeksUsersModel kum, Session currentSession)
        {
            CriteriaOperator filterCriteria = null;

            if (sExternalId.Length > 0)
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "' and Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "'");
            }
            else
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "'");
            }
            XPCollection<Users> usrColl = new XPCollection<Users>(currentSession, filterCriteria);

            if (usrColl.Count > 0)
            {
                foreach (Users usr1 in usrColl)
                {
                    if (!usr1.EmployeeLeaveCompany) // če še ni bil potrjen s strani admin
                    {
                        usr1.IsTimeAttendance = kum.IsTimeAttendance;
                        usr1.NotEmployedAnymore = (kum.IsTimeAttendance) ? 0 : 1;
                        usr1.LastEventTime = DateTime.Now;
                        usr1.Save();
                    }


                }
            }

        }

        /// <summary>
        /// Marked emplyees who changed name or externalID
        /// </summary>
        /// <param name="sExternalId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastname"></param>
        /// <param name="kum"></param>
        /// <param name="currentSession"></param>
        private void SetEmployeeChangedNameOrExternalId(string sExternalId, string sFirstName, string sLastname, KodeksUsersModel kum, Session currentSession)
        {
            CriteriaOperator filterCriteria = null;

            if (sExternalId.Length > 0)
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "' and Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "'");
            }
            else
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "'");
            }
            XPCollection<Users> usrColl = new XPCollection<Users>(currentSession, filterCriteria);

            if (usrColl.Count == 0)
            {
                usrColl = new XPCollection<Users>(currentSession, filterCriteria);
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "'");

                foreach (Users usr1 in usrColl)
                {
                    usr1.ExternalId = kum.ExternalId;
                    usr1.NameChanged = 1;
                    usr1.LastEventTime = DateTime.Now;
                    usr1.Save();
                }
            }

        }


        /// <summary>
        /// Marked employees who has changed DepartmentID
        /// </summary>
        /// <param name="sExternalId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastname"></param>
        /// <param name="kum"></param>
        /// <param name="currentSession"></param>
        private void SetChangeDepartmentByFirstnameAndLastnameExternalId(string sExternalId, string sFirstName, string sLastname, KodeksUsersModel kum, Session currentSession)
        {
            CriteriaOperator filterCriteria = null;

            if (sExternalId.Length > 0)
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "' and Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "'");
            }
            else
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "'");
            }
            XPCollection<Users> usrColl = new XPCollection<Users>(currentSession, filterCriteria);

            if (usrColl.Count > 0)
            {
                foreach (Users usr in usrColl)
                {
                    if ((usr.DepartmentId == null) && (kum.DepartmentId > 0))
                    {
                        KodeksDepartmentsModel kdm = GetKodeksDepartmentsById(kum.DepartmentId);
                        if (kdm != null)
                        {
                            usr.DepartmentId = departmentRepo.GetDepartmentByName(kdm.Name, currentSession);
                            usr.ChangedDepartment = true;
                            usr.LastEventTime = DateTime.Now;
                            usr.Save();
                        }
                    }

                    if ((usr.DepartmentId != null) && (usr.DepartmentId.Id > 0))
                    {
                        if (kum.DepartmentId != usr.DepartmentId.Id)
                        {
                            if (!kum.Deleted)
                            {
                                KodeksDepartmentsModel kdm = GetKodeksDepartmentsById(kum.DepartmentId);
                                if (kdm != null)
                                {
                                    usr.DepartmentId = departmentRepo.GetDepartmentByName(kdm.Name, currentSession);
                                    usr.ChangedDepartment = true;
                                    usr.LastEventTime = DateTime.Now;
                                    usr.Save();
                                }
                            }
                        }
                        else
                        {
                            KodeksDepartmentsModel kdm = GetKodeksDepartmentsById(kum.DepartmentId);
                            Departments departUsr = departmentRepo.GetDepartmentByID(usr.DepartmentId.Id);

                            if (kdm.Name != departUsr.Name)
                            {
                                Departments depart = GetDepartmentByNameInKVP(kdm.Name, currentSession);
                                if (depart != null) usr.DepartmentId = depart;
                                usr.ChangedDepartment = true;
                                usr.LastEventTime = DateTime.Now;
                                usr.Save();
                            }

                        }
                    }

                    if ((kum.Card.Length > 0) && (kum.Card != "NULL") && (kum.Card != usr.Card))
                    {
                        usr.Card = kum.Card;
                        usr.LastEventTime = DateTime.Now;
                        usr.Save();
                    }
                }
            }

        }

        private void SetStartDateByFirstnameAndLastnameExternalId(string sExternalId, string sFirstName, string sLastname, KodeksUsersModel kum, Session currentSession)
        {
            CriteriaOperator filterCriteria = null;

            if (sExternalId.Length > 0)
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "' and Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "'");
            }
            else
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "'");
            }
            XPCollection<Users> usrColl = new XPCollection<Users>(currentSession, filterCriteria);

            if (usrColl.Count > 0)
            {
                foreach (Users usr in usrColl)
                {
                    usr.TAStartDate = (usr.TAStartDate != kum.ValidFrom) ? kum.ValidFrom : usr.TAStartDate;
                    usr.LastEventTime = DateTime.Now;
                    usr.Save();
                }


            }
        }



        /// <summary>
        /// Marked all new employees who must be linkied to KVP groups
        /// </summary>
        /// <param name="sExternalId"></param>
        /// <param name="sFirstName"></param>
        /// <param name="sLastname"></param>
        /// <param name="kum"></param>
        /// <param name="currentSession"></param>
        private void AddOrUpdateNameEmployeetoeKVPFirstnameAndLastnameExternalId(string sExternalId, string sFirstName, string sLastname, KodeksUsersModel kum, Session currentSession)
        {
            CriteriaOperator filterCriteria = null;
            if ((sFirstName.Length == 0) && (sLastname.Length == 0)) return;
            if (sExternalId.Length > 0)
            {
                filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "' and Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "'");
            }
            XPCollection<Users> usrColl = new XPCollection<Users>(currentSession, filterCriteria);
            if (usrColl.Count == 0)
            {
                if (!kum.Deleted)
                {
                    Users newUsr = new Users(currentSession);

                    newUsr.Firstname = kum.Firstname;
                    newUsr.Lastname = kum.Lastname;
                    newUsr.ExternalId = kum.ExternalId;
                    newUsr.Card = kum.Card;
                    newUsr.DepartmentId = departmentRepo.GetDepartmentByID(kum.DepartmentId, currentSession);
                    newUsr.Email = kum.Email;
                    newUsr.TAStartDate = kum.ValidFrom;
                    newUsr.Save();
                    SetUsernameAndPassowordByRule(0, newUsr, currentSession);

                    newUsr.UpravicenDoKVP = true;
                    newUsr.UpravicenDoEPoste = true;
                    newUsr.eKVPPrijava = true;
                    newUsr.NewEmployee = 1;
                    newUsr.IsDuplicated = 0;
                    newUsr.NotEmployedAnymore = 0;
                    newUsr.ValidFrom = kum.ValidFrom;
                    newUsr.RoleID = employeeRepo.GetRoleByID(3, currentSession);
                    newUsr.SecondRoleID = employeeRepo.GetRoleByID(3, currentSession);
                    newUsr.LastEventTime = DateTime.Now;
                    newUsr.Save();
                }
            }            
        }

        private void CheckIfPersonalInfoIsChanged(string sExternalId, string sFirstName, string sLastname, KodeksUsersModel kum, Session currentSession)
        {
            CriteriaOperator filterCriteria = null;
            XPCollection<Users> usrColl = null;
            filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(Lastname)='" + sLastname.Trim().ToUpper() + "'");
            usrColl = new XPCollection<Users>(currentSession, filterCriteria);
            if (usrColl.Count > 1)
            {
                foreach (Users usr in usrColl)
                {
                    //usr.ExternalId = sExternalId;
                    usr.NameChanged = 1;
                    usr.LastEventTime = DateTime.Now;
                    usr.Save();
                }
            }

            filterCriteria = CriteriaOperator.Parse("Upper(Firstname)='" + sFirstName.Trim().ToUpper() + "' and Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "'");
            usrColl = new XPCollection<Users>(currentSession, filterCriteria);
            if (usrColl.Count > 1)
            {
                foreach (Users usr in usrColl)
                {
                    //usr.ExternalId = sExternalId;
                    usr.NameChanged = 1;
                    usr.LastEventTime = DateTime.Now;
                    usr.Save();
                }
            }

            filterCriteria = CriteriaOperator.Parse("Upper(Lastname)='" + sLastname.Trim().ToUpper() + "' and Upper(ExternalId)='" + sExternalId.Trim().ToUpper() + "'");
            usrColl = new XPCollection<Users>(currentSession, filterCriteria);
            if (usrColl.Count > 1)
            {
                foreach (Users usr in usrColl)
                {
                    //usr.ExternalId = sExternalId;
                    usr.NameChanged = 1;
                    usr.LastEventTime = DateTime.Now;
                    usr.Save();
                }
            }
        }



        private void SetUsernameAndPassowordByRule(int iZaporendaC, Users usr, Session currentSession)
        {
            string sUsrNm = CreateUserName(iZaporendaC, usr);
            if (CheckUserName(iZaporendaC, sUsrNm, usr.Id, currentSession) > 0)
            {
                if (iZaporendaC > 6)
                {
                    usr.Lastname = usr.Lastname + iZaporendaC;
                    usr.Save();
                }
                SetUsernameAndPassowordByRule(iZaporendaC + 1, usr, currentSession);

            }
            else
            {
                usr.Username = sUsrNm;
                usr.Password = CreatePassword(usr);
                usr.LastEventTime = DateTime.Now;
                usr.Save();
            }
        }

        /// <summary>
        /// Preveri ali obstaja že uporabniško ime, če obstaja vrže koliko črk imena lahko odrežem
        /// </summary>
        /// <param name="iNumberOfLetters"></param>
        /// <param name="usrName"></param>
        /// <returns></returns>
        private Int32 CheckUserName(int iNumberOfLetters, string usrName, Int32 iCurrentIDUser, Session currentSession)
        {
            CriteriaOperator filterCriteria = CriteriaOperator.Parse("Username ='" + usrName + "' and Id <> " + iCurrentIDUser);

            XPCollection<Users> collection_U = new XPCollection<Users>(currentSession, filterCriteria);
            foreach (Users usr in collection_U)
            {
                return iNumberOfLetters + 1;
            }

            return 0;
        }

        private string CreateUserName(Int32 iNumberOfLetters, Users usr)
        {
            string sFirstName = usr.Firstname;
            string sLastName = usr.Lastname;

            sFirstName = Common.CommonMethods.ReplaceSumniki(sFirstName.ToLower());
            sLastName = Common.CommonMethods.ReplaceSumniki(sLastName.ToLower());
            sFirstName = sFirstName.Replace(" ", "");
            sLastName = sLastName.Replace(" ", "");
            if (sFirstName.Length > 1)
            {
                sFirstName = sFirstName.Substring(0, iNumberOfLetters + 1);
            }
            string sUserName = sFirstName + sLastName;

            return sUserName;
        }

        private string CreatePassword(Users usr)
        {
            string sFirstName = usr.Firstname;
            string sLastName = usr.Lastname;

            sFirstName = Common.CommonMethods.ReplaceSumniki(sFirstName.ToLower());
            sLastName = Common.CommonMethods.ReplaceSumniki(sLastName.ToLower());

            if (sFirstName.Length > 1)
            {
                sFirstName = sFirstName.Substring(0, 1);
            }

            if (sLastName.Length > 1)
            {
                sLastName = sLastName.Substring(0, 1);
            }
            else
            {
                sLastName = "";
            }
            string sPassword = sFirstName + sLastName + "123.";

            return sPassword;
        }
        #endregion

        #region "Merge departments"

        private void MergeDepartments(Session currentSession)
        {
            List<KodeksDepartmentsModel> kodeksDepartments = GetKodeksDepartments();

            foreach (KodeksDepartmentsModel kdm in kodeksDepartments)
            {
                if (kdm.Name.Length > 0)
                {
                    Departments depart = GetDepartmentByNameInKVP(kdm.Name, currentSession);
                    if (depart != null)
                    {
                        if (depart.DepartmentHeadId == null) depart.DepartmentHeadId = kdm.DepartmentHeadId;
                        if (depart.DepartmentHeadDeputyId == null) depart.DepartmentHeadDeputyId = kdm.DepartmentHeadDeputyId;
                        if (depart.ParentId == null)
                            depart.DepartmentHeadDeputyId = kdm.DepartmentHeadDeputyId;
                        else if (depart.ParentId != kdm.ParentId)
                        {
                            depart.ParentId = kdm.ParentId;
                        }
                    }
                    else
                    {
                        Departments newDepart = new Departments(currentSession);

                        newDepart.Name = kdm.Name;
                        newDepart.FullName = kdm.Name;
                        newDepart.DepartmentHeadId = kdm.DepartmentHeadId;
                        newDepart.DepartmentHeadDeputyId = kdm.DepartmentHeadDeputyId;
                        newDepart.ParentId = kdm.ParentId;

                        newDepart.Save();
                    }
                }
            }
        }

        private Departments GetDepartmentByNameInKVP(string sDepartmentName, Session currentSession)
        {
            CriteriaOperator filterCriteria = CriteriaOperator.Parse("Upper([Name]) ='" + sDepartmentName.Trim().ToUpper() + "' and [Deleted] = 0");

            XPCollection<Departments> departColl = new XPCollection<Departments>(currentSession, filterCriteria);
            foreach (Departments dep in departColl)
            {
                return dep;

            }
            return null;
        }

        #endregion
    }
}
