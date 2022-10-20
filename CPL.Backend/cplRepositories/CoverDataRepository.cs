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
    public class CoverDataRepository
    {
        #region "Create_Events"

        public void CreateCoverData(CoverData coverData,ref SqlTransaction sqlTran)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", coverData.Id));
            parameters.Add(new SqlParameter("CoverDate", coverData.CoverDate));
            parameters.Add(new SqlParameter("CoverQuantity", coverData.CoverQuantity));
            parameters.Add(new SqlParameter("Subtotal", coverData.Subtotal));
            parameters.Add(new SqlParameter("Iva", coverData.Iva));
            parameters.Add(new SqlParameter("Total", coverData.Total));
            parameters.Add(new SqlParameter("CashierId", coverData.CashierId));
            parameters.Add(new SqlParameter("TerminalId", coverData.TerminalId)); 
            parameters.Add(new SqlParameter("OperationDate", coverData.OperationDate));
            DataAccess.Helper.ExecuteNonQuery(sqlTran, "CoverData_Insert", parameters);
        }

        #endregion

        #region "Get_Events"
        public CoverData GetCoverDataById(Guid id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("id", id));

            var ds = DataAccess.Helper.ExecuteDataSet("CoverData_Get", parameters);

            return ConvertDataSetToCoverDataList(ds).FirstOrDefault();
        }

        public List<CoverData> GetCoverDataByOperationDate(DateTime operationDate)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("operationDate", operationDate));

            var ds = DataAccess.Helper.ExecuteDataSet("CoverData_GetListByOperationDate", parameters);

            return ConvertDataSetToCoverDataList(ds);
        }

        public List<CoverData> GetCoverDataByRangeOfDate(DateTime startDate, DateTime endDate)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("StartDate", startDate));
            parameters.Add(new SqlParameter("EndDate", endDate));

            var ds = DataAccess.Helper.ExecuteDataSet("CoverData_GetListByRangeDate", parameters);

            return ConvertDataSetToCoverDataList(ds);
        }
        #endregion

        #region "ConvertData_Events"

        private List<CoverData> ConvertDataSetToCoverDataList(DataSet ds)
        {
            return (from i in ds.Tables[0].AsEnumerable()
                    select new CoverData()
                    {
                        Id = i.Field<Guid>("Id"),
                        CoverDate = i.Field<DateTime>("CoverDate"),
                        CoverQuantity = i.Field<Int32>("CoverQuantity"),
                        Subtotal = i.Field<Decimal>("Subtotal"),
                        Iva = i.Field<Decimal>("Iva"),
                        Total = i.Field<Decimal>("Total"),
                        CashierId = i.Field<Int64>("CashierId"),
                        TerminalId = i.Field<Int32>("TerminalId"),
                        OperationDate = i.Field<DateTime>("OperationDate"),
                        CoverDetails = (from cde in ds.Tables[1].AsEnumerable()
                                        where i.Field<Guid>("Id") == cde.Field<Guid>("CoverId")
                                        select new CoverDetail()
                                        {
                                            Id = cde.Field<Guid>("Id"),
                                            Quantity = cde.Field<Int32>("Quantity"),
                                            Barcode = cde.Field<Int64>("Barcode"),
                                            CustomerType = new CustomerType() { Id = cde.Field<Int32>("CustomerType"), Name = cde.Field<String>("CustomerTypeName") },
                                            UnitPrice = cde.Field<Decimal>("UnitPrice"),
                                            Subtotal = cde.Field<Decimal>("Subtotal"),
                                            Iva = cde.Field<Decimal>("Iva"),
                                            Total = cde.Field<Decimal>("Total"),
                                            ValidFrom = cde.Field<DateTime>("ValidFrom"),
                                            ValidTo = cde.Field<DateTime>("ValidTo"),
                                            CoverConfiguration = new CoverConfiguration() { Id = cde.Field<Int64>("CoverConfigurationId"), Name = cde.Field<String>("CoverConfigurationName"), StartDate = cde.Field<DateTime?>("StartDate"), StartDay = cde.Field<Int16?>("StartDay"), StartTime = cde.Field<TimeSpan>("CoverConfigurationStartTime"), EndDate = cde.Field<DateTime?>("EndDate"), EndDay = cde.Field<Int16?>("EndDay"), EndTime = cde.Field<TimeSpan>("CoverConfigurationFinalTime"), CustomerType = new CustomerType() { Id = cde.Field<Int32>("CustomerType"), Name = cde.Field<String>("CustomerTypeName") }, Printer = cde.Field<String>("Printer") },
                                        }).ToList(),
                        Payments = (from p in ds.Tables[2].AsEnumerable()
                                    where i.Field<Guid>("Id") == p.Field<Guid>("CoverId")
                                    select new Payment()
                                    {
                                        Id = p.Field<Int64>("Id"),
                                        CoverId = p.Field<Guid>("CoverId"),
                                        PaymentMethod = new PaymentMethod() { Id = p.Field<Int32>("PaymentMethodId"), Name = p.Field<String>("PaymentMethodName"), Equivalence = p.Field<Decimal>("PaymentMethodEquivalence") },
                                        ReceivedAmount = p.Field<Decimal>("ReceivedAmount"),
                                        TotalReceivedAmount = p.Field<Decimal>("TotalReceivedAmount"),
                                        TotalPaid = p.Field<Decimal>("TotalPaid"),
                                        Change = p.Field<Decimal>("Change"),
                                        OperationDate = p.Field<DateTime>("OperationDate"),
                                    }).ToList(),
                    }).ToList();
        }

        #endregion
    }

}
