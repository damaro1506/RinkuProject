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
using System.Globalization;
using ProyectoCPL.Backend.Entities;

namespace ProyectoCPL.UserControls
{
    /// <summary>
    /// Lógica de interacción para EmployeesUC.xaml
    /// </summary>
    public partial class MovementsUC : UserControl
    {
        #region Properties

        List<Month> months = new List<Month>();

        private MovementsService _movementService;
        private MovementsService movementService
        {
            get
            {
                if (_movementService == null)
                    _movementService = new MovementsService();
                return _movementService;
            }
            set
            {
                _movementService = value;
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

        public MovementsUC()
        {
            InitializeComponent();
            LoadMonths();
        }

        #region "Loaded"

        private void MovementsControls_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboMonths();
            LoadMovements();
        }

        private void LoadMovements()
        {
            dgMovements.Items.Clear();
            var montNumber = cmbWMonth.SelectedIndex + 1;
            var movements = movementService.GetMovementsByMonth(montNumber);
            foreach (var movement in movements) 
            {
                movement.Employee.FirstName += " " + movement.Employee.SecondName;
                dgMovements.Items.Add(movement);
            }
        }

        private void LoadMonths()
        {

            var americanCulture = new CultureInfo("es-MX");
            var count = 1;
            foreach (var month in americanCulture.DateTimeFormat.MonthNames.Take(12))
            {
                months.Add(new Month { Id = count, Name = month });
                count++;
            }
        }

        private void LoadComboMonths()
        {
            foreach (var month in months)
                cmbWMonth.Items.Add(month.Name);
        }
        void frm_OnSuccess()
        {
            LoadMovements();
        }


        private void btnCreateMovement_Click(object sender, RoutedEventArgs e)
        {
            ProjectsHelper.BeginFadeOut(parent);
            var frm = new Movements_Window();
            frm.OnSuccess += frm_OnSuccess;
            frm.ShowDialog();
            frm = null;
            ProjectsHelper.BeginFadeIn(parent);
        }
        #endregion

        private void cmbWMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMovements();
        }
    }
}
