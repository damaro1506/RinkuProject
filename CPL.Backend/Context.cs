using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cover.Backend.BL;

namespace Cover.Backend
{
    public class Context
    {
        #region "Properties"

        public static Cover.Backend.Entities.User User { get; set; }
        public static Cover.Backend.Entities.Terminal Terminal { get; set; }

        #endregion

        #region "Terminal_Events"

        public static String TerminalName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        #endregion

        #region "OperationDate"

        public static DateTime OperationDate
        {
            get
            {
                var service = new CashRegisterOperationService();
                var cashRegisterOperation = service.GetLastOpenCashRegisterOperationByTerminal();
                if (cashRegisterOperation == null)
                    return DateTime.Now.Date;
                else
                    return cashRegisterOperation.OperationDate;
            }
        }

        #endregion

        #region "ClosureCopies_Events"

        public static String CopiesClosure_Z
        {
            get
            {
                return "1";
            }
        }

        public static String CopiesClosure_Global
        {
            get
            {
                return "1";
            }
        }

        #endregion
    }
}
