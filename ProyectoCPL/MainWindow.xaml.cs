using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace ProyectoCPL
{
    
    public partial class MainWindow : Window
    {
        #region "Properties"
        private UserControls.EmployeesUC _employee_Window;
        private UserControls.EmployeesUC employee_Window
        {
            get
            {
                if (_employee_Window == null)
                    _employee_Window = new UserControls.EmployeesUC();
                return _employee_Window;
            }
            set
            {
                _employee_Window = value;
            }
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            //Window.GetWindow(this).Closing += MainWindow_Closing;
        }

        private void btnEmployees_Click(object sender, RoutedEventArgs e)
        {
            ShowUC(employee_Window);
        }

        private void btnMovements_Click(object sender, RoutedEventArgs e)
        {

        }

        #region "UserControl_Events"

        private void ShowUC(UserControl uc)
        {
            WindowContent.Content = null;
            WindowContent.Content = uc;
            //GC.Collect();
        }

        #endregion
    }
}
