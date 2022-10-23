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

        public List<RolesInformation> GetRoles()
        {
            return roleRepository.GetRoles().Where(a => a.Active).ToList();
        }

        #endregion

    }
}
