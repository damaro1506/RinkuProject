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
    public class AreaRepository
    {
        #region "Get_events"

        public Area GetById(Int32 id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", id));
            var dt = DataAccess.Helper.ExecuteDataTable("Area_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new Area()
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Name = dt.Rows[0]["Name"].ToString(),
                Active = Convert.ToBoolean(dt.Rows[0]["Active"]),
            };
        }

        public List<Area> GetAreasByCoverConfigurationId(Int64 coverConfigurationId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
            var dt = DataAccess.Helper.ExecuteDataTable("GetAreasByCoverConfigurationId", parameters);
            return (from i in dt.AsEnumerable()
                    select new Area()
                    {
                        Id = i.Field<Int32>("Id"),
                        Name = i.Field<String>("Name"),
                        Active = i.Field<Boolean>("Active"),
                        CoverConfigurationId = i.Field<Int64>("CoverConfigurationId"),
                    }).ToList();
        }

        public List<Area> GetAreas()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("Area_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new Area()
                    {
                        Id = i.Field<Int32>("Id"),
                        Name = i.Field<String>("Name"),
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
