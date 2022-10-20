using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using Zen.Barcode;
using Cover.Backend.Entities;
using Cover.Backend.BL;
using System.IO;

namespace Cover.Backend.Printer
{
    public class BraceletPrinter : PrinterBase
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


        private PrintDocument printDocument;
        private CoverDetail coverDetail { get; set; }
        private DateTime coverDate { get; set; }

        #endregion

        #region "Print"

        public void Print()
        {
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            printDocument.PrinterSettings.PrinterName = coverDetail.CoverConfiguration.Printer;
            //printDocument.DefaultPageSettings.PaperSize = new PaperSize("Bracelet", 100, 1000);
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Bracelet", 1000, 1000);
            printDocument.Print();
        }

        #endregion

        #region "PrintBracelet"

        public BraceletPrinter(CoverDetail coverDetail, DateTime coverDate)
        {
            this.coverDetail = coverDetail;
            this.coverDate = coverDate;
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            this.PrintPageEvent = e;

            this.PrintPageEvent.Graphics.Clear(SystemColors.Control);
            this.PrintPageEvent.Graphics.RotateTransform(90);

            Int32 x = 0;

            x += 360;
            var logo = logoService.GetLogo();
            var stream = new MemoryStream(Convert.FromBase64String(logo.Image));
            var image = new Bitmap(stream);
            stream.Flush();
            stream.Close();

            this.PrintPageEvent.Graphics.DrawImage(image, x, -105, 100, 100);

            x += 110;
            this.PrintPageEvent.Graphics.DrawString(coverDetail.CoverConfiguration.Name, FontBold, SystemBrushes.ControlText, x, -100);
            this.PrintPageEvent.Graphics.DrawString(String.Format("Vigencia desde {0} hasta {1}", coverDetail.ValidFrom.ToString("yyyy-MM-dd HH:mm"), coverDetail.ValidTo.ToString("yyyy-MM-dd HH:mm")), FontBold, SystemBrushes.ControlText, x, -85);
            this.PrintPageEvent.Graphics.DrawString(String.Format("Impresión {0}", DateTime.Now.ToString("yyMMddHHmm")), FontBold, SystemBrushes.ControlText, x, -70);
            this.PrintPageEvent.Graphics.DrawString(Cover.Backend.Configuration.SystemSettings.Bracelet_FreeText_Line1, FontBold, SystemBrushes.ControlText, x, -55);
            this.PrintPageEvent.Graphics.DrawString(Cover.Backend.Configuration.SystemSettings.Bracelet_FreeText_Line2, FontBold, SystemBrushes.ControlText, x, -40);
            this.PrintPageEvent.Graphics.DrawString(Cover.Backend.Configuration.SystemSettings.Bracelet_FreeText_Line3, FontBold, SystemBrushes.ControlText, x, -25);

            var barcodeHeight = 50;
            var bdraw = BarcodeDrawFactory.GetSymbology(BarcodeSymbology.Code128);
            var imageCb = bdraw.Draw(coverDetail.Barcode.ToString(), barcodeHeight);


            this.PrintPageEvent.Graphics.ResetTransform();

            x += 320;
            this.PrintPageEvent.Graphics.DrawImage(imageCb, 10, x, imageCb.Width, imageCb.Height);

            this.PrintPageEvent.Graphics.DrawString(coverDetail.Barcode.ToString(), FontBold, SystemBrushes.ControlText, 20, 850);
            
        }

        #endregion
    }
}
