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
        public void CreateMovement(Movement movement, Int64 employeeId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("employeeId", employeeId));
            parameters.Add(new SqlParameter("quantityOfDeliveries", movement.QuantityOfDeliveries));
            parameters.Add(new SqlParameter("registrationDate", DateTime.Now));
            parameters.Add(new SqlParameter("monthNumber", movement.MonthNumber));
            parameters.Add(new SqlParameter("monthlyWorkedHours", movement.WorkedHoursPerMonth));
            parameters.Add(new SqlParameter("subTotal", movement.SubTotal));
            parameters.Add(new SqlParameter("monthlyPayPerDeliveries", movement.MonthlyPayPerDelivery));
            parameters.Add(new SqlParameter("monthlyPayPerBonus", movement.MonthlyPayPerBonus));
            parameters.Add(new SqlParameter("monthlyRetention", movement.MonthlyRetention));
            parameters.Add(new SqlParameter("monthlyVouchers", movement.MonthlyVouchers));
            parameters.Add(new SqlParameter("TotalPayed", movement.TotalPayed));

            DataAccess.Helper.ExecuteNonQuery("RinkuMovementsControl_Insert", parameters);
        }

        #endregion

        #region "UpdateEvents"
        public void UpdateMovement(Movement movement)
        {
            var parameters = new List<SqlParameter>();
            //agregar parametros para update
            DataAccess.Helper.ExecuteNonQuery("RinkuMovementsControl_Update", parameters);
        }

        #endregion

        #region "Get_events"

        public List<Movement> GetMovements()
        {
            var dt = DataAccess.Helper.ExecuteDataTable("RinkuMovementsControl_GetAll", null);

            return (from i in dt.AsEnumerable()
                    select new Movement()
                    {
                        Id = i.Field<Int64>("Id"),
                        MonthNumber= i.Field<Int32>("monthNumber"),
                        WorkedHoursPerMonth = i.Field<Int32>("monthlyWorkedHours"),
                        SubTotal = i.Field<Decimal>("subTotal"),
                        RegistrationDate = i.Field<DateTime>("registrationDate"),
                        MonthlyPayPerDelivery = i.Field<Decimal>("monthlyPayPerDeliveries"),
                        MonthlyPayPerBonus = i.Field<Decimal>("monthlyPayPerBonus"),
                        MonthlyRetention = i.Field<Decimal>("monthlyRetention"),
                        MonthlyVouchers = i.Field<Decimal>("monthlyVouchers"),
                        TotalPayed = i.Field<Decimal>("TotalPayed"),
                        Employee = new Employee() { Id = i.Field<Int64>("employeeId"), FirstName = i.Field<String>("firstName"), SecondName = i.Field<String>("secondName"), EmployeeNumber = i.Field<Int64>("employeeNumber"),RolesInformation = new RolesInformation() { Id = i.Field<Int32>("roleId") },Active = i.Field<Boolean>("isActive") },

                    }).ToList();
        }

        //public Movement GetMovementById(Int32 id)
        //{
        //    var parameters = new List<SqlParameter>();
        //    parameters.Add(new SqlParameter("Id", id));
        //    var dt = DataAccess.Helper.ExecuteDataTable("RinkuEmployees_GetById", parameters);

        //    if (dt.Rows.Count == 0)
        //        return null;

        //    return new Movement()
        //    {
        //        Id = Convert.ToInt32(dt.Rows[0]["Id"]),
        //        FirstName = dt.Rows[0]["firstName"].ToString(),
        //        SecondName = dt.Rows[0]["secondName"].ToString(),
        //        //RegistrationDate
        //        Active = Convert.ToBoolean(dt.Rows[0]["Active"]),
        //    };
        //}


        //public List<Movement> GetMovements()
        //{
        //    var dt = DataAccess.Helper.ExecuteDataTable("RinkuMovementsControl_GetAll", null); 

        //    return (from i in dt.AsEnumerable()
        //            select new Movement()
        //            {
        //                Id = i.Field<Int32>("Id"),
        //                EmployeeNumber = i.Field<Int32>("employeeNumber"),
        //                FirstName = i.Field<String>("firstName"),
        //                SecondName = i.Field<String>("secondName"),
        //                RegistrationDate = i.Field<DateTime>("registrationDate"),
        //                LeaveDate = i.Field<DateTime?>("leaveDate"),
        //                RolesInformation = new RolesInformation() { Id = i.Field<Int32>("id"), Name = i.Field<String>("roleName"), DaysPerWeek = i.Field<Int32>("daysPerWeek"), HoursPerDay = i.Field<Int32>("hoursPerDay"), PayPerDelivery = i.Field<Decimal>("payPerDelivery"), PayPerHour = i.Field<Decimal>("payPerHour"), PayBonus = i.Field<Decimal>("payBonus"), Active = i.Field<Boolean>("IsActive") },
        //                Active = i.Field<Boolean>("Active"),

        //            }).ToList();
        //}

        #endregion



    }
}
