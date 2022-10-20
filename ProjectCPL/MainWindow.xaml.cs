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
using ProyectoCPL.Backend.cplServices;
using ProyectoCPL.Backend.Entities;
using System.Windows.Threading;
using ProyectoCPL.Backend.ExceptionManagement;
using Cover.POS;
using System.Windows.Media.Animation;
using System.Globalization;
using ProjectCPL.Controls;
using Projects.Commons.Windows;
using Projects.Commons;

namespace Cover.POS
{
    public partial class MainWindow : Window
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

        private Int64 SelectedCoverMaleConfiguration = 0;
        private Int64 SelectedCoverFemaleConfiguration = 0;
        private List<CoverConfiguration> coverConfigurations { get; set; }
        private Cover.Backend.CoverConfigurationType CoverConfigurationType;

        #endregion

        #region "Constructor"

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
        }

        #endregion

        #region "Loaded"

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.TopBar.UserName = Cover.Backend.Context.User.Name;
            this.TopBar.OperationDate = Cover.Backend.Context.OperationDate;
            this.TopBar.MinutedCompleted += TopBar_MinutedCompleted;

            CoverConfigurationType = Backend.CoverConfigurationType.General;
            NumPad.OnEnter += btnCerrar_Click;
            NumPad.ControlFocus = this.txtquantityBoy;

            if (DateTime.Now > Cover.Backend.Context.OperationDate.AddDays(1).AddHours(Cover.Backend.Configuration.SystemSettings.MaxHoursToClosure))
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window("Superó el tiempo máximo configurado para hacer el corte. Para continuar operando es necesario hacer el corte global.", MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
                return;
            }

            LoadCoverConfigurations();
        }

        void TopBar_MinutedCompleted()
        {
            LoadCoverConfigurations();
        }

        public void LoadCoverConfigurations()
        {
            this.pnlMaleCovers.Children.Clear();
            this.pnlFemaleCovers.Children.Clear();

            if (CoverConfigurationType == Backend.CoverConfigurationType.General)
            {
                coverConfigurations = coverconfigurationService.GetCoversConfiguration();
                coverConfigurations = coverConfigurations.Where(a => a.StartDateTime <= DateTime.Now && DateTime.Now <= a.EndDateTime).ToList();
            }
            else
                coverConfigurations = coverconfigurationService.GetCoversConfiguration().Where(a => a.ConfigurationType == Backend.CoverConfigurationType.Event && DateTime.Now < a.StartDate.Value).ToList();

            foreach (var coverConfiguration in coverConfigurations)
            {
                var coverConfigurationPrices = coverConfigurationPriceService.GetCoverConfigurationPrices(coverConfiguration.Id);
                var coverConfigurationPrice = coverConfigurationPrices.Where(a => a.StartDateTime <= DateTime.Now && DateTime.Now <= a.EndDateTime.AddSeconds(59).AddMilliseconds(999)).FirstOrDefault();

                if (coverConfigurationPrice == null)
                    continue;

                var button = new Button();
                button.Tag = coverConfiguration.Id.ToString();
                button.Width = 170;
                button.Height = 56;
                button.Margin = new Thickness(5);
                button.Style = (Style)FindResource("GrayButton");
                button.Content = String.Format("{0} {1} ({2})", coverConfiguration.Name, Environment.NewLine, coverConfigurationPrice.Price.ToString("C"));

                if (coverConfiguration.CustomerType.Id == 1)
                {
                    button.Click += buttonMale_Click;
                    this.pnlMaleCovers.Children.Add(button);
                    if (coverConfiguration.Id == SelectedCoverMaleConfiguration)
                        button.Style = (Style)FindResource("PrimaryButton");
                }
                else if (coverConfiguration.CustomerType.Id == 2)
                {
                    button.Click += buttonFemale_Click;
                    this.pnlFemaleCovers.Children.Add(button);
                    if (coverConfiguration.Id == SelectedCoverFemaleConfiguration)
                        button.Style = (Style)FindResource("PrimaryButton");
                }
            }

            if (!coverConfigurations.Where(a => a.Id == SelectedCoverMaleConfiguration).Any())
            {
                if (this.pnlMaleCovers.Children.Count > 0)
                {
                    var ctrl = this.pnlMaleCovers.Children[0];
                    if (ctrl.GetType() == typeof(Button))
                    {
                        ((Button)ctrl).Style = (Style)FindResource("PrimaryButton");
                        SelectedCoverMaleConfiguration = Convert.ToInt64(((Button)ctrl).Tag);
                    }
                }
            }

            if (!coverConfigurations.Where(a => a.Id == SelectedCoverFemaleConfiguration).Any())
            {
                if (this.pnlFemaleCovers.Children.Count > 0 && SelectedCoverFemaleConfiguration == 0)
                {
                    var ctrl = this.pnlFemaleCovers.Children[0];
                    if (ctrl.GetType() == typeof(Button))
                    {
                        ((Button)ctrl).Style = (Style)FindResource("PrimaryButton");
                        SelectedCoverFemaleConfiguration = Convert.ToInt64(((Button)ctrl).Tag);
                    }
                }
            }
        }

        #endregion

        #region "Closing"

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.TopBar.Dispose();
            this.NumPad.Dispose();
            GC.Collect();
        }

        #endregion

        #region "TopBarActions"

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            var frm = new Login();
            frm.Show();
            this.Close();
        }

        private void btnConfiguration_Click(object sender, RoutedEventArgs e)
        {
            if (Cover.Backend.Context.User.Role.Id == 3)
            {
                var frm = new Configuration();
                frm.Show();
                this.Close();
            }
            else
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("No cuenta con los permisos necesarios"), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
        }

        private void btnClosure_Click(object sender, RoutedEventArgs e)
        {
            var frm = new CashOut();
            frm.Show();
            this.Close();
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            var frm = new Reports();
            frm.Show();
            this.Close();
        }

        #endregion

        #region "Qty_Events"

        private void txtquantityBoy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
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

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            txtquantityBoy.Text = txtquantityBoy.Text == "" ? "0" : txtquantityBoy.Text;
            txtquantityBoy.Text = (Convert.ToInt32(txtquantityBoy.Text) + 1).ToString();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            txtquantityBoy.Text = txtquantityBoy.Text == "" ? "0" : txtquantityBoy.Text;
            var value = Convert.ToInt32(txtquantityBoy.Text);
            if (value >= 1)
                txtquantityBoy.Text = (value - 1).ToString();
        }

        private void PlusF_Click(object sender, RoutedEventArgs e)
        {
            txtquantityGirl.Text = txtquantityGirl.Text == "" ? "0" : txtquantityGirl.Text;
            txtquantityGirl.Text = (Convert.ToInt32(txtquantityGirl.Text) + 1).ToString();
        }

        private void MinusF_Click(object sender, RoutedEventArgs e)
        {
            txtquantityGirl.Text = txtquantityGirl.Text == "" ? "0" : txtquantityGirl.Text;
            var value = Convert.ToInt32(txtquantityGirl.Text);
            if (value >= 1)
                txtquantityGirl.Text = (value - 1).ToString();
        }

        private void txtquantityBoy_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            NumPad.ControlFocus = tb;
        }

        private void txtquantityBoy_GotMouseCapture(object sender, MouseEventArgs e)
        {
            ((TextBox)sender).Text = "";
            NumPad.ControlFocus = (TextBox)sender;
        }

        void buttonMale_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = ((Button)sender);
            selectedButton.Style = (Style)FindResource("PrimaryButton");
            foreach (var button in this.pnlMaleCovers.Children)
                if (button.GetType() == typeof(Button))
                    if (selectedButton.Content != ((Button)button).Content)
                        ((Button)button).Style = (Style)FindResource("GrayButton");
            SelectedCoverMaleConfiguration = Convert.ToInt32(selectedButton.Tag);
        }

        void buttonFemale_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = ((Button)sender);
            selectedButton.Style = (Style)FindResource("PrimaryButton");
            foreach (var button in this.pnlFemaleCovers.Children)
                if (button.GetType() == typeof(Button))
                    if (selectedButton.Content != ((Button)button).Content)
                        ((Button)button).Style = (Style)FindResource("GrayButton");
            SelectedCoverFemaleConfiguration = Convert.ToInt32(selectedButton.Tag);
        }

        #endregion

        #region "Close_Events"

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            btnCerrar_Click();
        }

        private void btnCerrar_Click()
        {
            try
            {
                Int32 quantityMale = txtquantityBoy.Text == "" ? 0 : Convert.ToInt32(txtquantityBoy.Text);
                Int32 quantityFemale = txtquantityGirl.Text == "" ? 0 : Convert.ToInt32(txtquantityGirl.Text);

                if (quantityMale == 0 && quantityFemale == 0)
                    throw new CoverException("Para continuar debe cobrar al menos un cover");

                var coverDetails = new List<CoverDetail>();

                if (quantityMale != 0 && SelectedCoverMaleConfiguration == 0)
                    throw new CoverException("Para continuar debe seleccionar un tipo de cover para hombre");

                if (quantityMale != 0 && SelectedCoverMaleConfiguration != 0)
                {
                    var covermaleConfiguration = coverConfigurations.Where(a => a.Id == SelectedCoverMaleConfiguration).First();

                    var coverConfigurationPrices = coverConfigurationPriceService.GetCoverConfigurationPrices(SelectedCoverMaleConfiguration);
                    var coverConfigurationPrice = coverConfigurationPrices.Where(a => a.StartDateTime <= DateTime.Now && DateTime.Now <= a.EndDateTime).FirstOrDefault();
                    if (coverConfigurationPrice == null)
                    {
                        LoadCoverConfigurations();
                        throw new CoverException("El producto seleccionado ya no está disponible");
                    }

                    coverDetails.Add(new CoverDetail() { Id = Guid.NewGuid(), CustomerType = new CustomerType() { Id = (int)Cover.Backend.CustomerTypeC.Male }, Quantity = quantityMale, UnitPrice = coverConfigurationPrice.Price, CoverConfiguration = covermaleConfiguration });
                }

                if (quantityFemale != 0 && SelectedCoverFemaleConfiguration == 0)
                    throw new CoverException("Para continuar debe seleccionar un tipo de cover para mujer");

                if (quantityFemale != 0 && SelectedCoverFemaleConfiguration != 0)
                {
                    var coverfemaleConfiguration = coverConfigurations.Where(a => a.Id == SelectedCoverFemaleConfiguration).First();

                    var coverConfigurationPrices = coverConfigurationPriceService.GetCoverConfigurationPrices(SelectedCoverFemaleConfiguration);
                    var coverConfigurationPrice = coverConfigurationPrices.Where(a => a.StartDateTime <= DateTime.Now && DateTime.Now <= a.EndDateTime).FirstOrDefault();
                    if (coverConfigurationPrice == null)
                    {
                        LoadCoverConfigurations();
                        throw new CoverException("El producto seleccionado ya no está disponible");
                    }

                    coverDetails.Add(new CoverDetail() { Id = Guid.NewGuid(), CustomerType = new CustomerType() { Id = (int)Cover.Backend.CustomerTypeC.Female }, Quantity = quantityFemale, UnitPrice = coverConfigurationPrice.Price, CoverConfiguration = coverfemaleConfiguration });
                }

                var frm = new PaymentWindow(coverDetails);
                frm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                var exDetail = Cover.Backend.ExceptionManagement.ExceptionManagement.Publish(ex);
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format(exDetail.Message), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
        }

        #endregion

        private void btnCoverConfigurationType_Click(object sender, RoutedEventArgs e)
        {
            if (CoverConfigurationType == Backend.CoverConfigurationType.General)
            {
                btnCoverConfigurationType.Content = "VENTA GENERAL";
                CoverConfigurationType = Backend.CoverConfigurationType.Event;
            }
            else
            {
                btnCoverConfigurationType.Content = "PREVENTA DE EVENTOS";
                CoverConfigurationType = Backend.CoverConfigurationType.General;
            }

            LoadCoverConfigurations();
        }

    }
}
