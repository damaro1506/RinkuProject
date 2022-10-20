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
    public class CustomerTypeRepository
    {
        public List<CustomerType> GetCustomerTypes()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("CustomerType_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new CustomerType()
                    {
                        Id = i.Field<Int32>("Id"),
                        Name = i.Field<String>("Name"),
                        Active = i.Field<Boolean>("Active"),
                    }).ToList();
        }
    }
}
