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
    public class MovementsService
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

        public List<Employee> GetActiveEmployees()
        {
            return repository.GetEmployees().Where(a => a.Active).ToList();
        }


        #endregion

        public void InsertEmployee(Employee employee, ref SqlTransaction sqlTran)
        {
            var employees = GetActiveEmployees();

            if (employees.Where(a=> a.FirstName.ToLower().Trim() == employee.FirstName.ToLower().Trim()).Any() && employees.Where(a => a.SecondName.ToLower().Trim() == employee.SecondName.ToLower().Trim()).Any())
                throw new ProyectoCPL.Backend.ExceptionManagement.ProyectoCPLException("El nombre del empleado ya existe.");

            repository.CreateEmployee(employee, ref sqlTran);
        }

    }
}
