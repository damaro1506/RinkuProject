using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCPL.Backend.Entities
{
    public class PantryVouchers
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Decimal Percent { get; set; }
        public Boolean Active { get; set; }
    }
}
