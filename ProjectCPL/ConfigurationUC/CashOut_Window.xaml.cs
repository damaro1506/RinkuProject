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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS.ConfigurationUC
{
    public partial class CashOut_Window : UserControl
    {
        #region "Properties

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

        public CashOut_Window()
        {
            InitializeComponent();
        }
    
        #endregion

        #region "Loaded"

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtZCopies.Text = Cover.Backend.Configuration.SystemSettings.ClosureZ_Copies.ToString();
            txtGlobalCopies.Text = Cover.Backend.Configuration.SystemSettings.ClosureGlobal_Copies.ToString();
            txtHoursMax.Text = Cover.Backend.Configuration.SystemSettings.MaxHoursToClosure.ToString();
        }

        #endregion

        #region "Button_Events"

        private void btnCashSave_Click(object sender, RoutedEventArgs e)
        {
            Cover.Backend.Configuration.SystemSettings.UpdateSystemSetting(1, txtZCopies.Text);
            Cover.Backend.Configuration.SystemSettings.UpdateSystemSetting(2, txtGlobalCopies.Text);
            Cover.Backend.Configuration.SystemSettings.UpdateSystemSetting(24, txtHoursMax.Text);
            StoryBoardHelper.BeginFadeOut(parent);
            var messageUC = new Message_Window(String.Format("Se guardó la configuración exitosamente"), MessageType.message);
            messageUC.ShowDialog();
            StoryBoardHelper.BeginFadeIn(parent);
            messageUC = null;
        }

        #endregion

        #region "TextBox_Events"

        private void txtCopies_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= txtCopies_GotFocus;
        }

        private void txtCopies_KeyDown(object sender, KeyEventArgs e)
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
