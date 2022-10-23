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
    public class RoleRepository
    {
        public List<RolesInformation> GetRoles()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("RinkuRoles_GetAll", null);

            return (from i in dt.AsEnumerable()
                    select new RolesInformation()
                    {
                        Id = i.Field<Int32>("id"),
                        Name = i.Field<String>("roleName"),
                        PayPerHour = i.Field<Decimal>("payPerHour"),
                        PayPerDelivery = i.Field<Decimal>("payPerDelivery"),
                        PayBonus = i.Field<Decimal>("payBonus"),
                        HoursPerDay = i.Field<Int32>("hoursPerDay"),
                        DaysPerWeek = i.Field<Int32>("daysPerWeek"),
                        Active = i.Field<Boolean>("isActive"),
                    }).ToList();
        }
    }
}
