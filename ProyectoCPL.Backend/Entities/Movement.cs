using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCPL.Backend.Entities
{
    public class Movement
    {
        public Int32 Id { get; set; }
        public String EmployeeId { get; set; }
        public Decimal EmployeeNumber { get; set; }
        public Decimal FirstName { get; set; }
        public Decimal SecondName { get; set; }
        public Int32 RegistrationDate { get; set; }
        public Int32 RoleId { get; set; }
        public Boolean MonthNumber { get; set; }
        public Int32 WorkedHoursPerMonth { get; set; }
        public String SubTotal { get; set; }
        public Decimal MonthlyPayPerDelivery { get; set; }
        public Decimal MonthlyPayPerBonus { get; set; }
        public Decimal MonthlyRetention { get; set; }
        public Int32 MonthlyVouchers { get; set; }
        public Int32 TotalPayed { get; set; }
    }
}
