using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;
using Cover.Backend.BL;
using Cover.Backend.ExceptionManagement;
using System.Data.SqlClient;

namespace Cover.Backend.BL
{
    public class CoverDetailService
    {
        #region "Properties"

        private CoverDetailRepository _coverdetailRepository;
        private CoverDetailRepository coverdetailRepository
        {
            get
            {
                if (_coverdetailRepository == null)
                    _coverdetailRepository = new CoverDetailRepository();
                return _coverdetailRepository;
            }
            set
            {
                _coverdetailRepository = value;
            }
        }

        #endregion

        #region "Create_Events"

        public void CreateCoverDetail(CoverDetail coverDetail, ref SqlTransaction sqlTran)
        {
            coverdetailRepository.CreateCoverDetail(coverDetail, ref sqlTran);
        }

        #endregion

        #region "Get_Events"

        public List<CoverDetail> GetCoverDetails()
        {
            return coverdetailRepository.GetCoverDetails();
        }
        public CoverDetail GetCoverDetailById(Guid Id)
        {
            return coverdetailRepository.GetCoverDetailById(Id);
        }


        public CoverDetail GetCoverDetailByBarcode(Int64 barcode)
        {
            return coverdetailRepository.GetCoverDetailByBarcode(barcode);
        }

        #endregion
    }
}
