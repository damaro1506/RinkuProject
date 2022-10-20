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
    public class PaymentRepository
    {
        public void CreatePayment(Payment payment, ref SqlTransaction sqlTran)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("CoverId", payment.CoverId));
            parameters.Add(new SqlParameter("ReceivedAmount", payment.ReceivedAmount));
            parameters.Add(new SqlParameter("PaymentMethodId", payment.PaymentMethod.Id));
            parameters.Add(new SqlParameter("TotalReceivedAmount", payment.TotalReceivedAmount));
            parameters.Add(new SqlParameter("Change", payment.Change));
            parameters.Add(new SqlParameter("TotalPaid", payment.TotalReceivedAmount - payment.Change));
            parameters.Add(new SqlParameter("OperationDate", payment.OperationDate));

            DataAccess.Helper.ExecuteNonQuery(sqlTran, "PaymentDetail_Insert", parameters);
        }
    }
}
