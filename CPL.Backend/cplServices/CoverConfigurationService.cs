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
    public class CoverConfigurationService
    {
        #region "Properties"

        private CoverConfigurationRepository _coverconfigurationRepository;
        private CoverConfigurationRepository coverconfigurationRepository
        {
            get
            {
                if (_coverconfigurationRepository == null)
                    _coverconfigurationRepository = new CoverConfigurationRepository();
                return _coverconfigurationRepository;
            }
            set
            {
                _coverconfigurationRepository = value;
            }
        }

        #endregion

        public Int64 CreateCoverConfiguration(CoverConfiguration coverConfiguration, Decimal defaultPrice)
        {
            if (defaultPrice < 0)
                throw new Cover.Backend.ExceptionManagement.CoverException("El campo precio es requerido");

            var coverConfigurations = GetCoversConfiguration();

            if (coverConfigurations.Where(a=>a.Name.ToLower().Trim() == coverConfiguration.Name.ToLower().Trim() || a.Code.ToLower().Trim() == coverConfiguration.Code.ToLower().Trim()).Any())
                throw new Cover.Backend.ExceptionManagement.CoverException("Ya existe un producto con el mismo nombre o clave.");

            var id = coverconfigurationRepository.CreateCoverConfiguration(coverConfiguration);

            var coverConfigurationPriceService = new CoverConfigurationPriceService();
            coverConfigurationPriceService.InsertCoverConfigurationPrice(id, coverConfiguration.StartDay, coverConfiguration.StartDate, coverConfiguration.StartTime, coverConfiguration.EndDay, coverConfiguration.EndDate, coverConfiguration.EndTime, defaultPrice);

            return id;
        }

        public void UpdateCoverConfiguration(CoverConfiguration coverConfiguration)
        {
            var coverConfigurations = GetCoversConfiguration();

            if (coverConfigurations.Where(a => (a.Name.ToLower().Trim() == coverConfiguration.Name.ToLower().Trim() || a.Code.ToLower().Trim() == coverConfiguration.Code.ToLower().Trim()) && a.Id != coverConfiguration.Id).Any())
                throw new Cover.Backend.ExceptionManagement.CoverException("Ya existe un producto con el mismo nombre o clave.");

            coverconfigurationRepository.UpdateCoverConfiguration(coverConfiguration);
        }

        public List<CoverConfiguration> GetCoversConfiguration()
        {
            return coverconfigurationRepository.GetCovers();
        }

        public CoverConfiguration GetCoverConfigurationById(Int64 id)
        {
            return coverconfigurationRepository.GetCoverConfigurationById(id);
        }

        public void CoverDelete(Int64 coverId, Boolean active)
        {
            coverconfigurationRepository.CoverConfigurationDelete(coverId, active);
        }

    }
}
