using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class PaymentMethod
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Boolean Active { get; set; }
        public Decimal Equivalence { get; set; }
        public Int32 PaymentTypeId { get; set; }
    }
}
