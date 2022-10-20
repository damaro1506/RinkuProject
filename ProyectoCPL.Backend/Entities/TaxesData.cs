using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCPL.Backend.Entities
{
    public class TaxesData
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Decimal TaxDiscount { get; set; }
        public Decimal SecondaryDiscount { get; set; }
        public Boolean Active { get; set; }
    }
}
