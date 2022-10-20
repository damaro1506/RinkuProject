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
    public class CustomerTypeService
    {

        #region "Properties"

        private CustomerTypeRepository _customerTypeRepository;
        private CustomerTypeRepository customerTypeRepository
        {
            get
            {
                if (_customerTypeRepository == null)
                    _customerTypeRepository = new CustomerTypeRepository();
                return _customerTypeRepository;
            }
            set
            {
                _customerTypeRepository = value;
            }
        }

        #endregion

        #region "Get_Events"

        public List<CustomerType> GetActiveCustomerTypes()
        {
            return GetCustomerTypes().Where(a=> a.Active).ToList();
        }

        public List<CustomerType> GetCustomerTypes()
        {
            return customerTypeRepository.GetCustomerTypes();
        }

        #endregion

    }
}
