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
    public class MovementsRepository
    {
        #region "CreateEvents"
        public void CreateEmployee(Employee employee, ref SqlTransaction sqlTran)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("employeeNumber", employee.EmployeeNumber));
            parameters.Add(new SqlParameter("firstName", employee.FirstName));
            parameters.Add(new SqlParameter("secondName", employee.SecondName));
            parameters.Add(new SqlParameter("registrationDate", DateTime.Now.ToString()));
            parameters.Add(new SqlParameter("roleId", employee.RolesInformation.Id));

            DataAccess.Helper.ExecuteNonQuery(sqlTran, "RinkuEmployees_Insert", parameters);
        }

        #endregion

        #region "UpdateEvents"
        public void UpdateEmployee(Employee employee)
        {
            var parameters = new List<SqlParameter>();
            //agregar parametros para update
            DataAccess.Helper.ExecuteNonQuery("RinkuEmployees_Update", parameters);
        }

        #endregion

        #region "Get_events"

        public Employee GetById(Int32 id)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("Id", id));
            var dt = DataAccess.Helper.ExecuteDataTable("RinkuEmployees_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return new Employee()
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                FirstName = dt.Rows[0]["firstName"].ToString(),
                SecondName = dt.Rows[0]["secondName"].ToString(),
                //RegistrationDate
                Active = Convert.ToBoolean(dt.Rows[0]["Active"]),
            };
        }

        //public List<Employee> GetAreasByCoverConfigurationId(Int64 coverConfigurationId)
        //{
        //    var parameters = new List<SqlParameter>();
        //    parameters.Add(new SqlParameter("CoverConfigurationId", coverConfigurationId));
        //    var dt = DataAccess.Helper.ExecuteDataTable("GetAreasByCoverConfigurationId", parameters);
        //    return (from i in dt.AsEnumerable()
        //            select new Employee()
        //            {
        //                Id = i.Field<Int32>("Id"),
        //                FirstName = i.Field<String>("Name"),
        //                Active = i.Field<Boolean>("Active"),
        //                //CoverConfigurationId = i.Field<Int64>("CoverConfigurationId"),
        //            }).ToList();
        //}

        //public List<Employee> GetEmployees()
        //{
        //    var dt = DataAccess.Helper.ExecuteDataTable("RinkuEmployees_GetAll", null);

        //    return (from i in dt.AsEnumerable()
        //            select new Employee()
        //            {
        //                Id = i.Field<Int32>("Id"),
        //                EmployeeNumber = i.Field<Int64>("employeeNumber"),
        //FirstName = i.Field<String>("firstName"),
        //                SecondName = i.Field<String>("secondName"),
        //                RolesInformation = i.Field<Int32>("idRole"),
        //                Active = i.Field<Boolean>("Active"),
        //            }).ToList();
        //}

        public List<Employee> GetEmployees()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("RinkuEmployees_GetAll", null); 

            return (from i in dt.AsEnumerable()
                    select new Employee()
                    {
                        Id = i.Field<Int32>("Id"),
                        EmployeeNumber = i.Field<Int32>("employeeNumber"),
                        FirstName = i.Field<String>("firstName"),
                        SecondName = i.Field<String>("secondName"),
                        RegistrationDate = i.Field<DateTime>("registrationDate"),
                        LeaveDate = i.Field<DateTime?>("leaveDate"),
                        RolesInformation = new RolesInformation() { Id = i.Field<Int32>("id"), Name = i.Field<String>("roleName"), DaysPerWeek = i.Field<Int32>("daysPerWeek"), HoursPerDay = i.Field<Int32>("hoursPerDay"), PayPerDelivery = i.Field<Decimal>("payPerDelivery"), PayPerHour = i.Field<Decimal>("payPerHour"), PayBonus = i.Field<Decimal>("payBonus"), Active = i.Field<Boolean>("IsActive") },
                        Active = i.Field<Boolean>("Active"),

                    }).ToList();
        }

        #endregion


        
    }
}
