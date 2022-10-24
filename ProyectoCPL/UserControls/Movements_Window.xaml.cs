using Microsoft.VisualBasic;
using Projects.Commons;
using Projects.Commons.Windows;
using ProyectoCPL.Backend;
using ProyectoCPL.Backend.cplServices;
using ProyectoCPL.Backend.Entities;
using ProyectoCPL.Backend.ExceptionManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
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

    public partial class Movements_Window : Window
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

        private TaxesDataService _taxesDataService;
        private TaxesDataService taxesDataService
        {
            get
            {
                if (_taxesDataService == null)
                    _taxesDataService = new TaxesDataService();
                return _taxesDataService;
            }
            set
            {
                _taxesDataService = value;
            }
        }

        private VoucherService _voucherService;
        private VoucherService voucherService
        {
            get
            {
                if (_voucherService == null)
                    _voucherService = new VoucherService();
                return _voucherService;
            }
            set
            {
                _voucherService = value;
            }
        }

        public Int64 employeeId { get; set; }
        Employee gEmployee = new Employee();
        List<Month> months = new List<Month>();
        public delegate void OnSuccessEventHandler();
        public event OnSuccessEventHandler OnSuccess;

        #endregion
        public Movements_Window()
        {
            InitializeComponent();
        }

        private void ExitButtonwW_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadComboMonths();
            if (employeeId == 0)
            {
                this.lbTitle.Content = "Crear movimiento";
            }
            else
            {
                //Codigo por si se requiere editar un movimiento

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

        private void LoadComboMonths()
        {
            LoadMonths();
            foreach (var month in months)
                cmbMonth.Items.Add(month.Name);
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gEmployee.Id != 0)
                {
                    var monthNumber = this.cmbMonth.SelectedIndex + 1;
                    if (String.IsNullOrEmpty(this.txtQuantityOfDeliveries.Text))
                        throw new ProyectoCPLException("Favor de capturar la cantidad de entregas");

                    var quantityOfDeliveries = Convert.ToInt32(this.txtQuantityOfDeliveries.Text);
                    //var hoursDiscounted = Convert.ToInt32(this.txtHoursDiscounted.Text);  Se puede agregar un descuento de horas.
                    var movement = new Movement();
                    movement.QuantityOfDeliveries = quantityOfDeliveries;
                    movement.MonthNumber = monthNumber;
                    var payPerHour = gEmployee.RolesInformation.PayPerHour;
                    var weeksInMonth = 4;
                    var workedHoursPerMonth = (gEmployee.RolesInformation.HoursPerDay * gEmployee.RolesInformation.DaysPerWeek) * weeksInMonth; // Aqui se resolveria el tema del descuento de horas
                    movement.WorkedHoursPerMonth = workedHoursPerMonth;
                    var monthlyPayPerDelivery = gEmployee.RolesInformation.PayPerDelivery * quantityOfDeliveries;
                    movement.MonthlyPayPerDelivery = monthlyPayPerDelivery;
                    decimal monthlyPayPerBonus = 0;
                    if (gEmployee.RolesInformation.PayBonus != 0)
                        monthlyPayPerBonus = gEmployee.RolesInformation.PayBonus * quantityOfDeliveries;

                    movement.MonthlyPayPerBonus = monthlyPayPerBonus;
                    var taxName = "ISR";
                    var taxISR = taxesDataService.GetTaxesDataByName(taxName);

                    var subTotal = (workedHoursPerMonth * payPerHour) + monthlyPayPerDelivery + monthlyPayPerBonus;
                    movement.SubTotal = subTotal;
                    var voucherName = "Vales de despensa";
                    var monthlyVouchers = voucherService.GetVoucherByName(voucherName);
                    var voucherPayed = subTotal * monthlyVouchers.Percent;
                    var limitBaseISR = 10000;

                    Decimal monthlyRetention = 0;

                    if (subTotal > limitBaseISR)
                        monthlyRetention = (taxISR.TaxDiscount + taxISR.SecondaryDiscount) * subTotal;
                    else
                        monthlyRetention = taxISR.TaxDiscount * subTotal;

                    movement.MonthlyRetention = monthlyRetention;
                    movement.MonthlyVouchers = voucherPayed;

                    var total = subTotal - monthlyRetention;
                    movement.TotalPayed = total;

                    movementService.InsertMovement(movement, gEmployee.Id);
                }
                else
                {
                    throw new ProyectoCPLException("No se puede guardar un movimiento sin empleado.");
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

        private void LoadEmployeeByName()
        {
            var captureEmployee = txtEmployee.Text;

            gEmployee = employeeService.GetActiveEmployeeByName(captureEmployee);
            LabelsUpdate(gEmployee);
        }

        private void LabelsUpdate(Employee employee)
        {
            var employeeName = "";
            var role = "";
            Int64 employeeNumber = 0;

            employeeName = employee.FirstName + " " + employee.SecondName;
            role = employee.RolesInformation.Name;
            employeeNumber = employee.EmployeeNumber;

            this.lbEmployeeNumber.Content = employeeNumber.ToString();
            this.lbName.Content = employeeName;
            this.lbRol.Content = role;
            this.txtEmployee.Text = "";

        }
        private void txtEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtEmployee.Text))
            {
                try
                {
                    if (e.Key == Key.Return)
                    {
                        CleanLabels();
                        LoadEmployeeByName();
                    }
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

        private void CleanLabels()
        {
            this.lbEmployeeNumber.Content = "";
            this.lbName.Content = "";
            this.lbRol.Content = "";
        }

        private void txtQuantityOfDeliveries_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                ProjectsHelper.BeginFadeOut(parent);
                var messageUC = new Info_Window(String.Format("Solo valores numéricos"), MessageType.message);
                messageUC.ShowDialog();
                ProjectsHelper.BeginFadeIn(parent);
            }
        }
    }
}
