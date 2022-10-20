using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;

namespace Cover.Backend.Handlers
{
    public  class SystemSettingsSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            try
            {
                var dt = DataAccess.Helper.ExecuteDataTable("SystemSettings_GetAll", null);
                var nvc = new System.Collections.Specialized.NameValueCollection();
                foreach (System.Data.DataRow dr in dt.Rows)
                    nvc.Add(dr["SystemSettingId"].ToString(), dr["Value"].ToString());
                return nvc;
            }
            catch (Exception ex)
            {
                Cover.Backend.ExceptionManagement.ExceptionManagement.Publish(ex);
                throw ex;
            }
        }
    }
}
