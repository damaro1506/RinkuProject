using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class Payment
    {
        public Payment()
        {
            PaymentMethod = new PaymentMethod();
        }

        public Int64 Id { get; set; }
        public Guid CoverId { get; set; }
        public Decimal ReceivedAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Decimal TotalReceivedAmount { get; set; }
        public Decimal Change { get; set; }
        public Decimal TotalPaid { get; set; }
        public DateTime OperationDate { get; set; }  

    }
        
}
