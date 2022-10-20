using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace ProyectoCPL.Backend.ExceptionManagement
{
    public class ExceptionFilePublisher
    {
        public void Publish(Exception exception, NameValueCollection additionalInfo)
        {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(AsyncPublish), new PublishParameters(exception, additionalInfo));
        }

        public void AsyncPublish(Object state)
        {
            var st = (ProyectoCPL.Backend.ExceptionManagement.PublishParameters)state;
            var exception = st.Ex;
            var additionalInfo = new NameValueCollection(st.AdditionalInfo);

            if (additionalInfo == null)
                return;

            var logFileName = GetLogFileName();
            ValidateDirectory(System.IO.Path.GetDirectoryName(logFileName));
            var strInfo = WriteExceptionToStringBuilder(exception, additionalInfo);
            WriteToFile(strInfo.ToString(), logFileName);
        }

        public StringBuilder WriteExceptionToStringBuilder(Exception exception, NameValueCollection additionalInfo)
        {
            var strInfo = new StringBuilder();
            strInfo.AppendFormat("{0}General Information{0}", Environment.NewLine);
            strInfo.AppendFormat("{0}Additonal Info:", Environment.NewLine);
            foreach (String i in additionalInfo)
                strInfo.AppendFormat("{0}{1}: {2}", Environment.NewLine, i, additionalInfo.Get(i));
            strInfo.AppendFormat("{0}{0}Exception Information{0}{1}", Environment.NewLine, exception.ToString());
            return strInfo;
        }

        public void ValidateDirectory(String directory)
        {
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
        }

        public String GetLogFileName()
        {
            String logFileName = "";
                var fileInfo = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                logFileName = fileInfo.DirectoryName + "\\Logs\\ExceptionLog_{0:yyyyMMdd}.txt";
                fileInfo = null;
            return logFileName;
        }

        public void WriteToFile(String info, String fullPathLogFile)
        {
            var formatedFileName = "";
            System.IO.StreamWriter sw = null;
            System.IO.FileStream fs = null;
            try
            {
                formatedFileName = String.Format(fullPathLogFile, DateTime.Now);
                fs = System.IO.File.Open(formatedFileName, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
                sw = new System.IO.StreamWriter(fs);
                sw.Write(info);
                sw.WriteLine(Environment.NewLine + "------------------------------------------------------------------------------------------------------------------------");
                sw.Flush();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sw.BaseStream != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                if (fs != null)
                    fs.Close();
            }
        }
    }
}
