using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cover.Backend.Entities;
using System.Data.SqlClient;
using System.Data;

namespace Cover.Backend.Repositories
{
    public class UserRepository
    {
        #region "Create_Events"

        public void CreateUser(User user)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Name", user.Name));
            parameters.Add(new SqlParameter("Password", user.Password));
            parameters.Add(new SqlParameter("RoleId", user.Role.Id));
            DataAccess.Helper.ExecuteNonQuery("User_Insert", parameters);
        }

        #endregion

        #region "Update_Events"

        public void UpdateUser(User user)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Name", user.Name));
            parameters.Add(new SqlParameter("Password", user.Password));
            parameters.Add(new SqlParameter("RoleId", user.Role.Id));
            parameters.Add(new SqlParameter("Id", user.Id));
            DataAccess.Helper.ExecuteNonQuery("User_Update", parameters);
        }

        public void ChangeStatus(Int64 userId, Boolean active)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", userId));
            parameters.Add(new SqlParameter("Active", active));
            DataAccess.Helper.ExecuteNonQuery("User_ChangeStatus", parameters);
        }

        #endregion

        #region "Get_Events"
        public User GetUserById(Int64 id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", id));
            var dt = DataAccess.Helper.ExecuteDataTable("User_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new User()
            {
                Id = id,
                Name = dt.Rows[0]["Name"].ToString(),
                Password = dt.Rows[0]["Password"].ToString(),
                Role = new Role() { Id = Convert.ToInt32(dt.Rows[0]["RoleId"]), Name = dt.Rows[0]["RoleName"].ToString() },
            };
        }

        public List<User> GetUsers()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("User_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new User()
                    {
                        Id = i.Field<Int64>("Id"),
                        Name = i.Field<String>("Name"),
                        Active = i.Field<Boolean>("Active"),
                        Password = i.Field<String>("Password"),
                        Role = new Role() { Id = i.Field<Int32>("RoleId"), Name = i.Field<String>("RoleName") }
                    }).ToList();
        }

        #endregion
        
    }
}
