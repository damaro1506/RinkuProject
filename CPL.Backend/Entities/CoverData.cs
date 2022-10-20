using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class CoverData
    {
        public CoverData()
        {
            CoverDetails = new List<CoverDetail>();
            Payments = new List<Payment>();
        }

        public Guid Id { get; set; }
        public DateTime CoverDate { get; set; }
        public Int32 CoverQuantity { get; set; }
        public Decimal Subtotal { get; set; }
        public Decimal Iva { get; set; }
        public Decimal Total { get; set; }
        public Int64 CashierId { get; set; } 
        public Int32 TerminalId { get; set; } 
        public DateTime OperationDate { get; set; }
        public List<Payment> Payments { get; set; }
        public List<CoverDetail> CoverDetails { get; set; }
    }
}
