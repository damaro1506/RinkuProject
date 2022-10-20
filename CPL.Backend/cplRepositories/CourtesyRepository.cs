using Cover.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Cover.Backend.Repositories
{
    public class CourtesyRepository
    {
        #region "Create_Events"
        public void CreateCourtesy(Courtesy courtesy)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Quantity", courtesy.Quantity));
            parameters.Add(new SqlParameter("UserId", courtesy.User.Id));
            parameters.Add(new SqlParameter("CoverConfigurationId", courtesy.CoverConfiguration.Id));
            parameters.Add(new SqlParameter("Reason", courtesy.Reason));
            parameters.Add(new SqlParameter("CourtesyDate", courtesy.CourtesyDate));
            parameters.Add(new SqlParameter("Total", courtesy.Total));
            DataAccess.Helper.ExecuteNonQuery("Courtesy_Insert", parameters);
        }
        #endregion

        #region "Get_Events"

        public List<Courtesy> GetCourtesysByRangeOfDate(DateTime startDate, DateTime endDate)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("StartDate", startDate));
            parameters.Add(new SqlParameter("EndDate", endDate));

            var ds = DataAccess.Helper.ExecuteDataSet("Courtesy_GetListByRangeDate", parameters);

            return (from i in ds.Tables[0].AsEnumerable()
                    select new Courtesy()
                    {
                        Id = i.Field<Int64>("Id"),
                        Quantity = i.Field<Int32>("Quantity"),
                        User = new User() { Id = i.Field<Int64>("UserId"), Name = i.Field<String>("UserName") },
                        CoverConfiguration = new CoverConfiguration() { Id = i.Field<Int64>("CoverConfigurationId"), CustomerType = new CustomerType() { Name = i.Field<String>("CustomerTypeName") }  },
                        CourtesyDate = i.Field<DateTime>("CourtesyDate"),
                        Reason = i.Field<String>("Reason"),
                        Total = i.Field<Decimal>("Total"),
                        CoverConfigurations = (from u in ds.Tables[1].AsEnumerable()
                                               where i.Field<Int64>("CoverConfigurationId") == u.Field<Int64>("Id")
                                               select new CoverConfiguration()
                                               {
                                                   Id = u.Field<Int64>("Id"),
                                                   StartTime = u.Field<TimeSpan>("StartTime"),
                                                   EndTime = u.Field<TimeSpan>("EndTime"),
                                                   CustomerType = new CustomerType() { Name = u.Field<String>("CustomerTypeName") },
                                                   Printer = u.Field<String>("Printer"),
                                               }).ToList()
                    }).ToList();
        }

        #endregion
    } 
}
