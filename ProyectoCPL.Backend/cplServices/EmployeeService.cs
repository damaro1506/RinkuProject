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

        #region "Create_Events"

        public void CreateEmployee(Employee employee)
        {
            if (employee.RolesInformation.Id < 1)
                throw new Exception("Favor de seleccionar un rol");
            if (String.IsNullOrEmpty(employee.FirstName))
                throw new Exception("El nombre del empleado es requerido");
            if (String.IsNullOrEmpty(employee.SecondName))
                throw new Exception("El apellido del empleado es requerido");
            if (String.IsNullOrEmpty(employee.EmployeeNumber.ToString()))
                throw new Exception("El numero de empleado es requerido");

            var employees = GetActiveEmployees();
            if (employees.Where(a => a.EmployeeNumber == employee.EmployeeNumber).Any())
                throw new Exception("Existe un empleado con el numero de empleado indicado");

            if (employees.Where(a => a.FirstName.Trim() == employee.FirstName.Trim()).Any() && employees.Where(a => a.SecondName.Trim() == employee.SecondName.Trim()).Any())
                throw new Exception("Ya existe un empleado con el nombre completo proporcionado");

            repository.CreateEmployee(employee);
        }

        #endregion
        #region "Get_events"

        public Employee GetByEmployeeId(Int64 id)
        {
            return repository.GetByEmployeeId(id);
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

            repository.CreateEmployee(employee);
        }

    }
}
