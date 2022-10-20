using Cover.Backend.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS.ConfigurationUC
{
    public partial class Bracelet_Window : UserControl
    {
        #region "Properties

        private LogoService _logoService;
        private LogoService logoService
        {
            get
            {
                if (_logoService == null)
                    _logoService = new LogoService();
                return _logoService;
            }
            set
            {
                _logoService = value;
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

        public Backend.Entities.Logo logo { get; set; }

        #endregion

        #region "Constructor"

        public Bracelet_Window()
        {
            InitializeComponent();
        }

        #endregion

        #region "Loaded"

        private void UserControl_Loaded_2(object sender, RoutedEventArgs e)
        {
            logo = logoService.GetLogo();
            var imageInBytes = Convert.FromBase64String(logo.Image);
            this.ImgLogo.Source = Base64ToImage(imageInBytes);
            txtlogoName.Text = Cover.Backend.Configuration.SystemSettings.Ticket_Header_Line01.ToString();
            txtFreeText1.Text = Cover.Backend.Configuration.SystemSettings.Bracelet_FreeText_Line1;
            txtFreeText2.Text = Cover.Backend.Configuration.SystemSettings.Bracelet_FreeText_Line2;
            txtFreeText3.Text = Cover.Backend.Configuration.SystemSettings.Bracelet_FreeText_Line3;
        }

        #endregion

        #region "MouseActions"

        private void ImgLogo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                var bm = new System.Drawing.Bitmap(filename);
                var imageInBytes = ImageToByte(bm);
                var imageB64 = Convert.ToBase64String(imageInBytes);
                logo = new Cover.Backend.Entities.Logo();
                logo.Image = imageB64;
                this.ImgLogo.Source = Base64ToImage(imageInBytes);

            }
        }

        #endregion

        #region "Image_Events"

        public BitmapImage Base64ToImage(byte[] byteArray)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                img.BeginInit();
                img.StreamSource = stream;
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                img.Freeze();
            }
            return img;
        }

        public static byte[] ImageToByte(System.Drawing.Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        #endregion

        #region "Button_Events"

        private void btnBraceletSave_Click(object sender, RoutedEventArgs e)
        {
            Cover.Backend.Configuration.SystemSettings.UpdateSystemSetting(3, txtlogoName.Text);
            Cover.Backend.Configuration.SystemSettings.UpdateSystemSetting(25, txtFreeText1.Text);
            Cover.Backend.Configuration.SystemSettings.UpdateSystemSetting(26, txtFreeText2.Text);
            Cover.Backend.Configuration.SystemSettings.UpdateSystemSetting(27, txtFreeText3.Text);
            StoryBoardHelper.BeginFadeOut(parent);
            var messageUC = new Message_Window(String.Format("Su configuración se guardó exitosamente."), MessageType.message);
            messageUC.ShowDialog();
            StoryBoardHelper.BeginFadeIn(parent);
            logoService.SaveLogo(logo);
        }

        #endregion

    }
}
