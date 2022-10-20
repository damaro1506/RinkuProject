using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class CashRegisterOperation
    {
        public Int64 Id { get; set; }
        public DateTime OperationDate { get; set; }
        public Int32 TerminalId { get; set; }
        public Int64 UserId { get; set; }
        public Decimal CashFund { get; set; }
        public DateTime CashFundDate { get; set; }
        public Int32 Status { get; set; }
        public DateTime? CashOutDate { get; set; }
    }
}
