using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Projects.Commons;
using System.Windows;
using System;
using System.Data;
using System.Data.SqlClient;
using ProyectoCPL.Backend;
using ProyectoCPL.Backend.DataAccess;
using ProyectoCPL.Backend.cplServices;
using System.Data.Common;
using ProyectoCPL.Backend.Entities;

namespace ProyectoCPL
{
    public partial class EmployeeWindow : Window
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

        Helper connection = new Helper();

        #endregion
        public EmployeeWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var employee = new Employee();

            //validacion de datos!
            employee.EmployeeNumber = Convert.ToInt64(numeroEmpleado.Text.ToString().Trim());
            employee.FirstName = nombreEmpleado.Text.ToString().Trim();
            employee.SecondName = apellidoEmpleado.Text.ToString().Trim();
            var employeeRoleId = 0;

            if (rbDriver.IsChecked.Value)
                employeeRoleId = (int)Roles.Driver;
            else {
                if (rbCarrier.IsChecked.Value)
                    employeeRoleId = (int)Roles.Carrier;
                else
                    employeeRoleId = (int)Roles.Assistant;
            }

            employee.RolesInformation = new RolesInformation() { Id = employeeRoleId };

            SqlConnection sqlConn = null;
            SqlTransaction sqlTran = null;

            

            try
            {

                sqlConn = new SqlConnection(ProyectoCPL.Backend.DataAccess.Helper.cplCS);
                sqlConn.Open();
                sqlTran = sqlConn.BeginTransaction();

                employeeService.InsertEmployee(employee, ref sqlTran);

                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                var exDetail = ProyectoCPL.Backend.ExceptionManagement.ExceptionManagement.Publish(ex);
                //StoryBoardHelper.BeginFadeOut(parent);
                //var messageUC = new Message_Window(String.Format(exDetail.Message), MessageType.message);
                //messageUC.ShowDialog();
                //StoryBoardHelper.BeginFadeIn(parent);
                return;
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
        }

        
    }
}
