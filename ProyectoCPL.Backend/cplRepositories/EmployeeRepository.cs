using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoCPL.Backend.Entities;
using System.Data.SqlClient;
using System.Data;


namespace ProyectoCPL.Backend.cplRepositories
{
    public class EmployeeRepository
    {
        #region "Get_events"

        public Employee GetById(Int32 id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", id));
            var dt = DataAccess.Helper.ExecuteDataTable("Area_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new Employee()
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                FirstName = dt.Rows[0]["Name"].ToString(),
                Active = Convert.ToBoolean(dt.Rows[0]["Active"]),
            };
        }

        public List<Employee> GetAreasByCoverConfigurationId(Int64 coverConfigurationId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
            var dt = DataAccess.Helper.ExecuteDataTable("GetAreasByCoverConfigurationId", parameters);
            return (from i in dt.AsEnumerable()
                    select new Employee()
                    {
                        Id = i.Field<Int32>("Id"),
                        FirstName = i.Field<String>("Name"),
                        Active = i.Field<Boolean>("Active"),
                        //CoverConfigurationId = i.Field<Int64>("CoverConfigurationId"),
                    }).ToList();
        }

        public List<Employee> GetAreas()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("Area_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new Employee()
                    {
                        Id = i.Field<Int32>("Id"),
                        FirstName = i.Field<String>("Name"),
                        Active = i.Field<Boolean>("Active"),
                    }).ToList();
        }

        #endregion

        public void InsertArea(String name)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Name", name));
            DataAccess.Helper.ExecuteNonQuery("Area_Insert", parameters);
        }

        public void UpdateArea(String name, Boolean active, Int32 id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Name", name));
            parameters.Add(new SqlParameter("Active", active));
            parameters.Add(new SqlParameter("Id", id));
            DataAccess.Helper.ExecuteNonQuery("Area_Update", parameters);
        }
    }
}
