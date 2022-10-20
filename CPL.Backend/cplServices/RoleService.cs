using Cover.Backend.Entities;
using Cover.Backend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.BL
{
    public class RoleService
    {
        #region "Properties"

        private RoleRepository _roleRepository;
        private RoleRepository roleRepository
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository();
                return _roleRepository;
            }
            set
            {
                _roleRepository = value;
            }
        }

        #endregion

        #region "Get_Events"

        public List<Role> GetRoles()
        {
            return roleRepository.GetRoles();
        }

        #endregion
    }
}
