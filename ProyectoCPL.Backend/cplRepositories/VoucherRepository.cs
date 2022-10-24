using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoCPL.Backend.Entities;
using System.Data.SqlClient;
using System.Data;


namespace ProyectoCPL.Backend.cplRepositories
{
    public class VoucherRepository
    {
        public PantryVouchers GetVoucherByName(String name)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("voucherName", name));
            var dt = DataAccess.Helper.ExecuteDataTable("RinkuTaxes_GetVoucherByName", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new PantryVouchers()
            {
                Id = Convert.ToInt32(dt.Rows[0]["id"]),
                Name = dt.Rows[0]["voucherName"].ToString(),
                Percent = Convert.ToDecimal(dt.Rows[0]["percentVoucher"].ToString()),
                Active = Convert.ToBoolean(dt.Rows[0]["isActive"]),
            };
        }
    }
}
