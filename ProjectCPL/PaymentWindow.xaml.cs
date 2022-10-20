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
using Cover.Backend.Entities;
using System.Windows.Threading;
using Cover.Backend.BL;
using Cover.Backend.ExceptionManagement;
using System.Data;
using System.Globalization;
using Cover.POS.Controls;
using WBM.Common.Windows;
using WBM.Common;
using Cover.Backend.Printer;
using System.Data.SqlClient;

namespace Cover.POS
{
    public partial class PaymentWindow : Window
    {

        #region "Properties"

        private CoverDataService _coverDataService;
        private CoverDataService coverDataService
        {
            get
            {
                if (_coverDataService == null)
                    _coverDataService = new CoverDataService();
                return _coverDataService;
            }
            set
            {
                _coverDataService = value;
            }
        }

        private CoverDetailService _coverdetailService;
        private CoverDetailService coverdetailService
        {
            get
            {
                if (_coverdetailService == null)
                    _coverdetailService = new CoverDetailService();
                return _coverdetailService;
            }
            set
            {
                _coverdetailService = value;
            }
        }

        private PaymentService _paymentService;
        private PaymentService paymentService
        {
            get
            {
                if (_paymentService == null)
                    _paymentService = new PaymentService();
                return _paymentService;
            }
            set
            {
                _paymentService = value;
            }
        }

        private PaymentMethodService _paymentMethodService;
        private PaymentMethodService paymentMethodService
        {
            get
            {
                if (_paymentMethodService == null)
                    _paymentMethodService = new PaymentMethodService();
                return _paymentMethodService;
            }
            set
            {
                _paymentMethodService = value;
            }
        }

        private List<PaymentMethod> _paymentMethods;
        private List<PaymentMethod> paymentMethods
        {
            get
            {
                if (_paymentMethods == null)
                    _paymentMethods = paymentMethodService.GetActivePaymentMethods();
                return _paymentMethods;
            }
            set
            {
                _paymentMethods = value;
            }
        }

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

        private CoverData coverData { get; set; }
        private Int32 SelectedPaymentMethodId { get; set; }

        #endregion

        #region "Constructor"

        public PaymentWindow(List<CoverDetail> coverDetails)
        {
            InitializeComponent();
            Window.GetWindow(this).Closing += MainWindow_Closing;
            coverData = new CoverData();
            coverData.CoverDetails = coverDetails;
            coverData.CoverDate = DateTime.Now;
            coverData.Subtotal = coverDetails.Sum(a => a.SubtotalCalculated);
            coverData.Iva = coverDetails.Sum(a => a.IvaCalculated);
            coverData.Total = coverDetails.Sum(a => a.TotalCalculated);
            coverData.CoverQuantity = coverDetails.Sum(a => a.Quantity);
            coverData.Id = Guid.NewGuid();
            coverData.CashierId = Cover.Backend.Context.User.Id;
            coverData.TerminalId = Convert.ToInt32(Cover.Backend.Context.Terminal.Id);
            coverData.OperationDate = Cover.Backend.Context.OperationDate;
            lbTotalPay.Content = coverData.Total.ToString("C");
            lbPendingPay.Content = coverData.Total.ToString("C");
        }

        #endregion

        #region "Loaded"

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TopBar.UserName = Cover.Backend.Context.User.Name;
            TopBar.OperationDate = Cover.Backend.Context.OperationDate;
            NumPad.ControlFocus = this.txtAmountPay;
            NumPad.OnEnter += numpadPay_OnEnter;
            LoadPaymentMethods();
        }

        private void LoadPaymentMethods()
        {
            this.PnlPaymentMethods.Children.Clear();
            foreach (var paymentMethod in paymentMethods)
            {
                var button = new Button();
                button.Content = paymentMethod.Equivalence == 1 ? paymentMethod.Name : String.Format("{0} ({1})", paymentMethod.Name, paymentMethod.Equivalence.ToString("C"));
                button.Tag = paymentMethod.Id.ToString();
                button.Width = 180;
                button.Height = 56;
                button.Click += button_Click;
                button.Margin = new Thickness(5);
                button.Style = (Style)FindResource("GrayButton");
                this.PnlPaymentMethods.Children.Add(button);
            }
            if (paymentMethods.Count > 0)
            {
                SelectedPaymentMethodId = paymentMethods.First().Id;
                foreach (var button in this.PnlPaymentMethods.Children)
                    if (button.GetType() == typeof(Button))
                        if (SelectedPaymentMethodId == Convert.ToInt32(((Button)button).Tag))
                            ((Button)button).Style = (Style)FindResource("PrimaryButton");
                txtAmountPay.Focus();
            }
        }

        #endregion

        #region "Closing"

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.TopBar.Dispose();
            this.NumPad.Dispose();
            this.PnlPaymentMethods.Children.Clear();
            coverData = null;
            GC.Collect();
        }

        #endregion

        #region "Button_Events"

        void button_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = ((Button)sender);
            selectedButton.Style = (Style)FindResource("PrimaryButton");
            foreach (var button in this.PnlPaymentMethods.Children)
                if (button.GetType() == typeof(Button))
                    if (selectedButton.Content != ((Button)button).Content)
                        ((Button)button).Style = (Style)FindResource("GrayButton");
            SelectedPaymentMethodId = Convert.ToInt32(selectedButton.Tag);
            txtAmountPay.Focus();
        }

        void numpadPay_OnEnter()
        {
            var balance = GetBalance();

            if (balance <= 0)
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("No hay saldo pendiente"), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
                return;
            }

            if (txtAmountPay.Text == "" || Convert.ToDecimal(txtAmountPay.Text) == 0)
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("Ingrese el monto a pagar"), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
                return;
            }

            var paymentMethod = paymentMethods.Where(a => a.Id == SelectedPaymentMethodId).First();
            var total = paymentMethod.Equivalence == 1 ? Convert.ToDecimal(txtAmountPay.Text) : Convert.ToDecimal(txtAmountPay.Text) * paymentMethod.Equivalence;

            var changePay = 0m;
            var totalPaid = GetTotalPaid();
            changePay = (totalPaid + total) > coverData.Total ? Math.Abs((totalPaid + total) - coverData.Total) : 0m;
            var payment = new Payment { CoverId = coverData.Id, ReceivedAmount = Convert.ToDecimal(txtAmountPay.Text), PaymentMethod = paymentMethod, TotalReceivedAmount = total, Change = changePay, OperationDate = Cover.Backend.Context.OperationDate };
            dgPaymentCover.Items.Add(payment);

            balance = GetBalance();
            totalPaid = GetTotalPaid();
            lbPendingPay.Content = balance.ToString("C");
            lbTotalPaid.Content = totalPaid.ToString("C");
            txtAmountPay.Text = "";
            lbChangePay.Content = changePay.ToString("C");

            if (changePay > 0)
            {
                lbChangePay.Content = Math.Abs(changePay).ToString("C");
                lbPendingPay.Content = (0.00).ToString("C");
            }
        }

        private void btnAuto_Click(object sender, RoutedEventArgs e)
        {
            var balance = GetBalance();
            if (coverData.Total != balance)
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("Solo para cargo total de la cuenta"), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
                return;
            }

            var paymentMethod = paymentMethods.Where(a => a.Id == SelectedPaymentMethodId).First();
            var total = paymentMethod.Equivalence == 1 ? balance : (balance / paymentMethod.Equivalence) * paymentMethod.Equivalence;

            lbPendingPay.Content = (0.00).ToString("C");
            lbTotalPaid.Content = coverData.Total.ToString("C");
            lbChangePay.Content = (0.00).ToString("C");

            var payment = new Payment { CoverId = coverData.Id, ReceivedAmount = balance, PaymentMethod = paymentMethod, TotalReceivedAmount = total, Change = 0, OperationDate = Cover.Backend.Context.OperationDate };
            dgPaymentCover.Items.Add(payment);
            txtAmountPay.Text = "";
        }

        private void btnDeletePayment_Click(object sender, RoutedEventArgs e)
        {
            if (dgPaymentCover.Items.Count == 0)
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("No hay pagos registrados"), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
                return;
            }

            var totalPaid = GetTotalPaid();
            var balance = GetBalance();
            var selectedItem = dgPaymentCover.SelectedItem;
            if (selectedItem != null || dgPaymentCover.SelectedItem != null)
                dgPaymentCover.Items.Remove(selectedItem);
            else
                dgPaymentCover.Items.RemoveAt(dgPaymentCover.Items.Count - 1);

            totalPaid = GetTotalPaid();
            balance = GetBalance();

            lbPendingPay.Content = balance.ToString("C");
            lbTotalPaid.Content = totalPaid.ToString("C");
            lbChangePay.Content = (0).ToString("C");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var balance = GetBalance();
            if (balance > Convert.ToDecimal(0.00))
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("Todavía tiene un saldo pendiente de {0}", lbPendingPay.Content), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
                return;
            }

            SqlConnection sqlConn = null;
            SqlTransaction sqlTran = null;

            try
            {
                var coverConfigurations = coverconfigurationService.GetCoversConfiguration();

                sqlConn = new SqlConnection(Cover.Backend.DataAccess.Helper.coverPosCS);
                sqlConn.Open();
                sqlTran = sqlConn.BeginTransaction();

                coverDataService.CreateCoverData(coverData, ref sqlTran);

                foreach (var coverDetail in coverData.CoverDetails)
                {
                    var temp_Quantity = coverDetail.Quantity;
                    for (var i = 0; i < temp_Quantity; i++)
                    {
                        var coverConfiguration = coverConfigurations.Where(a => a.Id == coverDetail.CoverConfiguration.Id).FirstOrDefault();
                        if (coverConfiguration == null)
                            throw new Exception(String.Format("No se encontró la configuración {0}", coverDetail.CoverConfiguration.Id));

                        coverDetail.ValidFrom = coverConfiguration.StartDateTime;
                        coverDetail.ValidTo = coverConfiguration.EndDateTime;
                        coverDetail.CoverId = coverData.Id;
                        coverDetail.Quantity = 1;
                        coverdetailService.CreateCoverDetail(coverDetail, ref sqlTran);
                    }
                }

                foreach (var payment in dgPaymentCover.Items)
                    paymentService.CreatePayment((Payment)payment, ref sqlTran);

                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                sqlTran.Rollback();
                var exDetail = Cover.Backend.ExceptionManagement.ExceptionManagement.Publish(ex);
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format(exDetail.Message), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
                return;
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }

            try
            {
                var coverDatas = coverDataService.GetCoverDataById(coverData.Id);
                Cover.Backend.Printer.BraceletPrinter braceletPrinter;
                foreach (var coverDetail in coverDatas.CoverDetails)
                {
                    if (coverDetail.CoverConfiguration.Printer != "[NO IMPRIMIR]")
                    {
                        braceletPrinter = new Backend.Printer.BraceletPrinter(coverDetail, coverData.CoverDate);
                        braceletPrinter.Print();
                        braceletPrinter = null;
                    }
                }

                for (var i = 0; i < Cover.Backend.Configuration.SystemSettings.TicketCopies; i++)
                {
                    var printer = new TicketPrinter(coverDatas);
                    printer.Print();
                }

                var frm = new MainWindow();
                frm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                var exDetail = Cover.Backend.ExceptionManagement.ExceptionManagement.Publish(ex);
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window("Ocurrió un problema al imprimir brazaletes o ticket de pagado", MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var frm = new MainWindow();
            frm.Show();
            this.Close();
        }

        #endregion

        #region "TextBox_events"

        private void txtPay_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtPay_GotFocus;
        }

        private void txtPay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.A || e.Key > Key.Z)
            {
                e.Handled = false;
                if (e.Key == Key.Enter)
                {
                    numpadPay_OnEnter();
                    txtAmountPay.Text = "";
                }
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

        #region "Balance_Events"

        private Decimal GetTotalPaid()
        {
            Decimal balance = 0;
            foreach (var item in dgPaymentCover.Items)
                balance += ((Payment)item).TotalReceivedAmount;
            return balance;
        }

        private Decimal GetBalance()
        {
            return coverData.Total - GetTotalPaid();
        }

        #endregion

    }
}
