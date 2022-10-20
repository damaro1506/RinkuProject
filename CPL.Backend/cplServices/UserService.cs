using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cover.Backend.Repositories;
using Cover.Backend.Entities;
using Cover.Backend.ExceptionManagement;

namespace Cover.Backend.BL
{
    public class UserService
    {
        #region "Properties"

        private UserRepository _userRepository;
        private UserRepository userRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository();
                return _userRepository;
            }
            set
            {
                _userRepository = value;
            }
        }

        #endregion

        #region "Create_Events"

        public void CreateUser(User user)
        {
            if (String.IsNullOrEmpty(user.Name))
                throw new Exception("El nombre del usuario es requerido");
            if (String.IsNullOrEmpty(user.Password))
                throw new Exception("La contraseña es requerida");

            var users = GetUsers();
            if (users.Where(a => a.Password == user.Password).Any())
                throw new Exception("Existe un usuario con la contraseña indicada");

            if (users.Where(a => a.Name.Trim() == user.Name.Trim()).Any())
                throw new Exception("Ya existe un usuario con el nombre proporcionado");

            userRepository.CreateUser(user);
        }

        #endregion

        #region "Update_Events"

        public void UpdateUser(User user)
        {
            if (String.IsNullOrEmpty(user.Name))
                throw new Exception("El nombre del usuario es requerido");
            if (String.IsNullOrEmpty(user.Password))
                throw new Exception("La contraseña es requerida");

            var users = GetUsers();
            if (users.Where(a => a.Password == user.Password && a.Id != user.Id).Any())
                throw new Exception("Existe un usuario con la contraseña indicada");

            if (users.Where(a => a.Name.Trim() == user.Name.Trim() && a.Id != user.Id).Any())
                throw new Exception("Ya existe un usuario con el nombre proporcionado");

            userRepository.UpdateUser(user);
        }

        public void ChangeStatus(Int64 userId, Boolean active)
        {
            userRepository.ChangeStatus(userId, active);
        }

        #endregion

        #region "Get_Events"

        public User GetUserById(Int64 id)
        {
            return userRepository.GetUserById(id);
        }

        public List<User> GetUsers()
        {
            return userRepository.GetUsers();
        }

        public List<User> GetActiveUsers()
        {
            return GetUsers().Where(a => a.Active).ToList();
        }

        public User GetUserByPassword(String password)
        {
            if (String.IsNullOrEmpty(password))
                throw new CoverException("La contraseña es requerida");

            var users = GetUsers();
            var user = users.Where(a => a.Password == password.Trim()).FirstOrDefault();

            if (user == null)
                throw new CoverException("No se encontró un usuario con la contraseña proporcionada");

            if (!user.Active)
                throw new CoverException("El usuario no está activo");

            return user;
        }

        #endregion

    }
}
