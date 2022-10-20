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
using System.Windows.Threading;
using Cover.Backend.BL;
using Cover.Backend.ExceptionManagement;
using Cover.POS.Controls;
using System.Data.SqlClient;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS
{
    public partial class Login : Window
    {
        #region "Properties"

        private UserService _userService;
        private UserService userService
        {
            get
            {
                if (_userService == null)
                    _userService = new UserService();
                return _userService;
            }
            set
            {
                _userService = value;
            }
        }

        private CashRegisterOperationService _cashRegisterOperationService;
        private CashRegisterOperationService cashRegisterOperationService
        {
            get
            {
                if (_cashRegisterOperationService == null)
                    _cashRegisterOperationService = new CashRegisterOperationService();
                return _cashRegisterOperationService;
            }
            set
            {
                _cashRegisterOperationService = value;
            }
        }
        private TerminalService _terminalService;
        private TerminalService terminalService
        {
            get
            {
                if (_terminalService == null)
                    _terminalService = new TerminalService();
                return _terminalService;
            }
            set
            {
                _terminalService = value;
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

        #endregion

        #region "Constructor"

        public Login()
        {
            InitializeComponent();
            Window.GetWindow(this).Closing += Login_Closing;
        }

        void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.TopBar.Dispose();
            this.NumPad.Dispose();
            GC.Collect();
        }

        #endregion

        #region "Loaded"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var coverSecurity = new WBM.Common.Windows.Configuration.ConfigurateDatabase_Window(Cover.Backend.DataAccess.Helper.coverPosCS, "CoverPOS");
                if (!TestConnection())
                {
                    StoryBoardHelper.BeginFadeOut(parent);
                    coverSecurity.SaveConnection += coverSecurity_SaveConnection;
                    coverSecurity.ShowDialog();
                    StoryBoardHelper.BeginFadeIn(parent);
                }
                var terminal = terminalService.GetTerminal(Environment.MachineName);

                if (terminal == null)
                {
                    StoryBoardHelper.BeginFadeOut(parent);
                    var messageUC = new Message_Window(String.Format("La terminal no está configurada. Favor de reportarlo al área de soporte"), MessageType.message);
                    messageUC.ShowDialog();
                    StoryBoardHelper.BeginFadeIn(parent);
                    Application.Current.Shutdown();
                }

                Cover.Backend.Context.Terminal = terminal;
                txtPassword.Focus();
                NumPad.ControlFocus = this.txtPassword;
                NumPad.OnEnter += NumPad_OnEnter;

                var cashRegisterOperation = cashRegisterOperationService.GetLastOpenCashRegisterOperationByTerminal();
                if (cashRegisterOperation != null)
                {
                    PnlCashFund.Visibility = System.Windows.Visibility.Hidden;
                    TopBar.OperationDate = Cover.Backend.Context.OperationDate;
                }
                else
                {
                    TopBar.Message = "Usted iniciará el día de operación " + Cover.Backend.Context.OperationDate.ToString("yyy-MM-dd");
                }
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

        #region "Actions"

        void NumPad_OnEnter()
        {
            try
            {
                txtPassword.Focus();
                if (Cover.Backend.Context.Terminal == null)
                    throw new CoverException("La terminal no está configurada para utilizar el punto de venta");

                var user = userService.GetUserByPassword(txtPassword.Password);
                Cover.Backend.Context.User = user;

                var cashRegisterOperation = cashRegisterOperationService.GetLastOpenCashRegisterOperationByTerminal();
                if (cashRegisterOperation == null)
                {
                    if (txtCashFund.Text.Trim() == "")
                    {
                        
                        if (txtCashFund.IsFocused)
                            throw new CoverException("El fondo de caja es requerido");
                        txtCashFund.Focus();
                        return;
                    }

                    var cashFund = Convert.ToDecimal(txtCashFund.Text);
                    cashRegisterOperationService.CreateCashRegisterOperation(cashFund, Cover.Backend.Context.OperationDate);
                }
                var frm = new MainWindow();
                frm.ShowDialog();
                this.Close();
                GC.Collect();
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

        private void txtCashFund_GotFocus(object sender, RoutedEventArgs e)
        {
            NumPad.ControlFocus = this.txtCashFund;
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            NumPad.ControlFocus = this.txtPassword;
        }

        private void txtPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.A || e.Key > Key.Z)
            {
                e.Handled = false;
                if (e.Key == Key.Enter)
                    NumPad_OnEnter();
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

        private void txtCashFund_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.A || e.Key > Key.Z)
            {
                e.Handled = false;
                if (e.Key == Key.Enter)
                    NumPad_OnEnter();
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

        #region "Test of Connection"

        private Boolean TestConnection()
        {
            try
            {
                SqlConnection sqlConn = new SqlConnection(Cover.Backend.DataAccess.Helper.coverPosCS);
                sqlConn.Open();
                sqlConn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        void coverSecurity_SaveConnection(String connectionString)
        {
            Cover.Backend.DataAccess.Helper.coverPosCS = connectionString;
        }

        void Numpad_OnClear()
        {

        }
        #endregion

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var messageUC = new Message_Window(String.Format("¿Está seguro que desea salir del sistema?"), MessageType.confirmation);
            messageUC.ShowDialog();
            StoryBoardHelper.BeginFadeIn(parent);
            if (messageUC.DialogResult == true)
                Application.Current.Shutdown();
        }

    }
}
