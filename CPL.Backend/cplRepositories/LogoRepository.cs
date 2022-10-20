using Cover.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing.Imaging;

namespace Cover.Backend.Repositories
{
    public class LogoRepository
    {
        public void SaveLogo(Logo logo)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Image", logo.Image));
            DataAccess.Helper.ExecuteNonQuery("Logo_Insert", parameters);
        }

        public Logo GetLogo()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("Logo_Get", null);

            if (dt.Rows.Count == 0)
                return null;

            return new Logo()
            {
                Image = dt.Rows[0]["Image"].ToString()
            };
        }
    }
}
