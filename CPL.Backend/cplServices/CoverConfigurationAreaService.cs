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
    public class CoverConfigurationAreaService
    {
        #region "Properties"

        private CoverConfigurationAreaRepository _repository;
        private CoverConfigurationAreaRepository repository
        {
            get
            {
                if (_repository == null)
                    _repository = new CoverConfigurationAreaRepository();
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        #endregion

        public List<CoverConfigurationArea> GetCoverConfigurationAreas(Int64 coverConfigurationId)
        {
            return repository.GetCoverConfigurationAreas(coverConfigurationId);
        }

        public void InsertCoverConfigurationArea(Int64 coverConfigurationId, Int32 areaId)
        {
            repository.InsertCoverConfigurationArea(coverConfigurationId, areaId);
        }

        public void DeleteCoverConfigurationArea(Int64 coverConfigurationId)
        {
            repository.DeleteCoverConfigurationArea(coverConfigurationId);
        }

    }
}
