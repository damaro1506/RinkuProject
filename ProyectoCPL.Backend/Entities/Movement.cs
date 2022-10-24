using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCPL.Backend.Entities
{
    public class Movement
    {
        public Int64 Id { get; set; }
        public Employee Employee { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Int32 MonthNumber { get; set; }
        public Int32 QuantityOfDeliveries { get; set; }
        public Int32 WorkedHoursPerMonth { get; set; }
        public Decimal SubTotal { get; set; }
        public Decimal MonthlyPayPerDelivery { get; set; }
        public Decimal MonthlyPayPerBonus { get; set; }
        public Decimal MonthlyRetention { get; set; }
        public Decimal MonthlyVouchers { get; set; }
        public Decimal TotalPayed { get; set; }
    }
}
