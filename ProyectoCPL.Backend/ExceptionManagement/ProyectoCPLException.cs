using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCPL.Backend.ExceptionManagement
{
    public class ProyectoCPLException : ApplicationException
    {
        public ProyectoCPLException(String message) : base (message)
        {
        }
    }
}
