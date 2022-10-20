using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace ProyectoCPL.Backend.ExceptionManagement
{
    public class PublishParameters
    {
        public System.Exception Ex { get; set; }
        public NameValueCollection AdditionalInfo { get; set; }

        public PublishParameters(System.Exception ex, NameValueCollection additionalInfo)
        {
            Ex = ex;
            AdditionalInfo = additionalInfo;
        }
    }
}
