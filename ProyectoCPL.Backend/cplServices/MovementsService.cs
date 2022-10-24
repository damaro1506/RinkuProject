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

        private MovementsRepository _repository;
        private MovementsRepository repository
        {
            get
            {
                if (_repository == null)
                    _repository = new MovementsRepository();
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        #endregion

        #region "Get_events"

        //public Movement GetByMovementId(Int32 id)
        //{
        //    return repository.GetBymovementId(id);
        //}

        public List<Movement> GetMovements()
        {
            return repository.GetMovements();
        }

        public List<Movement> GetMovementsByMonth(Int32 monthNumber)
        {
            return repository.GetMovements().Where(a => a.MonthNumber == monthNumber).ToList(); 
        }


        #endregion

        public void InsertMovement(Movement movement,Int64 employeeId)
        {
            //var movements = GetMovements();

            //if (employees.Where(a=> a.FirstName.ToLower().Trim() == employee.FirstName.ToLower().Trim()).Any() && employees.Where(a => a.SecondName.ToLower().Trim() == employee.SecondName.ToLower().Trim()).Any())
            //    throw new ProyectoCPL.Backend.ExceptionManagement.ProyectoCPLException("El nombre del empleado ya existe.");

            repository.CreateMovement(movement, employeeId);
        }

    }
}
