using Cover.POS.ConfigurationUC;
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

namespace Cover.POS
{
    public partial class Configuration : Window
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

        private ConfigurationUC.CashOut_Window _cashOut_Window;
        private ConfigurationUC.CashOut_Window cashOut_Window
        {
            get
            {
                if (_cashOut_Window == null)
                    _cashOut_Window = new ConfigurationUC.CashOut_Window();
                return _cashOut_Window;
            }
            set
            {
                _cashOut_Window = value;
            }
        }

        private ConfigurationUC.Bracelet_Window _bracelet_Window;
        private ConfigurationUC.Bracelet_Window bracelet_Window
        {
            get
            {
                if (_bracelet_Window == null)
                    _bracelet_Window = new ConfigurationUC.Bracelet_Window();
                return _bracelet_Window;
            }
            set
            {
                _bracelet_Window = value;
            }
        }

        private ConfigurationUC.CoverConfigurations _cover_Window;
        private ConfigurationUC.CoverConfigurations cover_Window
        {
            get
            {
                if (_cover_Window == null)
                    _cover_Window = new ConfigurationUC.CoverConfigurations();
                return _cover_Window;
            }
            set
            {
                _cover_Window = value;
            }
        }

        private ConfigurationUC.Users _users_Window;
        private ConfigurationUC.Users users_Window
        {
            get
            {
                if (_users_Window == null)
                    _users_Window = new ConfigurationUC.Users();
                return _users_Window;
            }
            set
            {
                _users_Window = value;
            }
        }

        private ConfigurationUC.Areas _areas_Window;
        private ConfigurationUC.Areas areas_Window
        {
            get
            {
                if (_areas_Window == null)
                    _areas_Window = new ConfigurationUC.Areas();
                return _areas_Window;
            }
            set
            {
                _areas_Window = value;
            }
        }

        private ConfigurationUC.Ticket_Window _ticket_Window;
        private ConfigurationUC.Ticket_Window ticket_Window
        {
            get
            {
                if (_ticket_Window == null)
                    _ticket_Window = new ConfigurationUC.Ticket_Window();
                return _ticket_Window;
            }
            set
            {
                _ticket_Window = value;
            }
        }

        #endregion

        #region "Constructor"

        public Configuration()
        {
            InitializeComponent();
            Window.GetWindow(this).Closing += MainWindow_Closing;
        }

        #endregion

        #region "Loaded"

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            TopBar.UserName = Cover.Backend.Context.User.Name;
            TopBar.OperationDate = Cover.Backend.Context.OperationDate;
        }

        #endregion

        #region "Closing"

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowContent.Content = null;
            this.TopBar.Dispose();
            GC.Collect();
        }

        #endregion

        #region "Button_Events"

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            var frm = new Login();
            frm.Show();
            this.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var frm = new MainWindow();
            frm.Show();
            this.Close();
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            ShowUC(users_Window);
        }

        private void btnArea_Click(object sender, RoutedEventArgs e)
        {
            ShowUC(areas_Window);
        }

        private void btnConfigCover_Click(object sender, RoutedEventArgs e)
        {
            ShowUC(cover_Window);
        }

        private void btnConfigBracelet_Click(object sender, RoutedEventArgs e)
        {
            ShowUC(bracelet_Window);
        }

        private void btnCortes_Click(object sender, RoutedEventArgs e)
        {
            ShowUC(cashOut_Window);
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            ShowUC(ticket_Window);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var messageUC = new Message_Window(String.Format("¿Está seguro que desea salir del sistema?"), MessageType.confirmation);
            messageUC.ShowDialog();
            StoryBoardHelper.BeginFadeIn(parent);
            if (messageUC.DialogResult == true)
                Application.Current.Shutdown();
        }

        #endregion

        #region "UserControl_Events"

        private void ShowUC(UserControl uc)
        {
            WindowContent.Content = null;
            WindowContent.Content = uc;
            GC.Collect();
        }

        #endregion
                     
    }
}
