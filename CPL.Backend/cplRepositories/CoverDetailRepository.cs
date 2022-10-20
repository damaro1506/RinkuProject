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
    public class CoverDetailRepository
    {
        #region "Create_Events"

        public void CreateCoverDetail(CoverDetail coverDetail, ref SqlTransaction sqlTran)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverId", coverDetail.CoverId));
            parameters.Add(new SqlParameter("Quantity", coverDetail.Quantity));
            parameters.Add(new SqlParameter("CustomerType", coverDetail.CustomerType.Id));
            parameters.Add(new SqlParameter("UnitPrice", coverDetail.UnitPrice));
            parameters.Add(new SqlParameter("Subtotal", coverDetail.SubtotalQuantity));
            parameters.Add(new SqlParameter("Iva", coverDetail.IvaCalculatedQuantity));
            parameters.Add(new SqlParameter("Total", coverDetail.TotalCalculatedQuantity));
            parameters.Add(new SqlParameter("CoverConfigurationId", coverDetail.CoverConfiguration.Id));
            parameters.Add(new SqlParameter("ValidFrom", coverDetail.ValidFrom));
            parameters.Add(new SqlParameter("ValidTo", coverDetail.ValidTo));

            DataAccess.Helper.ExecuteNonQuery(sqlTran, "CoverDetail_Insert", parameters);
        }

        #endregion

        #region "Get_Events"

        public CoverDetail GetCoverDetailById(Guid id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", id));
            var dt = DataAccess.Helper.ExecuteDataTable("CoverDetail_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new CoverDetail()
            {
                Id = id,
                CustomerType = new CustomerType() { Id = Convert.ToInt32(dt.Rows[0]["CustomerType"]), Name = dt.Rows[0]["CustomerTypeName"].ToString() },
                Barcode = Convert.ToInt64(dt.Rows[0]["Barcode"]),
                ValidFrom = Convert.ToDateTime(dt.Rows[0]["ValidFrom"]),
                ValidTo = Convert.ToDateTime(dt.Rows[0]["ValidTo"])
            };
        }

        public List<CoverDetail> GetCoverDetails()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("CoverDetail_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new CoverDetail()
                    {
                        Id = i.Field<Guid>("CoverId"),
                        Quantity = i.Field<Int32>("Quantity"),
                        CustomerType = new CustomerType() { Id = i.Field<Int32>("CustomerType"), Name = i.Field<String>("CustomerTypeName") },
                        UnitPrice = i.Field<Decimal>("UnitPrice"),
                        Subtotal = i.Field<Decimal>("Subtotal"),
                        Iva = i.Field<Decimal>("Iva"),
                        Total = i.Field<Decimal>("Total"),
                        CoverConfiguration = new CoverConfiguration() { Id = i.Field<Int64>("CoverConfigurationId") },
                        ValidFrom = i.Field<DateTime>("ValidFrom"),
                        ValidTo = i.Field<DateTime>("ValidTo"),
                        Barcode = i.Field<Int64>("Barcode"),
                    }).ToList();
        }

        public CoverDetail GetCoverDetailByBarcode(Int64 barcode)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Barcode", barcode));

            var dt = DataAccess.Helper.ExecuteDataTable("CoverDetail_GetByBarcode", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new CoverDetail()
                    {
                        Id = new Guid(dt.Rows[0]["CoverId"].ToString()),
                        Quantity = Convert.ToInt32(dt.Rows[0]["Quantity"]),
                        CustomerType = new CustomerType() { Id = Convert.ToInt32(dt.Rows[0]["CustomerType"]), Name = dt.Rows[0]["CustomerTypeName"].ToString() },
                        UnitPrice = Convert.ToDecimal(dt.Rows[0]["UnitPrice"]),
                        Subtotal = Convert.ToDecimal(dt.Rows[0]["Subtotal"]),
                        Iva = Convert.ToDecimal(dt.Rows[0]["Iva"]),
                        Total = Convert.ToDecimal(dt.Rows[0]["Total"]),
                        CoverConfiguration = new CoverConfiguration() { Id = Convert.ToInt64(dt.Rows[0]["CoverConfigurationId"]) },
                        ValidFrom = Convert.ToDateTime(dt.Rows[0]["ValidFrom"]),
                        ValidTo = Convert.ToDateTime(dt.Rows[0]["ValidTo"]),
                        Barcode = barcode,
                    };
        }
        #endregion
    }
}
