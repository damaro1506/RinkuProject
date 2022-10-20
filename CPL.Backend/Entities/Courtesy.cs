using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class Courtesy
    {
        public Courtesy()
        {
            User = new User();
            CoverConfiguration = new CoverConfiguration();
            CoverConfigurations = new List<CoverConfiguration>();
        }
        public Int64 Id { get; set; }
        public Int32 Quantity { get; set; }
        public User User { get; set; }
        public CoverConfiguration CoverConfiguration { get; set; }
        public String Reason { get; set; }
        public DateTime CourtesyDate { get; set; }
        public Decimal Total { get; set; }
        public List<CoverConfiguration> CoverConfigurations { get; set; }
        
    }
}
