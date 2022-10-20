using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class PaymentDetail
    {
        public PaymentDetail()
        {
            PaymentId = new PaymentType();
        }

        public Guid CoverId { get; set; }
        public PaymentType PaymentId { get; set; }
        public Int64 Total { get; set; }
        
    }
}
