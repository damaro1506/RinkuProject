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
    public class TerminalRepository
    {
        #region "Get_events"

        public Terminal GetTerminal(String name)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Name", name));
            var dt = DataAccess.Helper.ExecuteDataTable("Terminal_Get", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new Terminal()
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Name = name,
                
            };
        }

        public List<Terminal> GetTerminals()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("Terminal_GetList", null);

            return (from i in dt.AsEnumerable()
                    select new Terminal()
                    {
                        Id = i.Field<Int32>("Id"),
                        Name = i.Field<String>("Name"),
                        Active = i.Field<Boolean>("Active"),
                        
                    }).ToList();
        }

        #endregion
    }
}
