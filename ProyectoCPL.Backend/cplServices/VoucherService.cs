using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ProyectoCPL.Backend.cplRepositories;
using ProyectoCPL.Backend.Entities;
using System.Data;

namespace ProyectoCPL.Backend.cplServices
{
    public class VoucherService
    {
        #region "Properties"

        private VoucherRepository _voucherRepository;
        private VoucherRepository voucherRepository
        {
            get
            {
                if (_voucherRepository == null)
                    _voucherRepository = new VoucherRepository();
                return _voucherRepository;
            }
            set
            {
                _voucherRepository = value;
            }
        }

        #endregion

        #region "Get_Events"

        public PantryVouchers GetVoucherByName(String voucherName)
        {
            return voucherRepository.GetVoucherByName(voucherName);
        }

        #endregion

    }
}
