using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Projects.Commons.Windows;
using Projects.Commons;
using System.Windows;
using System;
using System.Data;
using System.Data.SqlClient;
using ProyectoCPL.Backend;
using ProyectoCPL.Backend.DataAccess;


namespace ProyectoCPL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Helper connection = new Helper();
        public MainWindow()
        {
            InitializeComponent();
            //Helper connection = new Helper();
            //connection.abrir();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var numEmployed = numeroEmpleado.Text;
            var nameEmployed = nombreEmpleado.Text;
            Console.WriteLine(numEmployed + "- ESTO -" + nameEmployed);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //connection.cerrar();
        }

        
        
        
        



        //#region "TestConnection"

        //private Boolean TestConnection()
        //{
        //    try
        //    {
        //        SqlConnection sqlConn  = new SqlConnection(AB.Backend.DataAccess.Helper.cplCS);
        //        sqlConn.Open();
        //        sqlConn.Close();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        //void coverSecurity_SaveConnection(String connectionString)
        //{
        //    AB.Backend.DataAccess.Helper.cplCS = connectionString;
        //}

        //#endregion
    }
}
