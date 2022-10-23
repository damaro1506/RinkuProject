using ProyectoCPL.Backend.cplServices;
using ProyectoCPL.Backend.DataAccess;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Projects.Commons.Windows;
using Projects.Commons;

namespace ProyectoCPL.UserControls
{
    /// <summary>
    /// Lógica de interacción para EmployeesUC.xaml
    /// </summary>
    public partial class EmployeesUC : UserControl
    {
        #region Properties
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
        //Helper connection = new Helper();

        #endregion

        public EmployeesUC()
        {
            InitializeComponent();
            LoadEmployees();
        }

        #region "Loaded"

        private void EmployeesControls_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            dgEmployees.Items.Clear();
            var employees = employeeService.GetActiveEmployees();
            foreach (var employee in employees)
            {
                employee.FirstName += " " + employee.SecondName; 
                dgEmployees.Items.Add(employee);
            }
                //dgEmployees.Items.Add(employee);
        }

        #endregion

        void frm_OnSuccess()
        {
            LoadEmployees();
        }

        private void btnCreateEmployee_Click(object sender, RoutedEventArgs e)
        {
            ProjectsHelper.BeginFadeOut(parent);
            var frm = new Employees_Window();
            frm.OnSuccess += frm_OnSuccess;
            frm.ShowDialog();
            frm = null;
            ProjectsHelper.BeginFadeIn(parent);
        }
    }
}
