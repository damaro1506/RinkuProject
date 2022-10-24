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
    public class TaxesDataService
    {
        #region "Properties"

        private TaxesDataRepository _taxRepository;
        private TaxesDataRepository taxRepository
        {
            get
            {
                if (_taxRepository == null)
                    _taxRepository = new TaxesDataRepository();
                return _taxRepository;
            }
            set
            {
                _taxRepository = value;
            }
        }

        #endregion

        #region "Get_Events"

        public TaxesData GetTaxesDataByName(String taxName)
        {
            return taxRepository.GetTaxesDataByName(taxName);
        }

        #endregion

    }
}
