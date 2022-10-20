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
using Cover.Backend.BL;
using Cover.Backend.Entities;
using Cover.POS.Controls;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS.ConfigurationUC
{
    public partial class User_Window : Window
    {

        #region "Properties"

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


        public Int64 userId { get; set; }
        public delegate void OnSuccessEventHandler();
        public event OnSuccessEventHandler OnSuccess;

        #endregion

        #region "Constructor"

        public User_Window()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadRoles();
            if (userId == 0)
            {
                this.lbTitle.Content = "Alta de usuario";
            }
            else
            {
                this.lbTitle.Content = "Edición de usuario";
                var user = userService.GetUserById(userId);
                this.txtName.Text = user.Name;
                this.txtPassword.Password = user.Password;
                for (var i = 0; i < cmbRol.Items.Count; i++)
                {
                    if (cmbRol.Items[i].ToString() == user.Role.Name)
                        cmbRol.SelectedIndex = i;
                }
            }
            
        }

        private void LoadRoles()
        {
            var roleNames = roleService.GetRoles();
            foreach (var role in roleNames)
                 cmbRol.Items.Add(role.Name);
        }

        #endregion

        #region "Button_Events"

        private void ExitButtonw_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userId == 0)
                {
                    var name = this.txtName.Text;
                    var password = this.txtPassword.Password;
                    var roleid = this.cmbRol.SelectedItem;
                    var user = new User();
                    user.Name = name;
                    user.Password = password;
                    user.Role = new Role() { Id = cmbRol.SelectedIndex + 1 };
                    userService.CreateUser(user);
                }
                else
                {
                    var user = userService.GetUserById(userId);
                    user.Name = this.txtName.Text;
                    user.Password = this.txtPassword.Password;
                    user.Role = new Role() { Id = cmbRol.SelectedIndex + 1 };
                    userService.UpdateUser(user);
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

        #endregion

        #region "TextBox_Events


        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.A || e.Key > Key.Z)
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
