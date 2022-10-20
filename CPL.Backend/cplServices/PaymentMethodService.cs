using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;

namespace Cover.Backend.BL
{
    public class PaymentMethodService
    {
        #region "Properties"

        private PaymentMethodRepository _paymentTypeRepository;
        private PaymentMethodRepository paymentTypeRepository
        {
            get
            {
                if (_paymentTypeRepository == null)
                    _paymentTypeRepository = new PaymentMethodRepository();
                return _paymentTypeRepository;
            }
            set
            {
                _paymentTypeRepository = value;
            }
        }

        #endregion

        #region "Get_Events"

        public List<PaymentMethod> GetPaymentMethods()
        {
            return paymentTypeRepository.GetPaymentMethods();
        }

        public List<PaymentMethod> GetActivePaymentMethods()
        {
            return paymentTypeRepository.GetPaymentMethods();
        }

        #endregion

    }
}
