using Cover.Backend.Entities;
using Cover.Backend.Printer;
using System;
using System.Collections.Generic;

using System.Media;
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
using WBM.Common;
using WBM.Common.Windows;
using Cover.Backend.Configuration;
using System.IO;
using System.Windows.Xps.Packaging;
using System.Windows.Markup;
using System.Drawing.Printing;
using System.Windows.Xps;
using System.ComponentModel;


namespace Cover.POS.ConfigurationUC
{
    public partial class Ticket_Window : System.Windows.Controls.UserControl
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

        #endregion

        #region "Contructor"

        public Ticket_Window()
        {
            InitializeComponent();
        }

        #endregion

        #region "Button_Events"

        private void btnSaveTicket_Click(object sender, RoutedEventArgs e)
        {
            SystemSettings.UpdateSystemSetting(4, cmbTicketQuantity.Text);
            SystemSettings.UpdateSystemSetting(5, txtHeader1.Text);
            SystemSettings.UpdateSystemSetting(6, txtHeader2.Text);
            SystemSettings.UpdateSystemSetting(7, txtHeader3.Text);
            SystemSettings.UpdateSystemSetting(8, txtHeader4.Text);
            SystemSettings.UpdateSystemSetting(9, txtHeader5.Text);
            SystemSettings.UpdateSystemSetting(10, txtHeader6.Text);
            SystemSettings.UpdateSystemSetting(14, txtFooter1.Text);
            SystemSettings.UpdateSystemSetting(15, txtFooter2.Text);
            SystemSettings.UpdateSystemSetting(16, txtFooter3.Text);
            SystemSettings.UpdateSystemSetting(17, txtFooter4.Text);
            SystemSettings.UpdateSystemSetting(18, txtFooter5.Text);
            SystemSettings.UpdateSystemSetting(21, this.chkCenterHeader.IsChecked.ToString());
            SystemSettings.UpdateSystemSetting(22, this.chkCenterFooter.IsChecked.ToString());
            SystemSettings.UpdateSystemSetting(30, this.cmbTicketQuantity.Text);

            StoryBoardHelper.BeginFadeOut(parent);
            var messageUC = new Message_Window(String.Format("Se guardó la configuración exitosamente"), MessageType.message);
            messageUC.ShowDialog();
            StoryBoardHelper.BeginFadeIn(parent);
            LoadTicketInfo();
        }

        #endregion

        #region "Loaded"

        private void LoadTicketInfo()
        {
            this.txtHeader1.Text = SystemSettings.Ticket_Header_Line02.ToString();
            txtHeader2.Text = SystemSettings.Ticket_Header_Line03.ToString();
            txtHeader3.Text = SystemSettings.Ticket_Header_Line04.ToString();
            txtHeader4.Text = SystemSettings.Ticket_Header_Line05.ToString();
            txtHeader5.Text = SystemSettings.Ticket_Header_Line06.ToString();
            txtHeader6.Text = SystemSettings.Ticket_Header_Line07.ToString();
            chkCenterHeader.IsChecked = SystemSettings.Ticket_Header_CenterAlign;

            txtFooter1.Text = SystemSettings.Ticket_Footer_Line01.ToString();
            txtFooter2.Text = SystemSettings.Ticket_Footer_Line02.ToString();
            txtFooter3.Text = SystemSettings.Ticket_Footer_Line03.ToString();
            txtFooter4.Text = SystemSettings.Ticket_Footer_Line04.ToString();
            txtFooter5.Text = SystemSettings.Ticket_Footer_Line05.ToString();
            cmbTicketQuantity.Text = Cover.Backend.Configuration.SystemSettings.TicketCopies.ToString();
            chkCenterFooter.IsChecked = SystemSettings.Ticket_Footer_CenterAlign;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadTicketInfo();
            ShowPreview();
        }

        #endregion

        public void ShowPreview()
        {
            System.Windows.Forms.PrintPreviewDialog pripredlg = new System.Windows.Forms.PrintPreviewDialog();


            var cover = GetDummyCover();
            var printer = new TicketPrinter(cover);
            var printDocument = new PrintDocument();
            printDocument.PrintPage += printer.printDocument_PrintPage;
            var a = new System.Drawing.Printing.PaperSize("Custom Paper Size", 280, 600);
            printDocument.PrinterSettings.DefaultPageSettings.PaperSize = a;
            printDocument.DefaultPageSettings.PaperSize = a;

            pripredlg.Document = printDocument;
            
            var ct = pripredlg.PrintPreviewControl;
            ct.Zoom = 1;
            ct.BackColor = System.Drawing.Color.Black;
            ct.AutoZoom = true;

            //var bm = new System.Drawing.Bitmap(450, 450);
            //ct.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, 1500, 1500));
            //var imageInBytes = ImageToByte(bm);
            //var imageB64 = Convert.ToBase64String(imageInBytes);
            //this.ImgLogo.Source = Base64ToImage(imageInBytes);
        }

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

        private CoverData GetDummyCover()
        {
            var cover = new CoverData();
            cover.CoverDate = DateTime.Now;
            cover.CashierId = 1;
            cover.CoverDetails = new List<CoverDetail>();
            cover.CoverDetails.Add(new CoverDetail() { Quantity = 1, CustomerType = new CustomerType { Name = "Hombre" }, Subtotal = 43.10m });
            cover.CoverDetails.Add(new CoverDetail() { Quantity = 1, CustomerType = new CustomerType { Name = "Mujer" }, Subtotal = 43.10m });
            cover.Subtotal = 86.20m;
            cover.Iva = 13.80m;
            cover.Total = 100;
            cover.Payments = new List<Payment>();
            cover.Payments.Add(new Payment()
            {
                PaymentMethod = new PaymentMethod() { Id = 1, Name = "Efectivo" },
                TotalReceivedAmount = 100,
                Change = 0,
                TotalPaid = 100
            });
            return cover;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            var cover = GetDummyCover();
            var printer = new TicketPrinter(cover);
            printer.Print();
        }

    }

}
