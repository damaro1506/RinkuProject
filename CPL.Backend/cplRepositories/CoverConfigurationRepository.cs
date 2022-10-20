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
    public class CoverConfigurationRepository
    {
        public Int64 CreateCoverConfiguration(CoverConfiguration coverconfig)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Name", coverconfig.Name));
            parameters.Add(new SqlParameter("Code", coverconfig.Code));
            parameters.Add(new SqlParameter("CustomerTypeId", coverconfig.CustomerType.Id));
            parameters.Add(new SqlParameter("ConfigurationTypeId", (int)coverconfig.ConfigurationType));
            parameters.Add(new SqlParameter("StartDay", coverconfig.StartDay));
            parameters.Add(new SqlParameter("StartDate", coverconfig.StartDate));
            parameters.Add(new SqlParameter("StartTime", coverconfig.StartTime));
            parameters.Add(new SqlParameter("EndDay", coverconfig.EndDay));
            parameters.Add(new SqlParameter("EndDate", coverconfig.EndDate));
            parameters.Add(new SqlParameter("EndTime", coverconfig.EndTime));
            parameters.Add(new SqlParameter("Printer", coverconfig.Printer));

            return Convert.ToInt64(DataAccess.Helper.ExecuteScalar("CoverConfiguration_Insert", parameters));
        }

        public void UpdateCoverConfiguration(CoverConfiguration coverconfig)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", coverconfig.Id));
            parameters.Add(new SqlParameter("Name", coverconfig.Name));
            parameters.Add(new SqlParameter("Code", coverconfig.Code));
            parameters.Add(new SqlParameter("CustomerTypeId", coverconfig.CustomerType.Id));
            parameters.Add(new SqlParameter("ConfigurationTypeId", (int)coverconfig.ConfigurationType));
            parameters.Add(new SqlParameter("StartDay", coverconfig.StartDay));
            parameters.Add(new SqlParameter("StartDate", coverconfig.StartDate));
            parameters.Add(new SqlParameter("StartTime", coverconfig.StartTime));
            parameters.Add(new SqlParameter("EndDay", coverconfig.EndDay));
            parameters.Add(new SqlParameter("EndDate", coverconfig.EndDate));
            parameters.Add(new SqlParameter("EndTime", coverconfig.EndTime));
            parameters.Add(new SqlParameter("Printer", coverconfig.Printer));

            DataAccess.Helper.ExecuteScalar("CoverConfiguration_Edit", parameters);
        }

        public List<CoverConfiguration> GetCovers()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("CoverConfiguration_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new CoverConfiguration()
                    {
                        Id = i.Field<Int64>("Id"),
                        Name = i.Field<String>("Name"),
                        Code = i.Field<String>("Code"),
                        CustomerType = new CustomerType() { Name = i.Field<String>("CustomerTypeName"), Id = i.Field<Int32>("CustomerTypeId") },
                        ConfigurationType = (Cover.Backend.CoverConfigurationType)i.Field<Int32>("ConfigurationTypeId"),
                        StartDay = i.Field<Int16?>("StartDay"),
                        StartDate = i.Field<DateTime?>("StartDate"),
                        StartTime = i.Field<TimeSpan>("StartTime"),
                        EndDay = i.Field<Int16?>("EndDay"),
                        EndDate = i.Field<DateTime?>("EndDate"),
                        EndTime = i.Field<TimeSpan>("EndTime"),
                        Printer = i.Field<String>("Printer"),
                    }).ToList();
        }

        public CoverConfiguration GetCoverConfigurationById(Int64 id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", id));
            var dt = DataAccess.Helper.ExecuteDataTable("CoverConfiguration_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new CoverConfiguration()
            {
                Id = id,
                Name = dt.Rows[0]["Name"].ToString(),
                Code = dt.Rows[0]["Code"].ToString(),
                CustomerType = new CustomerType() { Id = Convert.ToInt32(dt.Rows[0]["CustomerTypeId"]), Name = dt.Rows[0]["CustomerTypeName"].ToString() },
                ConfigurationType = (Cover.Backend.CoverConfigurationType)Convert.ToInt32(dt.Rows[0]["ConfigurationTypeId"]),
                StartDay = dt.Rows[0]["StartDay"] == DBNull.Value ? new Int16?() : Convert.ToInt16(dt.Rows[0]["StartDay"]),
                StartDate = dt.Rows[0]["StartDate"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(dt.Rows[0]["StartDate"]),
                StartTime = TimeSpan.Parse(dt.Rows[0]["StartTime"].ToString()),
                EndDay = dt.Rows[0]["EndDay"] == DBNull.Value ? new Int16?() : Convert.ToInt16(dt.Rows[0]["EndDay"]),
                EndDate = dt.Rows[0]["EndDate"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(dt.Rows[0]["EndDate"]),
                EndTime = TimeSpan.Parse(dt.Rows[0]["EndTime"].ToString()),
                Printer = dt.Rows[0]["Printer"].ToString(),
            };
        }

        public void CoverConfigurationDelete(Int64 coverId, Boolean active)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", coverId));
            parameters.Add(new SqlParameter("Active", active));
            DataAccess.Helper.ExecuteNonQuery("CoverConfiguration_Delete", parameters);
        }

    }
}
