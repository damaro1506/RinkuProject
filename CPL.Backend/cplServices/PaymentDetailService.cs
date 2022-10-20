using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;
using Cover.Backend.BL;

namespace Cover.Backend.BL
{
    public class PaymentDetailService
    {
        #region "Properties"

        private PaymentDetailRepository _payentdetailRepository;
        private PaymentDetailRepository paymentdetailRepository
        {
            get
            {
                if (_payentdetailRepository == null)
                    _payentdetailRepository = new PaymentDetailRepository();
                return _payentdetailRepository;
            }
            set
            {
                _payentdetailRepository = value;
            }
        }

        #endregion

        public void CreatePaymentDetail(Payment paymentDetail)
        {
            paymentdetailRepository.CreatePaymentDetail(paymentDetail);

        }
    }
}
