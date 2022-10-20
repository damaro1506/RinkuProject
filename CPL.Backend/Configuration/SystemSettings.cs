using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;

namespace Cover.Backend.Configuration
{
    public class SystemSettings
    {
        #region "Properties"

        #region "Ticket_Header"

        public static Boolean Ticket_Header_CenterAlign
        {
            get
            {
                return ReadConfig<Boolean>(21, true, false);
            }
        }

        public static String Ticket_Header_Line01
        {
            get
            {
                return ReadConfig<String>(3, "", false);
            }
        }

        public static String Ticket_Header_Line02
        {
            get
            {
                return ReadConfig<String>(5, "", false);
            }
        }

        public static String Ticket_Header_Line03
        {
            get
            {
                return ReadConfig<String>(6, "", false);
            }
        }

        public static String Ticket_Header_Line04
        {
            get
            {
                return ReadConfig<String>(7, "", false);
            }
        }

        public static String Ticket_Header_Line05
        {
            get
            {
                return ReadConfig<String>(8, "", false);
            }
        }

        public static String Ticket_Header_Line06
        {
            get
            {
                return ReadConfig<String>(9, "", false);
            }
        }

        public static String Ticket_Header_Line07
        {
            get
            {
                return ReadConfig<String>(10, "", false);
            }
        }

        public static String Ticket_Header_Line08
        {
            get
            {
                return ReadConfig<String>(11, "", false);
            }
        }

        public static String Ticket_Header_Line09
        {
            get
            {
                return ReadConfig<String>(12, "", false);
            }
        }

        public static String Ticket_Header_Line10
        {
            get
            {
                return ReadConfig<String>(13, "", false);
            }
        }

        #endregion

        #region "Ticket_Footer"

        public static Boolean Ticket_Footer_CenterAlign
        {
            get
            {
                return ReadConfig<Boolean>(22, true, false);
            }
        }

        public static Boolean Virtual_Keyboard
        {
            get
            {
                return ReadConfig<Boolean>(23, true, false);
            }
        }

        public static String Ticket_Footer_Line01
        {
            get
            {
                return ReadConfig<String>(14, "", false);
            }
        }

        public static String Ticket_Footer_Line02
        {
            get
            {
                return ReadConfig<String>(15, "", false);
            }
        }

        public static String Ticket_Footer_Line03
        {
            get
            {
                return ReadConfig<String>(16, "", false);
            }
        }

        public static String Ticket_Footer_Line04
        {
            get
            {
                return ReadConfig<String>(17, "", false);
            }
        }

        public static String Ticket_Footer_Line05
        {
            get
            {
                return ReadConfig<String>(18, "", false);
            }
        }

        public static short ClosureZ_Copies
        {
            get
            {
                return ReadConfig<short>(1, 1, false);
            }
        }

        public static short ClosureGlobal_Copies
        {
            get
            {
                return ReadConfig<short>(2, 1, false);
            }
        }

        public static short TicketCopies
        {
            get
            {
                return ReadConfig<short>(4, 1, false);
            }
        }

        public static short MaxHoursToClosure
        {
            get
            {
                return ReadConfig<short>(24, 9, false);
            }
        }

        public static String Bracelet_FreeText_Line1
        {
            get
            {
                return ReadConfig<String>(25, "", false);
            }
        }

        public static String Bracelet_FreeText_Line2
        {
            get
            {
                return ReadConfig<String>(26, "", false);
            }
        }

        public static String Bracelet_FreeText_Line3
        {
            get
            {
                return ReadConfig<String>(27, "", false);
            }
        }


        #endregion

        #region "SystemSettings_Events"

        private const String SystemSettingsSection = "SystemSettings";

        public static T ReadConfig<T>(Int32 systemSettingId, T defaultValue, bool throwWhenNotFound)
        {
            return ReadConfig<T>(systemSettingId, defaultValue, throwWhenNotFound, SystemSettingsSection);
        }

        private static T ReadConfig<T>(Int32 systemSettingId, T defaultValue, bool throwWhenNotFound, string configSection)
        {
            var config = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection(configSection);
            string configValue;
            if (config == null)
                configValue = "";
            else
                configValue = config[systemSettingId.ToString()];
            
            if (string.IsNullOrEmpty(configValue))
            {
                if (throwWhenNotFound)
                    throw new ConfigurationErrorsException("Configuration value not found " + systemSettingId);
                return defaultValue;
            }
            else
            {
                T res;
                try
                {
                    res = (T)Convert.ChangeType(configValue, typeof(T));
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(String.Format("Invalid configuration value {0} for {1}", configValue, systemSettingId), ex);
                }
                return res;
            }
        }

        public static void Refresh()
        {
            System.Configuration.ConfigurationManager.RefreshSection(SystemSettingsSection);
        }

        public static void UpdateSystemSetting(Int32 systemSettingId, String value)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("SystemSettingId",systemSettingId),
                new SqlParameter("Value",value),
            };
            DataAccess.Helper.ExecuteNonQuery("SystemSettings_Update", parameters);
            Refresh();
        }

        #endregion
    }
}
        #endregion