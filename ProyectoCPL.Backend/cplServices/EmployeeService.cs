using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ProyectoCPL.Backend.cplRepositories;
using ProyectoCPL.Backend.Entities;

namespace ProyectoCPL.Backend.cplServices
{
    public class EmployeeService
    {
        #region "Properties"

        private EmployeeRepository _repository;
        private EmployeeRepository repository
        {
            get
            {
                if (_repository == null)
                    _repository = new EmployeeRepository();
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        #endregion

        #region "Get_events"
        
        public Employee GetById(Int32 id)
        {
            return repository.GetById(id);
        }

        public List<Employee> GetActiveAreas()
        {
            return repository.GetAreas().Where(a=> a.Active).ToList();
        }

        public List<Employee> GetAreas()
        {
            return repository.GetAreas();
        }

        public List<Employee> GetAreasByCoverConfigurationId(Int64 coverConfigurationId)
        {
            return repository.GetAreasByCoverConfigurationId(coverConfigurationId);
        }

        #endregion

        public void InsertArea(String name)
        {
            var areas = GetActiveAreas();

            if (areas.Where(a=> a.FirstName.ToLower().Trim() == name.ToLower().Trim()).Any())
                throw new ProyectoCPL.Backend.ExceptionManagement.ProyectoCPLException("El nombre del empleado ya existe.");

            repository.InsertArea(name);
        }

        public void UpdateArea(String name, Boolean active, Int32 id)
        {
            var areas = GetActiveAreas();

            if (areas.Where(a => a.FirstName.ToLower().Trim() == name.ToLower().Trim() && a.Id != id).Any())
                throw new ProyectoCPL.Backend.ExceptionManagement.ProyectoCPLException("El nombre del empleado ya existe.");

            repository.UpdateArea(name, active, id);
        }
    }
}
