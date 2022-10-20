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
using Cover.Backend.BL;
using Cover.Backend.Entities;
using System.Windows.Media.Animation;
using Cover.POS;
using Cover.POS.Controls;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS.ConfigurationUC
{
    public partial class Users : UserControl
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

        public Users()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void UsersControls_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            dgUsers.Items.Clear();
            var users = userService.GetActiveUsers();
            foreach (var user in users)
                dgUsers.Items.Add(user);
        }

        #endregion

        #region "Button_Events"

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var frm = new User_Window();
            frm.OnSuccess += frm_OnSuccess;
            frm.ShowDialog();
            frm = null;
            StoryBoardHelper.BeginFadeIn(parent);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var frm = new User_Window();
            frm.userId = ((User)dgUsers.SelectedItem).Id;
            frm.OnSuccess += frm_OnSuccess;
            frm.ShowDialog();
            StoryBoardHelper.BeginFadeIn(parent);
        }

        void frm_OnSuccess()
        {
            LoadUsers();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            StoryBoardHelper.BeginFadeOut(parent);
            var user = ((User)dgUsers.SelectedItem);
            var messageUC = new Message_Window(String.Format("¿Está seguro que desea eliminar el usuario {0}?", user.Name), MessageType.confirmation);
            messageUC.ShowDialog();
            if (messageUC.DialogResult == true)
            {
                userService.ChangeStatus(user.Id, false);
                if (user.IsVisible)
                    dgUsers.Visibility = Visibility.Visible;
                LoadUsers();
            }
            StoryBoardHelper.BeginFadeIn(parent);
        }

        #endregion

    }
}
