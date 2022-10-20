using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.ExceptionManagement
{
    public class CoverException : ApplicationException
    {
        public CoverException(String message) : base (message)
        {
        }
    }
}
