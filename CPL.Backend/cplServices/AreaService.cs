using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;
using Cover.Backend.ExceptionManagement;

namespace Cover.Backend.BL
{
    public class AreaService
    {
        #region "Properties"

        private AreaRepository _repository;
        private AreaRepository repository
        {
            get
            {
                if (_repository == null)
                    _repository = new AreaRepository();
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        #endregion

        #region "Get_events"
        
        public Area GetById(Int32 id)
        {
            return repository.GetById(id);
        }

        public List<Area> GetActiveAreas()
        {
            return repository.GetAreas().Where(a=> a.Active).ToList();
        }

        public List<Area> GetAreas()
        {
            return repository.GetAreas();
        }

        public List<Area> GetAreasByCoverConfigurationId(Int64 coverConfigurationId)
        {
            return repository.GetAreasByCoverConfigurationId(coverConfigurationId);
        }

        #endregion

        public void InsertArea(String name)
        {
            var areas = GetActiveAreas();

            if (areas.Where(a=> a.Name.ToLower().Trim() == name.ToLower().Trim()).Any())
                throw new Cover.Backend.ExceptionManagement.CoverException("El nombre del área ya existe.");

            repository.InsertArea(name);
        }

        public void UpdateArea(String name, Boolean active, Int32 id)
        {
            var areas = GetActiveAreas();

            if (areas.Where(a => a.Name.ToLower().Trim() == name.ToLower().Trim() && a.Id != id).Any())
                throw new Cover.Backend.ExceptionManagement.CoverException("El nombre del área ya existe.");

            repository.UpdateArea(name, active, id);
        }
    }
}
