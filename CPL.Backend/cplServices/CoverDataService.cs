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
    public class CoverDataService
    {
        #region "Properties"

        private CoverDataRepository _coverdataRepository;
        private CoverDataRepository coverdataRepository
        {
            get
            {
                if (_coverdataRepository == null)
                    _coverdataRepository = new CoverDataRepository();
                return _coverdataRepository;
            }
            set
            {
                _coverdataRepository = value;
            }
        }

        #endregion

        #region "Create_Events"

        public void CreateCoverData(CoverData coverData, ref SqlTransaction sqlTran)
        {
            coverdataRepository.CreateCoverData(coverData, ref sqlTran);
        }

        #endregion

        #region "Get_Events"

        public List<CoverData> GetCoverDataByOperationDate(DateTime operationDate)
        {
            return coverdataRepository.GetCoverDataByOperationDate(operationDate);
        }

        public List<CoverData> GetCoverDataByRangeOfDate(DateTime startDate, DateTime endDate)
        {
            return coverdataRepository.GetCoverDataByRangeOfDate(startDate, endDate);
        }

        public CoverData GetCoverDataById(Guid id)
        {
            return coverdataRepository.GetCoverDataById(id);
        }

        #endregion

    }
}
