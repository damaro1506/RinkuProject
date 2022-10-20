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
   public class RoleRepository
    {
       public List<Role> GetRoles()
       {
           var dt = DataAccess.Helper.ExecuteDataTable("Role_GetList", null);

           return (from i in dt.AsEnumerable()
                   select new Role()
                   {
                       Id = i.Field<Int32>("Id"),
                       Name = i.Field<String>("Name"),
                   }).ToList();
       }
    }
}
