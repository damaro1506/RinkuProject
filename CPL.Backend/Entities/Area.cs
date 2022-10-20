using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class Area
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Boolean Active { get; set; }
        public Int64 CoverConfigurationId { get; set; }
    }
}
