using Cover.Backend.BL;
using Cover.Backend.Entities;
using Cover.POS.Controls;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS.ConfigurationUC
{
    public partial class CoverConfiguration_Window : Window
    {

        #region "Properties"

        private CoverConfigurationService _coverconfigurationService;
        private CoverConfigurationService coverconfigurationService
        {
            get
            {
                if (_coverconfigurationService == null)
                    _coverconfigurationService = new CoverConfigurationService();
                return _coverconfigurationService;
            }
            set
            {
                _coverconfigurationService = value;
            }
        }

        private CoverConfigurationAreaService _coverConfigurationAreaService;
        private CoverConfigurationAreaService coverConfigurationAreaService
        {
            get
            {
                if (_coverConfigurationAreaService == null)
                    _coverConfigurationAreaService = new CoverConfigurationAreaService();
                return _coverConfigurationAreaService;
            }
            set
            {
                _coverConfigurationAreaService = value;
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

        public Int64 coverConfigurationId { get; set; }
        public delegate void OnSuccessEventHandler();
        public event OnSuccessEventHandler OnSuccess;

        #endregion

        #region "Constructor"

        public CoverConfiguration_Window()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomerTypes();
            LoadPrinters();
            LoadDays(cmbStartDay);
            LoadDays(cmbEndDay);
            LoadAreas();

            if (coverConfigurationId == 0)
                this.lbTitle.Content = "Alta de producto";
            else
            {
                this.lbTitle.Content = "Edición de producto";
                var coverConfiguration = coverconfigurationService.GetCoverConfigurationById(coverConfigurationId);
                this.txtName.Text = coverConfiguration.Name;
                this.txtCode.Text = coverConfiguration.Code;
                this.txtPrice.Visibility = System.Windows.Visibility.Hidden;
                this.lbPrice.Visibility = System.Windows.Visibility.Hidden;
                this.cmbPrint.SelectedValue = coverConfiguration.Printer;
                this.cmbCustomerType.SelectedValue = coverConfiguration.CustomerType.Name;
                if (coverConfiguration.ConfigurationType == Backend.CoverConfigurationType.Event)
                {
                    rbEvent.IsChecked = true;
                    RbEvent_Checked();
                    this.dpStartDate.SelectedDate = coverConfiguration.StartDate;
                    this.dpEndDate.SelectedDate = coverConfiguration.EndDate;
                }
                else
                {
                    rbGeneral.IsChecked = true;
                    RbGeneral_Checked();
                    this.cmbStartDay.SelectedIndex = (int)coverConfiguration.StartDay;
                    this.cmbEndDay.SelectedIndex = (int)coverConfiguration.EndDay;
                }
                
                ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, coverConfiguration.StartTime.Hours, coverConfiguration.StartTime.Minutes,0);
                ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, coverConfiguration.EndTime.Hours, coverConfiguration.EndTime.Minutes, 0);
            }
        }

        private void LoadCustomerTypes()
        {
            cmbCustomerType.Items.Clear();
            var customerTypeService = new CustomerTypeService();
            var customerTypes = customerTypeService.GetActiveCustomerTypes();
            foreach (var customerType in customerTypes)
                cmbCustomerType.Items.Add(customerType.Name);
        }

        private void LoadPrinters()
        {
            cmbPrint.Items.Clear();
            cmbPrint.Items.Add("[NO IMPRIMIR]");
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                cmbPrint.Items.Add(printer);
        }

        private void LoadDays(ComboBox cmb)
        {
            cmb.Items.Clear();
            cmb.Items.Add("Domingo");
            cmb.Items.Add("Lunes");
            cmb.Items.Add("Martes");
            cmb.Items.Add("Miércoles");
            cmb.Items.Add("Jueves");
            cmb.Items.Add("Viernes");
            cmb.Items.Add("Sábado");
        }

        private void LoadAreas()
        {
            var areaService = new Cover.Backend.BL.AreaService();
            var areas = areaService.GetActiveAreas();
            var coverConfigurationAreas = new List<CoverConfigurationArea>();
            if (coverConfigurationId != 0)
                coverConfigurationAreas = coverConfigurationAreaService.GetCoverConfigurationAreas(coverConfigurationId);
            foreach (var area in areas)
            {
                if (coverConfigurationAreas.Where(a => a.AreaId == area.Id).Any())
                    ltbAreasAdded.Items.Add(area);
                else
                    ltbAreasToAdd.Items.Add(area);
            }
        }

        #endregion

        #region "TextBox_Events"

        private void txtPrice_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtPrice_GotFocus;
        }

        private void txtPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("Solo valores numéricos"), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
        }

        #endregion

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtName.Text))
                    throw new Cover.Backend.ExceptionManagement.CoverException("El campo Nombre es requerido.");

                if (String.IsNullOrEmpty(txtCode.Text))
                    throw new Cover.Backend.ExceptionManagement.CoverException("El campo Clave es requerido.");

                if (cmbCustomerType.SelectedItem == null)
                    throw new Cover.Backend.ExceptionManagement.CoverException("El campo Tipo de cliente es requerido.");

                if (coverConfigurationId == 0)
                {
                    if (txtPrice.Text == null || txtPrice.Text == "")
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Precio es requerido.");
                }

                if (cmbPrint.SelectedItem == null)
                    throw new Cover.Backend.ExceptionManagement.CoverException("El campo Impresora es requerida.");

                if (((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value == null || ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value == null)
                    throw new Cover.Backend.ExceptionManagement.CoverException("Debe indicar una hora inicio y una hora fin.");

                if (ltbAreasAdded.Items.Count == 0)
                    throw new Cover.Backend.ExceptionManagement.CoverException("Debe seleccionar al menos un área de acceso.");

                var startTime = ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value.Value.TimeOfDay;
                var endTime = ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value.Value.TimeOfDay;

                if (rbGeneral.IsChecked.Value)
                {
                    if (cmbStartDay.SelectedItem == null)
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Día inicial es requerido.");
                    if (cmbEndDay.SelectedItem == null)
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Día final es requerido.");
                    if (cmbStartDay.SelectedItem.ToString() == cmbStartDay.SelectedItem.ToString() && startTime == endTime)
                        throw new Cover.Backend.ExceptionManagement.CoverException("La fecha final no puede ser igual a la fecha inicial.");
                }
                else
                {
                    if (!dpStartDate.SelectedDate.HasValue)
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Fecha inicial es requerido.");
                    if (!dpEndDate.SelectedDate.HasValue)
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Fecha final es requerido.");

                    if (new DateTime(dpStartDate.SelectedDate.Value.Year, dpStartDate.SelectedDate.Value.Month, dpStartDate.SelectedDate.Value.Day, startTime.Hours, startTime.Minutes, startTime.Seconds) > new DateTime(dpEndDate.SelectedDate.Value.Year, dpEndDate.SelectedDate.Value.Month, dpEndDate.SelectedDate.Value.Day, endTime.Hours, endTime.Minutes, endTime.Seconds))
                        throw new Cover.Backend.ExceptionManagement.CoverException("La fecha final no puede ser menor a la fecha inicial.");
                }

                var coverConfiguration = new CoverConfiguration();
                coverConfiguration.Name = txtName.Text.ToString();
                coverConfiguration.Code = txtCode.Text.ToString();
                coverConfiguration.CustomerType = new CustomerType() { Id = cmbCustomerType.SelectedIndex + 1 };

                if (rbGeneral.IsChecked.Value)
                {
                    coverConfiguration.ConfigurationType = Cover.Backend.CoverConfigurationType.General;
                    coverConfiguration.StartDay = WBM.Common.Helper.CommonFunctions.GetDayNumber(cmbStartDay.SelectedItem.ToString());
                    coverConfiguration.EndDay = WBM.Common.Helper.CommonFunctions.GetDayNumber(cmbEndDay.SelectedItem.ToString());
                }
                else
                {
                    coverConfiguration.ConfigurationType = Cover.Backend.CoverConfigurationType.Event;
                    coverConfiguration.StartDate = dpStartDate.SelectedDate.Value;
                    coverConfiguration.EndDate = dpEndDate.SelectedDate.Value;
                }

                coverConfiguration.StartTime = TimeSpan.Parse(startTime.ToString());
                coverConfiguration.EndTime = TimeSpan.Parse(endTime.ToString());
                coverConfiguration.Printer = cmbPrint.SelectedItem.ToString();

                if (coverConfigurationId == 0)
                {
                    var defaultPrice = Convert.ToDecimal(this.txtPrice.Text);
                    coverConfigurationId = coverconfigurationService.CreateCoverConfiguration(coverConfiguration, defaultPrice);
                }
                else
                {
                    coverConfiguration.Id = coverConfigurationId;
                    coverconfigurationService.UpdateCoverConfiguration(coverConfiguration);
                }

                coverConfigurationAreaService.DeleteCoverConfigurationArea(coverConfigurationId);

                foreach (var area in ltbAreasAdded.Items)
                {
                    coverConfigurationAreaService.InsertCoverConfigurationArea(coverConfigurationId, ((Cover.Backend.Entities.Area)area).Id);
                }

                OnSuccess();
                this.Close();
            }
            catch (Exception ex)
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format(ex.Message), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RbGeneral_Checked();
        }

        private void RbGeneral_Checked()
        {
            if (this.dpStartDate == null)
                return;
            this.dpStartDate.Visibility = System.Windows.Visibility.Hidden;
            this.dpEndDate.Visibility = System.Windows.Visibility.Hidden;
            this.cmbStartDay.Visibility = System.Windows.Visibility.Visible;
            this.cmbEndDay.Visibility = System.Windows.Visibility.Visible;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            RbEvent_Checked();
        }

        private void RbEvent_Checked()
        {
            this.dpStartDate.Visibility = System.Windows.Visibility.Visible;
            this.dpEndDate.Visibility = System.Windows.Visibility.Visible;
            this.cmbStartDay.Visibility = System.Windows.Visibility.Hidden;
            this.cmbEndDay.Visibility = System.Windows.Visibility.Hidden;
        }

        private void ExitButtonw_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddArea_Click(object sender, RoutedEventArgs e)
        {
            if (ltbAreasToAdd.SelectedItem != null)
            {
                ltbAreasAdded.Items.Add(ltbAreasToAdd.SelectedItem);
                ltbAreasToAdd.Items.Remove(ltbAreasToAdd.SelectedItem);
            }
            else
            {
                if (ltbAreasToAdd.Items.Count == 0)
                    return;

                ltbAreasAdded.Items.Add(ltbAreasToAdd.Items[0]);
                ltbAreasToAdd.Items.Remove(ltbAreasToAdd.Items[0]);
            }
        }

        private void btnRemoveArea_Click(object sender, RoutedEventArgs e)
        {
            if (ltbAreasAdded.SelectedItem != null)
            {
                ltbAreasToAdd.Items.Add(ltbAreasAdded.SelectedItem);
                ltbAreasAdded.Items.Remove(ltbAreasAdded.SelectedItem);
            }
            else
            {
                if (ltbAreasAdded.Items.Count == 0)
                    return;

                ltbAreasToAdd.Items.Add(ltbAreasAdded.Items[ltbAreasAdded.Items.Count - 1]);
                ltbAreasAdded.Items.Remove(ltbAreasAdded.Items[ltbAreasAdded.Items.Count - 1]);
            }
        }

    }
}

