using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;
using Cover.Backend.BL;
using System.Data.SqlClient;

namespace Cover.Backend.BL
{
    public class PaymentService
    {
        #region "Properties"

        private PaymentRepository _paymentRepository;
        private PaymentRepository paymentRepository
        {
            get
            {
                if (_paymentRepository == null)
                    _paymentRepository = new PaymentRepository();
                return _paymentRepository;
            }
            set
            {
                _paymentRepository = value;
            }
        }

        #endregion

        #region "Create_Events"

        public void CreatePayment(Payment payment, ref SqlTransaction sqlTran)
        {
            paymentRepository.CreatePayment(payment, ref sqlTran);
        }

        #endregion

    }
}
