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
    public class CoverConfigurationPriceRepository
    {
        public List<CoverConfigurationPrice> GetCoverConfigurationPrices(Int64 coverConfigurationId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
            var dt = DataAccess.Helper.ExecuteDataTable("CoverConfigurationPrice_GetList", parameters);

            return (from i in dt.AsEnumerable()
                    select new CoverConfigurationPrice()
                    {
                        Id = i.Field<Int64>("Id"),
                        CoverConfigurationId = i.Field<Int64>("CoverConfigurationId"),
                        StartDay = i.Field<Int16?>("StartDay"),
                        StartDate = i.Field<DateTime?>("StartDate"),
                        StartTime = i.Field<TimeSpan>("StartTime"),
                        EndDay = i.Field<Int16?>("EndDay"),
                        EndDate = i.Field<DateTime?>("EndDate"),
                        EndTime = i.Field<TimeSpan>("EndTime"),
                        Price = i.Field<Decimal>("Price"),
                    }).ToList();
        }

        public void InsertCoverConfigurationPrice(Int64 coverConfigurationId, Int16? startDay, DateTime? startDate, TimeSpan startTime,Int16? endDay, DateTime? endDate, TimeSpan endTime,  Decimal price)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
            parameters.Add(new SqlParameter("StartDay", startDay));
            parameters.Add(new SqlParameter("StartDate", startDate));
            parameters.Add(new SqlParameter("StartTime", startTime));
            parameters.Add(new SqlParameter("EndDay", endDay));
            parameters.Add(new SqlParameter("EndDate", endDate));
            parameters.Add(new SqlParameter("EndTime", endTime));
            parameters.Add(new SqlParameter("Price", price));
            DataAccess.Helper.ExecuteNonQuery("CoverConfigurationPrice_Insert", parameters);
        }

        public void DeleteCoverConfigurationPrice(Int64 id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", id));
            DataAccess.Helper.ExecuteNonQuery("CoverConfigurationPrice_Delete", parameters);
        }

        public void DeleteCoverConfigurationPrices(Int64 coverConfigurationId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
            DataAccess.Helper.ExecuteNonQuery("CoverConfigurationPrice_DeleteAll", parameters);
        }

    }
}
