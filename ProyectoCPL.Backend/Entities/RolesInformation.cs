using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCPL.Backend.Entities
{
    public class RolesInformation
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Decimal PayPerHour { get; set; }
        public Decimal PayPerDelivery { get; set; }
        public Decimal PayBonus { get; set; }
        public Int32 HoursPerDay { get; set; }
        public Int32 DaysPerWeek { get; set; }
        public Boolean Active { get; set; }
    }
}
