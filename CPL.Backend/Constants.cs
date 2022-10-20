using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend
{
    public enum CustomerTypeC
    {
        Male = 1,
        Female = 2
    }

    public enum Roles
    {
        Cashier = 1,
        Manager = 2
    }

    public enum PaymentTypeC
    {
        Cash = 1,
        DebitCard = 2,
        CreditCard = 3
    }

    public enum CashRegisterOperationStatus
    {
        Open = 1,
        Closed = 2
    }

    public enum ClosureType
    {
        Z = 1,
        Global = 2
    }

    public enum CoverConfigurationType
    {
        General = 1,
        Event = 2
    }

}
