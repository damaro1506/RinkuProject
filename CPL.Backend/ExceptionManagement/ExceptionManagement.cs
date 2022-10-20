using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace Cover.Backend.ExceptionManagement
{
    public class ExceptionManagement
    {

        public static ExceptionDetail Publish(Exception ex, Boolean NewThread)
        {
            var exDetail = new ExceptionDetail();
            if (ex.GetType() == typeof(CoverException))
            {
                exDetail.Message = ex.Message;
                exDetail.MessageType = 2;
            }
            else
            {
                exDetail.MessageType = 3;
                exDetail.Message = "Ocurrió un error en la aplicación.";
                var exceptionPublisher = new ExceptionFilePublisher();
                var additionalInfo = GetAdditionalInfo();
                exceptionPublisher.Publish(ex, additionalInfo);
            }
            return exDetail;
        }

        public static ExceptionDetail Publish(Exception ex)
        {
            return Publish(ex, true);
        }

        public static ExceptionDetail Publish(SqlException ex)
        {
            return Publish((Exception)ex);
        }

        public static NameValueCollection GetAdditionalInfo()
        {
            try
            {
                var additionalInfoService = new AdditionalInfo();
                return new NameValueCollection() { 
                    {"IP", additionalInfoService.GetLocalIPAddress()},
                    {"Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")},
                    {"MachineName", System.Environment.MachineName},
                    {"AppVersion", additionalInfoService.GetApplicationVersion()},
                    {"WindowsProcess", System.Diagnostics.Process.GetCurrentProcess().ProcessName}
                };
            }catch(Exception ex)
            {
                return new NameValueCollection();
            }
        }

        public class ExceptionDetail
        {
            public Guid? Id { get; set; }
            public Int32 MessageType { get; set; }
            public String Message { get; set; }
        }
    }
}
