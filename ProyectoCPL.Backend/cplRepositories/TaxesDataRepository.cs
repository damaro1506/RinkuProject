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
    public class TaxesDataRepository
    {
        public TaxesData GetTaxesDataByName(String name)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("taxName", name));
            var dt = DataAccess.Helper.ExecuteDataTable("RinkuTaxes_GetISRDataByName", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new TaxesData()
            {
                Id = Convert.ToInt32(dt.Rows[0]["id"]),
                Name = dt.Rows[0]["taxName"].ToString(),
                TaxDiscount = Convert.ToDecimal(dt.Rows[0]["baseTax"].ToString()),
                SecondaryDiscount = Convert.ToDecimal(dt.Rows[0]["secondaryTax"].ToString()),
                Active = Convert.ToBoolean(dt.Rows[0]["isActive"]),
            };
        }
    }
}
