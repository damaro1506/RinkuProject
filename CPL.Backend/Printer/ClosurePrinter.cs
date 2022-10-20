using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Cover.Backend.Entities;
using Cover.Backend.BL;


namespace Cover.Backend.Printer
{
    public class ClosurePrinter : PrinterBase
    {

        #region Properties

        private CoverDataService _coverdataService;
        private CoverDataService coverdataService
        {
            get
            {
                if (_coverdataService == null)
                    _coverdataService = new CoverDataService();
                return _coverdataService;
            }
            set
            {
                _coverdataService = value;
            }
        }

        private CashRegisterOperationService _cashregisteroperationService;
        private CashRegisterOperationService cashregisteroperationService
        {
            get
            {
                if (_cashregisteroperationService == null)
                    _cashregisteroperationService = new CashRegisterOperationService();
                return _cashregisteroperationService;
            }
            set
            {
                _cashregisteroperationService = value;
            }
        }

        private CoverDetailService _coverdetailService;
        private CoverDetailService coverdetailService
        {
            get
            {
                if (_coverdetailService == null)
                    _coverdetailService = new CoverDetailService();
                return _coverdetailService;
            }
            set
            {
                _coverdetailService = value;
            }
        }

        private PaymentService _paymentService;
        private PaymentService paymentService
        {
            get
            {
                if (_paymentService == null)
                    _paymentService = new PaymentService();
                return _paymentService;
            }
            set
            {
                _paymentService = value;
            }
        }

        private PrintDocument printDocument;
        private DateTime ClosureDate;
        private String ClosureName { get; set; }
        private List<CashRegisterOperation> CashRegisterOps { get; set; }
        private List<CoverData> CoverDatas { get; set; }
        private Decimal cash { get; set; }

        #endregion

        #region "Printer_Events"

        public ClosurePrinter(DateTime operationDate, ClosureType type)
        {
            ClosureDate = Cover.Backend.Context.OperationDate.Date;
            ClosureName = type == ClosureType.Z ? "Corte Z" : "Corte Global";
            if (type == Backend.ClosureType.Z)
                CoverDatas = coverdataService.GetCoverDataByOperationDate(operationDate).Where(a=> a.TerminalId == Cover.Backend.Context.Terminal.Id).ToList();
            else
                CoverDatas = coverdataService.GetCoverDataByOperationDate(operationDate);
        }

        public void Print()
        {
            try
            {
                printDocument = new PrintDocument();
                printDocument.DocumentName = ClosureName + " - " + ClosureDate.ToString("yyyy-MM-dd");
                printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
                printDocument.Print();
            }
            catch (Exception e)
            {
            }
        }

        public void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                this.y = 0;

                this.PrintPageEvent = e;

                AddHeader();

                AddGeneralTotals();

                AddPaymentSales();

                AddCashInfo();

                AddCoverDetailInfo();

                AddCoverConfigurationInfo();

                AddPrintDate();
            }
            catch (Exception ex)
            {
                
            }
        }

        #endregion

        #region "Ticket_Events"

        public void AddHeader()
        {
            AddLabelBold(Cover.Backend.Configuration.SystemSettings.Ticket_Header_Line01, (X_TicketWidth * Scale) / 2, y, Align.Center);
            AddJump(ref y);
            AddLabelBold(ClosureName, (X_TicketWidth * Scale) / 2, y, Align.Center);
            AddJump(ref y);
            AddLine(ref y);
            AddLabel("Fecha:", 0, y, Align.Left);
            AddLabel(ClosureDate.ToString("yyyy-MM-dd"), (X_TicketWidth * Scale), y, Align.Right);
            AddJump(ref y);
            AddLabel("Terminal:", 0, y, Align.Left);
            AddLabel(Cover.Backend.Context.Terminal.Id.ToString(), (X_TicketWidth * Scale), y, Align.Right);
            AddJump(ref y);
            AddLabel("Cajero:", 0, y, Align.Left);
            AddLabel(Cover.Backend.Context.User.Name, (X_TicketWidth * Scale), y, Align.Right);
        }

        public void AddGeneralTotals()
        {
            decimal total = CoverDatas.Sum(a => a.Total);
            decimal subtotal = CoverDatas.Sum(a => a.Subtotal);
            decimal iva = CoverDatas.Sum(a => a.Iva);
            

            AddTitle(ref y, "TOTALES GENERALES");
            AddLabel("Total de venta sin impuestos:", 0, y, Align.Left);
            AddLabel(FormatMoney(subtotal), (X_TicketWidth * Scale), y, Align.Right);
            AddJump(ref y);
            AddLabel("IVA: ", 0, y, Align.Left);
            AddLabel(FormatMoney(iva), (X_TicketWidth * Scale), y, Align.Right);
            AddJump(ref y);
            AddLabelBold("Total de venta con impuestos: ", 0, y, Align.Left);
            AddLabelBold(FormatMoney(total), (X_TicketWidth * Scale), y, Align.Right);
        }

        public void AddPaymentSales()
        {
            AddTitle(ref y, "VENTAS POR FORMA DE PAGO");

            var payments = CoverDatas.SelectMany(a => a.Payments).ToList();
            var groupedPaymentsByPaymentMethod = payments.GroupBy(a => new { a.PaymentMethod.Name, a.PaymentMethod.Id }).ToList();
            var groupedPaymentMethodByEquivalence = payments.GroupBy(a => new { a.PaymentMethod.Name, a.PaymentMethod.Id, a.PaymentMethod.Equivalence }).ToList();

            foreach (var item in groupedPaymentMethodByEquivalence)
            {
                if (item.Key.Id > 3)
                {
                    var amounts = item.Sum(a => a.ReceivedAmount);
                    AddLabel(String.Format("{0} ({1})*({2})", item.Key.Name, Math.Round(amounts)/item.Key.Equivalence, item.Key.Equivalence.ToString("C")), 0, y, Align.Left);
                    AddLabel(item.Sum(a => a.TotalPaid).ToString("C"), (X_TicketWidth * Scale), y, Align.Right);
                    AddJump(ref y);
                }

            }
            foreach (var item in groupedPaymentsByPaymentMethod)
            {
                if (item.Key.Id < 4)
                {
                    AddLabel(String.Format("{0}", item.Key.Name), 0, y, Align.Left);
                    AddLabel(item.Sum(a => a.TotalPaid).ToString("C"), (X_TicketWidth * Scale), y, Align.Right);
                    AddJump(ref y);
                }
            }

            AddLabelBold("Total por forma de pago:", 0, y, Align.Left);
            AddLabelBold(payments.Sum(a => a.TotalPaid).ToString("C"), (X_TicketWidth * Scale), y, Align.Right);
        }

        public void AddCashInfo()
        {
            AddTitle(ref y, "EFECTIVO REAL");

            var payments = CoverDatas.SelectMany(a => a.Payments).ToList();
            var groupedPaymentsByPaymentMethod = payments.GroupBy(a => new { a.PaymentMethod.Name, a.PaymentMethod.Id }).ToList();
            var fund = cashregisteroperationService.GetLastOpenCashRegisterOperationByTerminal();

            foreach (var item in groupedPaymentsByPaymentMethod)
            {
                if (item.Key.Id == 1)
                {
                    AddLabel(String.Format("{0}", item.Key.Name), 0, y, Align.Left);
                    AddLabel(item.Sum(a => a.TotalPaid).ToString("C"), (X_TicketWidth * Scale), y, Align.Right);
                    AddJump(ref y);
                    cash = item.Sum(a => a.TotalPaid);
                }
            }

            AddLabel("Fondo de caja:", 0, y, Align.Left);
            AddLabel(FormatMoney(fund.CashFund), (X_TicketWidth * Scale), y, Align.Right);
            AddJump(ref y);
            AddLabelBold("Efectivo total en caja:", 0, y, Align.Left);
            AddLabelBold(FormatMoney(fund.CashFund + cash), (X_TicketWidth * Scale), y, Align.Right);
        }

        public void AddCoverDetailInfo()
        {
            AddTitle(ref y, "VENTAS POR TIPO DE CLIENTE");

            var coverDetails = CoverDatas.SelectMany(a => a.CoverDetails).ToList();
            var groupedCoverDetailsByCustomerType = coverDetails.GroupBy(a => a.CustomerType.Name).ToList();

            foreach (var item in groupedCoverDetailsByCustomerType)
            {
                AddLabel(item.Key + ": (" + item.Sum(a => a.Quantity).ToString() + ")", 0, y, Align.Left);
                AddLabel(item.Sum(a => a.Total).ToString("C"), (X_TicketWidth * Scale), y, Align.Right);
                AddJump(ref y);
            }

            AddLabelBold(String.Format("Total de personas: ({0})", coverDetails.Sum(a => a.Quantity).ToString()), 0, y, Align.Left);
            AddLabelBold(FormatMoney(CoverDatas.Sum(a => a.Total)), (X_TicketWidth * Scale), y, Align.Right);
        }

        public void AddCoverConfigurationInfo()
        {
            AddTitle(ref y, "VENTAS POR TIPO DE COVER");

            var coverDetails = CoverDatas.SelectMany(a => a.CoverDetails).ToList();
            var groupedCoverDetailsByCoverConfiguration = coverDetails.GroupBy(a => new { a.CoverConfiguration.CustomerType.Name, a.CoverConfiguration.StartDateTime, a.CoverConfiguration.EndDateTime }).ToList();

            foreach (var item in groupedCoverDetailsByCoverConfiguration)
            {
                AddLabel(String.Format("{0}-{1}-{2}: ({3})", item.Key.StartDateTime.ToString("HH:mm"), item.Key.EndDateTime.ToString("HH:mm"), item.Key.Name, item.Sum(a => a.Quantity)), 0, y, Align.Left);
                AddLabel(item.Sum(a => a.Total).ToString("C"), (X_TicketWidth * Scale), y, Align.Right);
                AddJump(ref y);
            }
        }

        public void AddPrintDate()
        {
            AddLine(ref y);
            AddLabel("Fecha de impresión: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, y, Align.Left);
        }

        #endregion
    }
}
