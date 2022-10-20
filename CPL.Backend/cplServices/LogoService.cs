using Cover.Backend.Entities;
using Cover.Backend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.BL
{
    public class LogoService
    {
        #region "Properties"

        private LogoRepository _imagesRepository;
        private LogoRepository imagesRepository
        {
            get
            {
                if (_imagesRepository == null)
                    _imagesRepository = new LogoRepository();
                return _imagesRepository;
            }
            set
            {
                _imagesRepository = value;
            }
        }

        #endregion

        #region "Save_Events"

        public void SaveLogo(Logo logo)
        {
            imagesRepository.SaveLogo(logo);
        }

        #endregion

        #region "Get_Events"

        public Logo GetLogo()
        {
            return imagesRepository.GetLogo();
        }

        #endregion
    }
}
