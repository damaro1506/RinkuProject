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
    public class PaymentDetailRepository
    {
        
            public void CreatePaymentDetail(Payment paymentDetail)
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("CoverId", paymentDetail.CoverId));
               // parameters.Add(new SqlParameter("PaymentId", paymentDetail.PaymentId.Id));
                parameters.Add(new SqlParameter("Total", paymentDetail.Total));

                DataAccess.Helper.ExecuteNonQuery("PaymentDetail_Insert", parameters);
            }

      }
}
