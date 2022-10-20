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
    public partial class CoverPrices_Window : Window
    {

        #region "Properties"

        private CoverConfigurationService _coverconfigurationService;
        private CoverConfigurationService coverConfigurationService
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

        private CoverConfigurationPriceService _coverConfigurationPriceService;
        private CoverConfigurationPriceService coverConfigurationPriceService
        {
            get
            {
                if (_coverConfigurationPriceService == null)
                    _coverConfigurationPriceService = new CoverConfigurationPriceService();
                return _coverConfigurationPriceService;
            }
            set
            {
                _coverConfigurationPriceService = value;
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
        public Cover.Backend.CoverConfigurationType coverConfigurationType { get; set; }

        #endregion

        #region "Constructor"

        public CoverPrices_Window()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPrices();
            var coverConfiguration = coverConfigurationService.GetCoverConfigurationById(coverConfigurationId);
            if (coverConfiguration == null)
                this.Close();
            coverConfigurationType = coverConfiguration.ConfigurationType;
            lbTitle.Content = String.Format("Configuración de precios ({0})", coverConfiguration.Name);
            this.lbPeriod.Content = String.Format("Acceso de {0} a {1}", coverConfiguration.StartDateDescription, coverConfiguration.EndDateDescription);

            ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, coverConfiguration.StartTime.Hours, coverConfiguration.StartTime.Minutes, 0);
            ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, coverConfiguration.EndTime.Hours, coverConfiguration.EndTime.Minutes, 0);

            if (coverConfigurationType == Backend.CoverConfigurationType.General)
            {
                cmbStartDay.Visibility = System.Windows.Visibility.Visible;
                cmbEndDay.Visibility = System.Windows.Visibility.Visible;
                dpStartDate.Visibility = System.Windows.Visibility.Hidden;
                dpEndDate.Visibility = System.Windows.Visibility.Hidden;

                LoadDays(cmbStartDay);
                LoadDays(cmbEndDay);
                this.cmbStartDay.SelectedIndex = (int)coverConfiguration.StartDay;
                this.cmbEndDay.SelectedIndex = (int)coverConfiguration.EndDay;
                this.cmbStartDay.IsEnabled = false;
                tpStartTime.IsEnabled = false;
            }
            else
            {
                cmbStartDay.Visibility = System.Windows.Visibility.Hidden;
                cmbEndDay.Visibility = System.Windows.Visibility.Hidden;
                dpStartDate.Visibility = System.Windows.Visibility.Visible;
                dpEndDate.Visibility = System.Windows.Visibility.Visible;

                this.dpStartDate.SelectedDate = coverConfiguration.StartDate;
                this.dpEndDate.SelectedDate = coverConfiguration.EndDate;
            }
        }

        private void LoadPrices()
        {
            dgPrices.Items.Clear();
            var coversConfigurationPrices = coverConfigurationPriceService.GetCoverConfigurationPrices(coverConfigurationId);
            foreach (var price in coversConfigurationPrices)
                dgPrices.Items.Add(price);
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

        #endregion

        private void ExitButtonw_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddPrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value == null || ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value == null)
                    throw new Cover.Backend.ExceptionManagement.CoverException("Debe indicar una hora inicio y una hora fin.");

                var startTime = ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value.Value.TimeOfDay;
                var endTime = ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value.Value.TimeOfDay;

                Int16? startDay = null; Int16? endDay = null; DateTime? startDate = null; DateTime? endDate = null;

                if (coverConfigurationType == Backend.CoverConfigurationType.General)
                {
                    if (cmbStartDay.SelectedItem == null)
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Día inicial es requerido.");
                    if (cmbEndDay.SelectedItem == null)
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Día final es requerido.");

                    startDay = WBM.Common.Helper.CommonFunctions.GetDayNumber(cmbStartDay.SelectedItem.ToString());
                    endDay = WBM.Common.Helper.CommonFunctions.GetDayNumber(cmbEndDay.SelectedItem.ToString());
                }
                else
                {
                    if (!dpStartDate.SelectedDate.HasValue)
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Fecha inicial es requerido.");
                    if (!dpEndDate.SelectedDate.HasValue)
                        throw new Cover.Backend.ExceptionManagement.CoverException("El campo Fecha final es requerido.");

                    if (new DateTime(dpStartDate.SelectedDate.Value.Year, dpStartDate.SelectedDate.Value.Month, dpStartDate.SelectedDate.Value.Day, startTime.Hours, startTime.Minutes, startTime.Seconds) > new DateTime(dpEndDate.SelectedDate.Value.Year, dpEndDate.SelectedDate.Value.Month, dpEndDate.SelectedDate.Value.Day, endTime.Hours, endTime.Minutes, endTime.Seconds))
                        throw new Cover.Backend.ExceptionManagement.CoverException("La fecha final no puede ser menor a la fecha inicia.");

                    startDate = dpStartDate.SelectedDate.Value;
                    endDate = dpEndDate.SelectedDate.Value;
                }

                if (String.IsNullOrEmpty(txtPrice.Text))
                    throw new Cover.Backend.ExceptionManagement.CoverException("El campo Precio es requerido.");

                var coverConfigurationPrice = new CoverConfigurationPrice();
                coverConfigurationPrice.CoverConfigurationId = coverConfigurationId;
                coverConfigurationPrice.StartDay = startDay;
                coverConfigurationPrice.StartDate = startDate;
                coverConfigurationPrice.StartTime = startTime;
                coverConfigurationPrice.EndDay = endDay;
                coverConfigurationPrice.EndDate = endDate;
                coverConfigurationPrice.EndTime = endTime;
                coverConfigurationPrice.Price = Convert.ToDecimal(txtPrice.Text);

                var coverConfiguration = coverConfigurationService.GetCoverConfigurationById(coverConfigurationId);

                foreach (CoverConfigurationPrice item in dgPrices.Items)
                {
                    if (coverConfigurationType == Backend.CoverConfigurationType.General)
                    {
                        if (item.EndDay == coverConfiguration.EndDay && item.EndTime == coverConfiguration.EndTime)
                            throw new Cover.Backend.ExceptionManagement.CoverException("La configuración de precios está completa");
                    }
                    else
                    {
                        if (item.EndDate == coverConfiguration.EndDate && item.EndTime == coverConfiguration.EndTime)
                            throw new Cover.Backend.ExceptionManagement.CoverException("La configuración de precios está completa");
                    }
                }

                
                if (coverConfigurationType == Backend.CoverConfigurationType.General)
                {
                    if (!(endDay == coverConfiguration.EndDay && endTime == coverConfiguration.EndTime))
                    {
                        var lastSunday = Cover.Backend.Helper.Dates.GetLastSunday();
                        var date = new DateTime(lastSunday.AddDays((double)endDay).Year, lastSunday.AddDays((double)endDay).Month, lastSunday.AddDays((double)endDay).Day, endTime.Hours, endTime.Minutes, 0).AddMinutes(1);

                        this.cmbStartDay.SelectedIndex = (short)date.DayOfWeek;
                        ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                        this.cmbEndDay.SelectedIndex = (int)coverConfiguration.EndDay;
                        ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, coverConfiguration.EndTime.Hours, coverConfiguration.EndTime.Minutes, 0);
                    }
                }
                else
                {
                    if (!(endDate == coverConfiguration.EndDate && endTime == coverConfiguration.EndTime))
                    {
                        var date = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, endTime.Hours, endTime.Minutes, 0).AddMinutes(1);
                        this.dpStartDate.SelectedDate = date;
                        this.dpEndDate.SelectedDate = coverConfiguration.EndDate;
                        ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                        ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value = new DateTime(coverConfiguration.EndDate.Value.Year, coverConfiguration.EndDate.Value.Month, coverConfiguration.EndDate.Value.Day, coverConfiguration.EndTime.Hours, coverConfiguration.EndTime.Minutes, 0);
                    }
                }

                dgPrices.Items.Add(coverConfigurationPrice);
                txtPrice.Text = "";

                if (dgPrices.Items.Count > 0 && coverConfigurationType == Backend.CoverConfigurationType.Event)
                {
                    this.dpStartDate.IsEnabled = false;
                    tpStartTime.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format(ex.Message), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
        }

        private void btnDeletePrice_Click(object sender, RoutedEventArgs e)
        {
            if (dgPrices.Items.Count > 0)
            {
                dgPrices.Items.Remove(GetLastItem());

                var coverConfiguration = coverConfigurationService.GetCoverConfigurationById(coverConfigurationId);
                if (dgPrices.Items.Count > 0)
                {
                    var lastItem = GetLastItem();

                    if (coverConfigurationType == Backend.CoverConfigurationType.General)
                    {
                        var lastSunday = Cover.Backend.Helper.Dates.GetLastSunday();
                        var date = new DateTime(lastSunday.AddDays((double)lastItem.EndDay).Year, lastSunday.AddDays((double)lastItem.EndDay).Month, lastSunday.AddDays((double)lastItem.EndDay).Day, lastItem.EndTime.Hours, lastItem.EndTime.Minutes, 0).AddMinutes(1);

                        this.cmbStartDay.SelectedIndex = (short)date.DayOfWeek;
                        ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                        this.cmbEndDay.SelectedIndex = (int)coverConfiguration.EndDay;
                        ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, coverConfiguration.EndTime.Hours, coverConfiguration.EndTime.Minutes, 0);
                    }
                    else
                    {
                        var date = new DateTime(lastItem.EndDate.Value.Year, lastItem.EndDate.Value.Month, lastItem.EndDate.Value.Day, lastItem.EndTime.Hours, lastItem.EndTime.Minutes, 0).AddMinutes(1);
                        this.dpStartDate.SelectedDate = date;
                        ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                        this.dpEndDate.SelectedDate = coverConfiguration.EndDate;
                        ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value = new DateTime(coverConfiguration.EndDate.Value.Year, coverConfiguration.EndDate.Value.Month, coverConfiguration.EndDate.Value.Day, coverConfiguration.EndTime.Hours, coverConfiguration.EndTime.Minutes, 0);
                    }
                }
                else
                {
                    ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpStartTime.txt).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, coverConfiguration.StartTime.Hours, coverConfiguration.StartTime.Minutes, 0);
                    ((Xceed.Wpf.Toolkit.Primitives.DateTimePickerBase)tpEndTime.txt).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, coverConfiguration.EndTime.Hours, coverConfiguration.EndTime.Minutes, 0);
                    if (coverConfigurationType == Backend.CoverConfigurationType.General)
                    {
                        this.cmbStartDay.SelectedIndex = (int)coverConfiguration.StartDay;
                        this.cmbEndDay.SelectedIndex = (int)coverConfiguration.EndDay;
                    }
                    else
                    {
                        this.dpStartDate.SelectedDate = coverConfiguration.StartDate;
                        this.dpEndDate.SelectedDate = coverConfiguration.EndDate;
                    }
                }

                if (dgPrices.Items.Count == 0 && coverConfigurationType == Backend.CoverConfigurationType.Event)
                {
                    this.dpStartDate.IsEnabled = true;
                    tpStartTime.IsEnabled = true;
                }
                else
                {
                    this.dpStartDate.IsEnabled = false;
                    tpStartTime.IsEnabled = false;
                }
            }
        }

        CoverConfigurationPrice GetLastItem()
        {
            if (dgPrices.Items.Count > 0)
                return (CoverConfigurationPrice)dgPrices.Items[dgPrices.Items.Count - 1];
            return null;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgPrices.Items.Count == 0)
                    throw new Cover.Backend.ExceptionManagement.CoverException("Debe configurar al menos un precio para el producto {0}");

                var lastItem = (CoverConfigurationPrice)dgPrices.Items[dgPrices.Items.Count - 1];
                var coverConfiguration = coverConfigurationService.GetCoverConfigurationById(coverConfigurationId);

                if (coverConfigurationType == Backend.CoverConfigurationType.General)
                {
                    if (!(lastItem.EndDay == coverConfiguration.EndDay && lastItem.EndTime == coverConfiguration.EndTime))
                        throw new Cover.Backend.ExceptionManagement.CoverException("El día y hora fin del último precio configurado debe ser igual al día y hora fin de acceso.");
                }
                else
                {
                    if (!(lastItem.EndDate == coverConfiguration.EndDate && lastItem.EndTime == coverConfiguration.EndTime))
                        throw new Cover.Backend.ExceptionManagement.CoverException("La fecha y hora fin del último precio configurado debe ser igual a la fecha y hora fin de acceso.");
                }

                coverConfigurationPriceService.DeleteCoverConfigurationPrices(coverConfigurationId);

                foreach (CoverConfigurationPrice item in dgPrices.Items)
                    coverConfigurationPriceService.InsertCoverConfigurationPrice(coverConfigurationId, item.StartDay, item.StartDate, item.StartTime, item.EndDay, item.EndDate, item.EndTime, item.Price);

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
