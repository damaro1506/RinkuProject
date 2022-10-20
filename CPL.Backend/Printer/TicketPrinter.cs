using Cover.Backend.Configuration;
using Cover.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Printer
{
    public class TicketPrinter : PrinterBase
    {
        #region "Properties"

        public PrintDocument printDocument;
        public CoverData coverData { get; set; }

        #endregion

        public TicketPrinter(CoverData coverData)
        {
            this.coverData = coverData;
        }

        public void Print()
        {
            printDocument = new PrintDocument();
            printDocument.DocumentName = "Ticket - " + coverData.Id.ToString();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            printDocument.Print();
        }

        public void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            this.PrintPageEvent = e;

            AddHeader();

            AddTitle(ref y, "Ticket de pagado");

            AddSubheader();

            AddBody();

            AddTotals();

            AddPayments();

            AddFooter();
        }

        #region "Header"

        void AddHeader()
        {
            AddLineOfHeader(SystemSettings.Ticket_Header_Line01);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line02);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line03);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line04);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line05);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line06);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line07);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line08);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line09);
            AddLineOfHeader(SystemSettings.Ticket_Header_Line10);
        }

        void AddLineOfHeader(String text)
        {
            if (String.IsNullOrEmpty(text))
                return;

            var x = 0f;
            var align = Align.Left;

            if (SystemSettings.Ticket_Header_CenterAlign)
            {
                x = (X_TicketWidth * Scale) / 2;
                align = Align.Center;
            }

            AddLabel(text, x, y, align);
            AddJump(ref y);
        }

        #endregion

        #region "Subheader"

        void AddSubheader()
        {
            AddLabel("Fecha: ", 0, y, Align.Left);
            if (coverData.CoverDate != null)
                AddLabel(coverData.CoverDate.ToString("yyyy-MM-dd"), X_TicketWidth * Scale, y, Align.Right);
            AddJump(ref y);
            AddLabel("Cajero: ", 0, y, Align.Left);
            AddLabel(Cover.Backend.Context.User.Name, X_TicketWidth * Scale, y, Align.Right);
        }

        #endregion

        #region "Body"

        void AddBody()
        {
            AddLine(ref y);
            AddLabel("Cant.", 0, y, Align.Left);
            AddLabel("Descripción", (X_TicketWidth / 3) * Scale, y, Align.Left);
            AddLabel("Importe", X_TicketWidth * Scale, y, Align.Right);
            AddLine(ref y);

            var descriptionWidth = 180;
            var groupedCoverDetailsByCustomerType = coverData.CoverDetails.GroupBy(a => new { CustomerTypeName = a.CustomerType.Name, ProductName = a.CoverConfiguration.Name }).ToList();

            foreach (var item in groupedCoverDetailsByCustomerType)
            {
                AddLabel(item.Sum(a => a.Quantity).ToString(), 0, y, Align.Left);
                var description = String.Format("{0} - {1}", item.Key.CustomerTypeName, item.Key.ProductName);
                var measureString = this.PrintPageEvent.Graphics.MeasureString(description, FontBase);
                var rect_height = Math.Ceiling(measureString.Width / descriptionWidth) * measureString.Height + 15;
                var rect = new System.Drawing.RectangleF((X_TicketWidth / 10) * Scale, y, descriptionWidth, (float)rect_height);
                AddLabel(description, rect, Align.Left);
                AddLabel(item.Sum(a => a.Subtotal).ToString("C"), (X_TicketWidth * Scale), y, Align.Right);
                AddJump(ref y);
            }
            AddLine(ref y);
        }

        #endregion

        #region "Totals"

        void AddTotals()
        {

            AddLabel("Subtotal ", (X_TicketWidth / 2) * Scale, y, Align.Left);
            AddLabel(coverData.Subtotal.ToString("C"), X_TicketWidth * Scale, y, Align.Right);
            AddJump(ref y);
            AddLabel("IVA: ", (X_TicketWidth / 2) * Scale, y, Align.Left);
            AddLabel(coverData.Iva.ToString("C"), X_TicketWidth * Scale, y, Align.Right);
            AddJump(ref y);
            AddLabel("Total: ", (X_TicketWidth / 2) * Scale, y, Align.Left);
            AddLabel(coverData.Total.ToString("C"), X_TicketWidth * Scale, y, Align.Right);
            AddLine(ref y);
        }

        #endregion

        #region "Payments"

        void AddPayments()
        {
            AddLabel("Pago", 0, y, Align.Left);
            AddLabel("Total pagado MXN", (X_TicketWidth / 3) * Scale, y, Align.Left);
            AddLabel("Cambio", X_TicketWidth * Scale, y, Align.Right);
            var descriptionWidth = 180;
            AddJump(ref y);
            foreach (var payment in coverData.Payments)
            {

                AddLabel(payment.PaymentMethod.Name, 0, y, Align.Left);
                var description = String.Format("{0}", payment.TotalReceivedAmount.ToString("C"));
                var measureString = this.PrintPageEvent.Graphics.MeasureString(description, FontBase);
                var rect_height = Math.Ceiling(measureString.Width / descriptionWidth) * measureString.Height + 15;
                var rect = new System.Drawing.RectangleF((X_TicketWidth / 10) * Scale, y, descriptionWidth, (float)rect_height);
                AddLabel(description, rect, Align.Center);
                AddLabel(payment.Change.ToString("C"), (X_TicketWidth * Scale), y, Align.Right);
                AddJump(ref y);

            }

            AddLine(ref y);
        }

        void AddChange()
        {
            AddLabelBold("Cambio:", 0, y, Align.Left);
            AddJump(ref y);
            foreach (var payment in coverData.Payments)
            {
                if (payment.PaymentMethod.Id != 4)
                {
                    if (payment.Change > Convert.ToDecimal(0.00))
                    {
                        AddLabelBold(payment.PaymentMethod.Name + " " + FormatMoney(payment.Change), 0, y, Align.Left);
                        AddJump(ref y);
                    }
                }
            }
        }

        #endregion

        #region "Footer"

        void AddFooter()
        {
            AddLabel("Terminal: " + System.Environment.MachineName, 0, y, Align.Left);
            AddJump(ref y);
            AddLabel(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, y, Align.Left);
            AddJump(ref y);
            AddJump(ref y);

            AddLineOfFooter(SystemSettings.Ticket_Footer_Line01);
            AddLineOfFooter(SystemSettings.Ticket_Footer_Line02);
            AddLineOfFooter(SystemSettings.Ticket_Footer_Line03);
            AddLineOfFooter(SystemSettings.Ticket_Footer_Line04);
            AddLineOfFooter(SystemSettings.Ticket_Footer_Line05);
        }

        void AddLineOfFooter(String text)
        {
            if (String.IsNullOrEmpty(text))
                return;

            var x = 0f;
            var align = Align.Left;

            if (SystemSettings.Ticket_Footer_CenterAlign)
            {
                x = (X_TicketWidth * Scale) / 2;
                align = Align.Center;
            }

            AddLabel(text, x, y, align);
            AddJump(ref y);
        }

        #endregion



    }
}
