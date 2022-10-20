using Cover.Backend.Entities;
using Cover.Backend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.BL
{
    public class CourtesyService
    {
        #region "Properties"

        private CourtesyRepository _courtesyRepository;
        private CourtesyRepository courtesyRepository
        {
            get
            {
                if (_courtesyRepository == null)
                    _courtesyRepository = new CourtesyRepository();
                return _courtesyRepository;;
            }
            set
            {
                _courtesyRepository = value;
            }
        }

        #endregion

        #region "Create_Events"

        public void CreateCourtesy(Courtesy courtesy)
        {
            if (String.IsNullOrEmpty(courtesy.User.Id.ToString()))
                throw new Exception("El usuario es requerido");
            courtesyRepository.CreateCourtesy(courtesy);
        }

        #endregion

        #region "Get_Events"

        public List<Courtesy> GetCourtesysByRangeOfDate(DateTime startDate, DateTime endDate)
        {
            return courtesyRepository.GetCourtesysByRangeOfDate(startDate, endDate);
        }

        #endregion
    }
}
