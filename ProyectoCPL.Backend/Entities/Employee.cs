using ProyectoCPL.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCPL.Backend.Entities
{
    public class Employee
    {
        public Int64 Id { get; set; }
        public Int64 EmployeeNumber { get; set; }
        public String FirstName { get; set; }
        public String SecondName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public RolesInformation RolesInformation { get; set; }
        public Boolean Active { get; set; }
    }
}
