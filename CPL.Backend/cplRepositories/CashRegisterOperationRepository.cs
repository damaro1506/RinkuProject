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
    public class CashRegisterOperationRepository
    {
        #region "Create_Events"
        public void CreateCashRegisterOperation(CashRegisterOperation cashRegisterOperation)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("OperationDate", cashRegisterOperation.OperationDate));
            parameters.Add(new SqlParameter("TerminalId", cashRegisterOperation.TerminalId));
            parameters.Add(new SqlParameter("UserId", cashRegisterOperation.UserId));
            parameters.Add(new SqlParameter("CashFund", cashRegisterOperation.CashFund));
            parameters.Add(new SqlParameter("Status", cashRegisterOperation.Status));
            parameters.Add(new SqlParameter("CashOutDate", cashRegisterOperation.CashOutDate));
            DataAccess.Helper.ExecuteNonQuery("CashRegisterOperation_Insert", parameters);
        }
        #endregion

        #region "Get_Events"
        public CashRegisterOperation GetLastOpenCashRegisterOperationByTerminal()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("TerminalName", Cover.Backend.Context.TerminalName));
            parameters.Add(new SqlParameter("Status", (int)CashRegisterOperationStatus.Open));
            var dt = DataAccess.Helper.ExecuteDataTable("CashRegisterOperation_GetLastOpenByTerminal", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new CashRegisterOperation()
            {
                OperationDate = Convert.ToDateTime(dt.Rows[0]["OperationDate"]),
                TerminalId = Convert.ToInt32(dt.Rows[0]["TerminalId"]),
                UserId = Convert.ToInt64(dt.Rows[0]["UserId"]),
                CashFund = Convert.ToDecimal(dt.Rows[0]["CashFund"]),
                CashFundDate = Convert.ToDateTime(dt.Rows[0]["CashFundDate"]),
                Status = Convert.ToInt32(dt.Rows[0]["Status"]),
                CashOutDate = dt.Rows[0]["CashOutDate"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(dt.Rows[0]["CashOutDate"]),
            };
        }
        
        public List<CashRegisterOperation> GetCashRegisterOperations()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("CashRegisterOperation_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new CashRegisterOperation()
                    {
                        Id = i.Field<Int64>("Id"),
                        OperationDate = i.Field<DateTime>("OperationDate"),
                        TerminalId = i.Field<Int32>("TerminalId"),
                        UserId = i.Field<Int64>("UserId"),
                        CashFund = i.Field<Decimal>("CashFund"),
                        CashFundDate = i.Field<DateTime>("CashFundDate"),
                        Status = i.Field<Int32>("Status"),
                        CashOutDate = i.Field<DateTime?>("CashOutDate"),

                    }).ToList();
        }

        public CashRegisterOperation GetOpenCashRegisterOperationByStatus()
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("TerminalId", Cover.Backend.Context.Terminal.Id));
            parameters.Add(new SqlParameter("Status", (int)CashRegisterOperationStatus.Closed));
            var dt = DataAccess.Helper.ExecuteDataTable("CashRegisterOperation_GetByStatus", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new CashRegisterOperation()
            {
                OperationDate = Convert.ToDateTime(dt.Rows[0]["OperationDate"]),
                TerminalId = Convert.ToInt32(dt.Rows[0]["TerminalId"]),
                UserId = Convert.ToInt64(dt.Rows[0]["UserId"]),
                CashFund = Convert.ToDecimal(dt.Rows[0]["CashFund"]),
                CashFundDate = Convert.ToDateTime(dt.Rows[0]["CashFundDate"]),
                Status = Convert.ToInt32(dt.Rows[0]["Status"]),
                CashOutDate = dt.Rows[0]["CashOutDate"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(dt.Rows[0]["CashOutDate"]),
            };
        }
        #endregion

        #region "Update_Events"
        public void UpdateCashOutRegisterOperation(CashRegisterOperation cashRegisterOperation)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("TerminalId", cashRegisterOperation.TerminalId));
            parameters.Add(new SqlParameter("Status", cashRegisterOperation.Status));
            parameters.Add(new SqlParameter("CashOutDate", cashRegisterOperation.CashOutDate));
            DataAccess.Helper.ExecuteNonQuery("CashRegisterOperation_Update", parameters);
        }
        #endregion
    }
}
