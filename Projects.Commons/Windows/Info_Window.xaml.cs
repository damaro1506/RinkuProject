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

namespace Projects.Commons.Windows
{
    public enum MessageType
    {
        message = 1,
        confirmation = 2
    }

    public partial class Info_Window : Window
    {

        #region "Properties"

        private Window parent { get; set; }
        private String message { get; set; }
        private MessageType messageType { get; set; }

        #endregion

        #region "Constructors"

        public Info_Window(String message)
        {
            InitializeComponent();
            this.message = message;
        }

        public Info_Window(String message, MessageType messageType)
        {
            InitializeComponent();
            this.message = message;
            this.messageType = messageType;
        }

        public Info_Window(String message, MessageType messageType, Window parent)
        {
            InitializeComponent();
            this.message = message;
            this.messageType = messageType;
            this.parent = parent;
        }

        #endregion

        #region "Loaded"

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            tbMessage.Text = message;
            if (messageType == MessageType.message)
                this.btnCancel.Visibility = System.Windows.Visibility.Hidden;
            btnOk.Focus();
            if (parent != null)
                ProjectsHelper.BeginFadeOut(parent);
        }

        #endregion

        #region "Closing"

        private void Info_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (parent != null)
                ProjectsHelper.BeginFadeIn(parent);
        }

        #endregion

        #region "Button_Events"

        public void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Z || e.Key == Key.Delete)
                btnCancel_Click(null, e);

            base.OnKeyDown(e);
        }

    }
}
