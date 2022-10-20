using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Data;


namespace ProyectoCPL.Backend.ExceptionManagement
{
    public class AdditionalInfo
    {

        public string GetLocalIPAddress()
        {
            String localIP = String.Empty;
            try
            {
                IPHostEntry host;
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                return localIP;
            }catch(Exception ex)
            {
                return localIP;
            }
        }

        public String GetApplicationVersion()
        {
            String Version = String.Empty;
            try
            {
                Version = AssemblyName.GetAssemblyName("ProyectoCPL.exe").Version.ToString();
                return Version;
            }catch(Exception ex)
            {
                return Version;
            }
        }

    }
}
