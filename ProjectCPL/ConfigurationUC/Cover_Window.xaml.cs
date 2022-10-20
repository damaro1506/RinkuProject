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
    public partial class Cover_Window : UserControl
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

        public Int64 coverId { get; set; }
        
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
        
        #endregion

        #region "Constructor"

        public Cover_Window()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomerTypes();
            LoadPrinters();
            LoadCovers();
        }

        private void LoadCustomerTypes()
        {
            cmbCustomerType.Items.Clear();
            var customerTypeService = new CustomerTypeService();
            var customerTypes = customerTypeService.GetActiveCustomerTypes();
            foreach (var customerType in customerTypes)
                cmbCustomerType.Items.Add(customerType.Name);
        }

        private void LoadCovers()
        {
            dgCover.Items.Clear();
            var coversConfiguration = coverconfigurationService.GetCoversConfiguration();
            foreach (var cover in coversConfiguration)
                dgCover.Items.Add(cover);
        }

        private void LoadPrinters()
        {
            cmbPrint.Items.Clear();
            cmbPrint.Items.Add("[NO IMPRIMIR]");
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                cmbPrint.Items.Add(printer);
        }

        #endregion

        #region "Button_Events"

        public void AddCover_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //if (((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStart.txt).Value == null || ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpFinal.txt).Value == null)
            //    throw new Cover.Backend.ExceptionManagement.CoverException("Debe seleccionar un rango de horas.");

            //    var startTime = ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStart.txt).Value.Value.TimeOfDay;
            //    var endTime = ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpFinal.txt).Value.Value.TimeOfDay;

            //    if (cmbPrint.SelectedItem == null)
            //        throw new Cover.Backend.ExceptionManagement.CoverException("Debe seleccionar una impresora");

            //    var printer = this.cmbPrint.SelectedItem;
            //    var coverConfiguration = new CoverConfiguration();
            //    coverConfiguration.StartTime = TimeSpan.Parse(startTime.ToString());
            //    coverConfiguration.FinalTime = TimeSpan.Parse(endTime.ToString());

            //    if (txtPrice.Text == null|| txtPrice.Text == "")
            //        throw new Cover.Backend.ExceptionManagement.CoverException("Debe asignar un precio de cover");

            //    coverConfiguration.Price = Convert.ToDecimal(this.txtPrice.Text);
            //    coverConfiguration.Printer = printer.ToString();

            //    if (cmbCustomerType.SelectedItem == null)
            //        throw new Cover.Backend.ExceptionManagement.CoverException("Debe seleccionar un tipo de cliente");

            //    coverConfiguration.CustomerType = new CustomerType() { Id = cmbCustomerType.SelectedIndex + 1 };

            //    if (String.IsNullOrEmpty(txtValidHours.Text))
            //        throw new Cover.Backend.ExceptionManagement.CoverException("Debe asignar el tiempo valido del cover");

            //    coverConfiguration.ValidHours = Convert.ToInt32(txtValidHours.Text);

            //    if (String.IsNullOrEmpty(txtCode.Text))
            //        throw new Cover.Backend.ExceptionManagement.CoverException("Debe asignar una clave a la configuración");

            //    coverConfiguration.KeyCode = txtCode.Text.ToString();

            //    if (String.IsNullOrEmpty(txtCode.Text))
            //        throw new Cover.Backend.ExceptionManagement.CoverException("Debe asignar un nombre a la configuración");

            //    coverConfiguration.ConfigurationName = txtName.Text.ToString();

            //    if (endTime <= startTime)
            //        throw new Cover.Backend.ExceptionManagement.CoverException("La hora final no puede ser menor o igual a la hora de inicio");

            //    var daySelected = false;
            //    if (chMon.IsChecked.Value)
            //    {
            //        coverConfiguration.Day = 1;
            //        coverconfigurationService.CreateCoverConfiguration(coverConfiguration);
            //        daySelected = true;
            //    }
            //    if (chTue.IsChecked.Value)
            //    {
            //        coverConfiguration.Day = 2;
            //        coverconfigurationService.CreateCoverConfiguration(coverConfiguration);
            //        daySelected = true;
            //    }
            //    if (chWed.IsChecked.Value)
            //    {
            //        coverConfiguration.Day = 3;
            //        coverconfigurationService.CreateCoverConfiguration(coverConfiguration);
            //        daySelected = true;
            //    }
            //    if (chThu.IsChecked.Value)
            //    {
            //        coverConfiguration.Day = 4;
            //        coverconfigurationService.CreateCoverConfiguration(coverConfiguration);
            //        daySelected = true;
            //    }
            //    if (chFri.IsChecked.Value)
            //    {
            //        coverConfiguration.Day = 5;
            //        coverconfigurationService.CreateCoverConfiguration(coverConfiguration);
            //        daySelected = true;
            //    }
            //    if (chSat.IsChecked.Value)
            //    {
            //        coverConfiguration.Day = 6;
            //        coverconfigurationService.CreateCoverConfiguration(coverConfiguration);
            //        daySelected = true;
            //    }

            //    if (chSun.IsChecked.Value)
            //    {
            //        coverConfiguration.Day = 0;
            //        coverconfigurationService.CreateCoverConfiguration(coverConfiguration);
            //        daySelected = true;
            //    }

            //    if (!daySelected)
            //        throw new Cover.Backend.ExceptionManagement.CoverException("Es requerido seleccionar por lo menos un día");

            //    LoadCovers();
            //}
            //catch (Exception ex)
            //{
            //    StoryBoardHelper.BeginFadeOut(parent);
            //    var messageUC = new Message_Window(String.Format(ex.Message), MessageType.message);
            //    messageUC.ShowDialog();
            //    StoryBoardHelper.BeginFadeIn(parent);
            //}
        }

        private void btndeleteConfiguration_Click(object sender, RoutedEventArgs e)
        {
            var cover = ((CoverConfiguration)dgCover.SelectedItem);

            {
                coverconfigurationService.CoverDelete(cover.Id, false);
                LoadCovers();
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

    }
}

