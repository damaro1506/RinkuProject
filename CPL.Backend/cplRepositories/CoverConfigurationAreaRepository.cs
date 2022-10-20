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
    public class CoverConfigurationAreaRepository
    {
        public List<CoverConfigurationArea> GetCoverConfigurationAreas(Int64 coverConfigurationId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
            var dt = DataAccess.Helper.ExecuteDataTable("CoverConfigurationArea_GetList", parameters);

            return (from i in dt.AsEnumerable()
                    select new CoverConfigurationArea()
                    {
                        CoverConfigurationId = i.Field<Int64>("CoverConfigurationId"),
                        AreaId = i.Field<Int32>("AreaId"),
                    }).ToList();
        }

        public void InsertCoverConfigurationArea(Int64 coverConfigurationId, Int32 areaId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
            parameters.Add(new SqlParameter("AreaId", areaId));
            DataAccess.Helper.ExecuteNonQuery("CoverConfigurationArea_Insert", parameters);
        }

        public void DeleteCoverConfigurationArea(Int64 coverConfigurationId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
            DataAccess.Helper.ExecuteNonQuery("CoverConfigurationArea_Delete", parameters);
        }

    }
}
