using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class CoverDetail
    {
        public Guid Id { get; set; }
        public Guid CoverId { get; set; }
        public Int32 Quantity { get; set; }
        public CustomerType CustomerType { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal Subtotal { get; set; }
        public Decimal Iva { get; set; }
        public Decimal Total { get; set; }
        public CoverConfiguration CoverConfiguration { get; set; }
        public Int64 Barcode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public CoverDetail()
        {
            CoverConfiguration = new CoverConfiguration();
            CustomerType = new CustomerType();
        }

        public Decimal SubtotalCalculated
        {
            get
            {
                decimal ivaPct = 16;
                return TotalCalculated / (1 + (ivaPct / 100));
            }
        }

        public Decimal IvaCalculated
        {
            get
            {
                return TotalCalculated - SubtotalCalculated;
            }
        }

        public Decimal TotalCalculated
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }

        public Decimal SubtotalQuantity
        {
            get
            {
                decimal ivaPct = 16;
                return UnitPrice / (1 + (ivaPct / 100));
            }
        }

        public Decimal IvaCalculatedQuantity
        {
            get
            {
                return UnitPrice - SubtotalQuantity;
            }
        }

        public Decimal TotalCalculatedQuantity
        {
            get
            {
                return UnitPrice * Quantity;
            }
        }
    }
}
