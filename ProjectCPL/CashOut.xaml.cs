using Cover.Backend;
using Cover.Backend.BL;
using Cover.POS.Controls;
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
using WBM.Common.Windows;
using WBM.Common;
using Cover.Backend.Entities;

namespace Cover.POS
{
    public partial class CashOut : Window
    {

        #region Properties

        private CashRegisterOperation cashOut { get; set; }

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

        public CashOut()
        {
            InitializeComponent();
            Window.GetWindow(this).Closing += MainWindow_Closing;
        }

        #endregion

        #region "Loaded"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TopBar.UserName = Cover.Backend.Context.User.Name;
            TopBar.OperationDate = Cover.Backend.Context.OperationDate;
        }

        #endregion

        #region "Closing"

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.TopBar.Dispose();
            GC.Collect();
        }

        #endregion

        #region "Button_Events"

        private void btnZ_Click(object sender, RoutedEventArgs e)
        {
            var closurePrinter = new Cover.Backend.Printer.ClosurePrinter(Cover.Backend.Context.OperationDate, Cover.Backend.ClosureType.Z);
            for (var i = 0; i < Cover.Backend.Configuration.SystemSettings.ClosureZ_Copies; i++)
            {
                closurePrinter.Print();
            }
            CashOutEndDate();
            CashRegisterOperationsCheck();
        }

        private void btnGlobal_Click(object sender, RoutedEventArgs e)
        {
            printGlobal();
        }

        private void btnExitCashOut_Click(object sender, RoutedEventArgs e)
        {
            var frm = new MainWindow();
            frm.Show();
            this.Close();
        }

        private void btnLockCashOut_Click(object sender, RoutedEventArgs e)
        {
            var frm = new Login();
            frm.Show();
            this.Close();
        }

        #endregion

        #region "CashRegisterOperations_Events"

        private void CashOutEndDate()
        {
            cashOut = cashRegisterOperationService.GetLastOpenCashRegisterOperationByTerminal();
            cashOut.Status = (int)CashRegisterOperationStatus.Closed;
            cashOut.CashOutDate = DateTime.Now;
            cashOut.TerminalId = Cover.Backend.Context.Terminal.Id;
            
        }

        private void CashRegisterOperationsCheck()
        {
            var cashregisterOperation = cashRegisterOperationService.GetOpenCashRegisterOperationByStatus();
            StoryBoardHelper.BeginFadeOut(parent);

            if (cashregisterOperation == null || cashregisterOperation.Status == (int)CashRegisterOperationStatus.Closed)
            {
                var messageUC = new Message_Window(String.Format("No hay terminales abiertas, se imprimirá el Corte Global."), MessageType.message);
                messageUC.ShowDialog();
                printGlobal();
                cashRegisterOperationService.UpdateCashOutRegisterOperation(cashOut);
            }
            else
            {
                var messageUC = new Message_Window(String.Format("Existen terminales abiertas, no se imprimirá el Corte Global hasta que todas se encuentren cerradas"), MessageType.message);
                messageUC.ShowDialog();
                cashRegisterOperationService.UpdateCashOutRegisterOperation(cashOut);
            }
            StoryBoardHelper.BeginFadeIn(parent);
            Application.Current.Shutdown();
        }

        private void printGlobal()
        {
            var closurePrinter = new Cover.Backend.Printer.ClosurePrinter(Cover.Backend.Context.OperationDate, Cover.Backend.ClosureType.Global);
            for (var i = 0; i < Cover.Backend.Configuration.SystemSettings.ClosureGlobal_Copies; i++)
            {
                closurePrinter.Print();
            }
        }

        #endregion


        
    }
}
