using Projects.Commons;
using Projects.Commons.Windows;
using ProyectoCPL.Backend.cplServices;
using ProyectoCPL.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProyectoCPL.UserControls
{
    
    public partial class Employees_Window : Window
    {
        #region "Properties"

        private Window _parent;
        private Window parent
        {
            get
            {
                if (_parent == null)
                    _parent = Window.GetWindow(this);
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        private RoleService _roleService;
        private RoleService roleService
        {
            get
            {
                if (_roleService == null)
                    _roleService = new RoleService();
                return _roleService;
            }
            set
            {
                _roleService = value;
            }
        }

        private EmployeeService _employeeService;
        private EmployeeService employeeService
        {
            get
            {
                if (_employeeService == null)
                    _employeeService = new EmployeeService();
                return _employeeService;
            }
            set
            {
                _employeeService = value;
            }
        }

        public Int64 employeeId { get; set; }
        public delegate void OnSuccessEventHandler();
        public event OnSuccessEventHandler OnSuccess;

        #endregion
        public Employees_Window()
        {
            InitializeComponent();
        }

        private void ExitButtonwW_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadRoles();
            if (employeeId == 0)
            {
                this.lbTitle.Content = "Alta de empleado";
            }
            else
            {
                //Codigo por si se requiere editar un Empleado

                //this.lbTitle.Content = "Edición de empleado";
                //var employee = employeeService.GetByEmployeeId(employeeId);
                //this.txtfirstName.Text = employee.FirstName;
                ////this.txtPassword.Password = user.Password;
                //for (var i = 0; i < cmbRol.Items.Count; i++)
                //{
                //    if (cmbRol.Items[i].ToString() == employee.RoleInformation.roleName)
                //        cmbRol.SelectedIndex = i;
                //}
            }

        }

        private void LoadRoles()
        {
            var roleNames = roleService.GetRoles();
            foreach (var role in roleNames)
                cmbRol.Items.Add(role.Name);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                employeeId = 0;
                if (employeeId == 0)
                {
                    var employeeFirstName = this.txtFirstName.Text;
                    var employeeSecondName = this.txtSecondName.Text;
                    var employeeNumber = Convert.ToInt32(this.txtEmployeeNumber.Text);
                    var roleid = this.cmbRol.SelectedItem;
                    var employee = new Employee();
                    employee.FirstName = employeeFirstName;
                    employee.SecondName = employeeSecondName;
                    employee.EmployeeNumber = employeeNumber;
                    employee.RolesInformation = new RolesInformation() { Id = cmbRol.SelectedIndex + 1 };
                    employeeService.CreateEmployee(employee);
                }
                else
                {
                    //Aqui actualizaremos un empleado de ser necesario

                    //var employee = employeeService.GetByEmployeeId(employeeId);
                    //employee.FirstName = this.txtFirstName.Text;
                    //employee.SecondName = this.txtSecondName.Text;
                    //employee.RolesInformation = new RolesInformation() { Id = cmbRol.SelectedIndex + 1 };
                    //employeeService.UpdateEmployee(employee);
                }
                OnSuccess();
                this.Close();


            }
            catch (Exception ex)
            {
                ProjectsHelper.BeginFadeOut(parent);
                var messageUC = new Info_Window(String.Format(ex.Message), MessageType.message);
                messageUC.ShowDialog();
                ProjectsHelper.BeginFadeIn(parent);
            }
        }

    }
}
