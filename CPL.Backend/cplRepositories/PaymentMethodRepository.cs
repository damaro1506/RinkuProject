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
    public class PaymentMethodRepository
    {
        public List<PaymentMethod> GetPaymentMethods()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("PaymentMethod_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new PaymentMethod()
                    {
                        Id = i.Field<Int32>("Id"),
                        Name = i.Field<String>("Name"),
                        Active = i.Field<Boolean>("Active"),
                        Equivalence = i.Field<Decimal>("Equivalence"),
                        PaymentTypeId = i.Field<Int32>("PaymentTypeId"),
                    }).ToList();
        }
    }
}
