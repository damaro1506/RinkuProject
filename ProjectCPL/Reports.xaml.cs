using Cover.Backend.BL;
using Cover.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using NsExcel = Microsoft.Office.Interop.Excel;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Windows.Forms;
using Cover.POS.Controls;
using WBM.Common.Windows;
using WBM.Common;

namespace Cover.POS
{
    public partial class Reports : Window
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

        private CoverConfigurationService _coverconfigurationService;
        private CoverConfigurationService coverconfigurationService
        {
            get
            {
                if (_coverconfigurationService == null)
                    _coverconfigurationService = new CoverConfigurationService();
                return _coverconfigurationService;
            }
            set
            {
                _coverconfigurationService = value;
            }
        }
        private CourtesyService _courtesyService;
        private CourtesyService courtesyService
        {
            get
            {
                if (_courtesyService == null)
                    _courtesyService = new CourtesyService();
                return _courtesyService;
            }
            set
            {
                _courtesyService = value;
            }
        }

        #endregion

        #region "Constructor"

        public Reports()
        {
            InitializeComponent();
            Window.GetWindow(this).Closing += MainWindow_Closing;
            dpStartDate.Loaded += delegate
            {
                var textBox1 = (System.Windows.Controls.TextBox)dpStartDate.Template.FindName("PART_TextBox", dpStartDate);
                textBox1.Background = dpStartDate.Background;

                var textBox2 = (System.Windows.Controls.TextBox)dpEndDate.Template.FindName("PART_TextBox", dpEndDate);
                textBox2.Background = dpEndDate.Background;
            };
        }

        #endregion

        #region "Loaded"

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TopBar.UserName = Cover.Backend.Context.User.Name;
            TopBar.OperationDate = Cover.Backend.Context.OperationDate;
        }

        #endregion

        #region "Closing"

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.TopBar.Dispose();
            GC.Collect();
        }

        #endregion

        #region "Button_Actions"

        private void btnLockReports_Click(object sender, RoutedEventArgs e)
        {
            var frm = new Login();
            frm.Show();
            this.Close();
        }

        private void btnExitReports_Click(object sender, RoutedEventArgs e)
        {
            var frm = new MainWindow();
            frm.Show();
            this.Close();
        }

        private void btnReportOne_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDateTime(dpStartDate.Text) <= Convert.ToDateTime(dpEndDate.Text))
                CreateExcelReport();
            else
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("La fecha de inicio no puede ser mayor a la fecha fin"), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
        }

        #endregion

        #region "Report_Events"

        public void CreateExcelReport()
        {
            var startDate = dpStartDate.SelectedDate.Value;
            var endDate = dpEndDate.SelectedDate.Value.AddDays(1);

            var coverDatas = coverdataService.GetCoverDataByRangeOfDate(startDate, endDate);
            var courtesies = courtesyService.GetCourtesysByRangeOfDate(startDate, endDate);

            var fileStream = new FileStream(String.Format("{0}Excel/ReporteVentasCovers.xls", AppDomain.CurrentDomain.BaseDirectory), FileMode.Open, FileAccess.Read);
            var templateWorkbook = new HSSFWorkbook(fileStream, true);

            var sheet1 = templateWorkbook.GetSheet("Venta por día");
            Int32 rowNumber = 8;
            var coversGroupedByOperationDate = coverDatas.GroupBy(a => a.OperationDate).OrderBy(a => a.Key);

            sheet1.CreateRow(1).CreateCell(5).SetCellValue((String.Format("{0} a {1}", dpStartDate.Text, dpEndDate.Text)));
            sheet1.CreateRow(2).CreateCell(5).SetCellValue(String.Format("{0}", coverDatas.Sum(a => Convert.ToInt32(a.CoverDetails.Sum(b => b.Quantity)))));
            sheet1.CreateRow(3).CreateCell(5).SetCellValue((Double)coverDatas.Sum(a => a.Subtotal));
            sheet1.CreateRow(4).CreateCell(5).SetCellValue((Double)coverDatas.Sum(a => a.Iva));
            sheet1.CreateRow(5).CreateCell(5).SetCellValue((Double)coverDatas.Sum(a => a.Total));

            foreach (var item in coversGroupedByOperationDate)
            {
                var row = sheet1.CreateRow(rowNumber);
                row.CreateCell(1).SetCellValue(item.Key.ToString("yyyy-MM-dd"));
                row.CreateCell(2).SetCellValue(item.Sum(a => Convert.ToInt32(a.CoverDetails.Sum(b => b.Quantity))));
                row.CreateCell(3).SetCellValue((Double)item.Sum(a => a.Subtotal));
                row.CreateCell(4).SetCellValue((Double)item.Sum(a => a.Iva));
                row.CreateCell(5).SetCellValue((Double)item.Sum(a => a.Total));
                rowNumber += 1;
            }

            var sheet2 = templateWorkbook.GetSheet("Venta por tipo de cover y fecha");
            rowNumber = 2;
            foreach (var item in coversGroupedByOperationDate)
            {
                var coverDetailsGroupedByCoverConfigurationAndDate = coverDatas.Where(a => a.OperationDate == item.Key).SelectMany(a => a.CoverDetails).GroupBy(a => new { a.CoverConfiguration.StartDateDescription, a.CoverConfiguration.EndDateDescription, CustomerName = a.CoverConfiguration.CustomerType.Name, a.UnitPrice, a.CoverConfiguration.Name }).ToList();
                foreach (var cd in coverDetailsGroupedByCoverConfigurationAndDate)
                {
                    var row = sheet2.CreateRow(rowNumber);
                    row.CreateCell(1).SetCellValue(item.Key.ToString("yyyy-MM-dd"));
                    row.CreateCell(2).SetCellValue(cd.Key.StartDateDescription);
                    row.CreateCell(3).SetCellValue(cd.Key.EndDateDescription);
                    row.CreateCell(4).SetCellValue(cd.Key.CustomerName);
                    row.CreateCell(5).SetCellValue(cd.Key.Name);
                    row.CreateCell(6).SetCellValue((Double)cd.Key.UnitPrice);
                    row.CreateCell(7).SetCellValue((Double)cd.Count());
                    row.CreateCell(8).SetCellValue((Double)cd.Sum(a => a.Subtotal));
                    row.CreateCell(9).SetCellValue((Double)cd.Sum(a => a.Iva));
                    row.CreateCell(10).SetCellValue((Double)cd.Sum(a => a.Total));
                    rowNumber += 1;
                }
            }

            var sheet3 = templateWorkbook.GetSheet("Venta por tipo de cover");
            rowNumber = 2;
            var coverDetailsGroupedByCoverConfiguration = coverDatas.SelectMany(a => a.CoverDetails).GroupBy(a => new { a.CoverConfiguration.StartDateDescription, a.CoverConfiguration.EndDateDescription, CustomerName = a.CoverConfiguration.CustomerType.Name, a.UnitPrice, a.CoverConfiguration.Name }).ToList();
            foreach (var cd in coverDetailsGroupedByCoverConfiguration)
            {
                var row = sheet3.CreateRow(rowNumber);
                row.CreateCell(1).SetCellValue(cd.Key.StartDateDescription);
                row.CreateCell(2).SetCellValue(cd.Key.EndDateDescription);
                row.CreateCell(3).SetCellValue(cd.Key.CustomerName);
                row.CreateCell(4).SetCellValue(cd.Key.Name);
                row.CreateCell(5).SetCellValue((Double)cd.Key.UnitPrice);
                row.CreateCell(6).SetCellValue((Double)cd.Count());
                row.CreateCell(7).SetCellValue((Double)cd.Sum(a => a.Subtotal));
                row.CreateCell(8).SetCellValue((Double)cd.Sum(a => a.Iva));
                row.CreateCell(9).SetCellValue((Double)cd.Sum(a => a.Total));
                rowNumber += 1;
            }

            var sheet4 = templateWorkbook.GetSheet("Venta por forma de pago");
            rowNumber = 2;
            var coverPaymentsGroupedByPaymentMethods = coverDatas.SelectMany(a => a.Payments).GroupBy(a => new { OperationDate = a.OperationDate, PaymentMethodName = a.PaymentMethod.Name }).ToList();
            foreach (var item in coverPaymentsGroupedByPaymentMethods)
            {
                var row = sheet4.CreateRow(rowNumber);
                row.CreateCell(1).SetCellValue(item.Key.OperationDate.ToString("yyyy-MM-dd"));
                row.CreateCell(2).SetCellValue(item.Key.PaymentMethodName);
                row.CreateCell(3).SetCellValue((Double)item.Sum(a => a.TotalPaid));
                rowNumber += 1;
            }

            var sheet5 = templateWorkbook.GetSheet("Detalle de ventas");
            rowNumber = 2;
            foreach (var coverData in coverDatas)
            {
                foreach (var detail in coverData.CoverDetails)
                {
                    var row = sheet5.CreateRow(rowNumber);
                    row.CreateCell(1).SetCellValue(coverData.OperationDate.ToString("yyyy-MM-dd"));
                    row.CreateCell(2).SetCellValue(coverData.CoverDate.ToString("yyyy-MM-dd HH:mm"));
                    row.CreateCell(3).SetCellValue(detail.CoverConfiguration.Name);
                    row.CreateCell(4).SetCellValue(detail.CustomerType.Name);
                    row.CreateCell(5).SetCellValue((Double)detail.UnitPrice);
                    row.CreateCell(6).SetCellValue((Double)detail.Subtotal);
                    row.CreateCell(7).SetCellValue((Double)detail.Iva);
                    row.CreateCell(8).SetCellValue((Double)detail.Total);
                    row.CreateCell(9).SetCellValue(detail.ValidFrom.ToString("yyyy-MM-dd HH:mm"));
                    row.CreateCell(10).SetCellValue(detail.ValidTo.ToString("yyyy-MM-dd HH:mm"));
                    row.CreateCell(11).SetCellValue(detail.Barcode);
                    rowNumber += 1;
                }
            }

            var sheet6 = templateWorkbook.GetSheet("Cortesias");
            rowNumber = 2;
            foreach (var item in courtesies)
            {
                var row = sheet6.CreateRow(rowNumber);
                row.CreateCell(1).SetCellValue(item.CourtesyDate.ToString("yyyy-MM-dd"));
                row.CreateCell(2).SetCellValue(item.CoverConfiguration.CustomerType.Name);
                row.CreateCell(3).SetCellValue(item.Quantity);
                row.CreateCell(4).SetCellValue(item.Reason);
                row.CreateCell(5).SetCellValue(item.User.Name);
                row.CreateCell(6).SetCellValue((Double)item.Total);
                rowNumber += 1;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xls)|*.xls";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "xls";
            saveFileDialog.FileName = "Reporte de ventas " + DateTime.Now.ToString("yyyyMMdd HHmmss");

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var memoryStream = new MemoryStream();
                Stream fileStream2 = saveFileDialog.OpenFile();
                templateWorkbook.Write(memoryStream);
                memoryStream.Position = 0;
                memoryStream.WriteTo(fileStream2);
                fileStream2.Close();
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("Se guardó el reporte en la ruta seleccionada"), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
            else
            {
                StoryBoardHelper.BeginFadeOut(parent);
                var messageUC = new Message_Window(String.Format("Debe seleccionar una ruta para poder generar el reporte."), MessageType.message);
                messageUC.ShowDialog();
                StoryBoardHelper.BeginFadeIn(parent);
            }
        }

        #endregion

    }
}


