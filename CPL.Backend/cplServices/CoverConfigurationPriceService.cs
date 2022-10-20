using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;
using Cover.Backend.ExceptionManagement;

namespace Cover.Backend.BL
{
    public class CoverConfigurationPriceService
    {
        #region "Properties"

        private CoverConfigurationPriceRepository _repository;
        private CoverConfigurationPriceRepository repository
        {
            get
            {
                if (_repository == null)
                    _repository = new CoverConfigurationPriceRepository();
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        #endregion

        public List<CoverConfigurationPrice> GetCoverConfigurationPrices(Int64 coverConfigurationId)
        {
            return repository.GetCoverConfigurationPrices(coverConfigurationId);
        }

        public void InsertCoverConfigurationPrice(Int64 coverConfigurationId, Int16? startDay, DateTime? startDate, TimeSpan startTime, Int16? endDay, DateTime? endDate, TimeSpan endTime, Decimal price)
        {
            repository.InsertCoverConfigurationPrice(coverConfigurationId, startDay, startDate, startTime, endDay, endDate, endTime, price);
        }

        public void DeleteCoverConfigurationPrice(Int64 id)
        {
            repository.DeleteCoverConfigurationPrice(id);
        }

        public void DeleteCoverConfigurationPrices(Int64 coverConfigurationId)
        {
            repository.DeleteCoverConfigurationPrices(coverConfigurationId);
        }

    }
}
