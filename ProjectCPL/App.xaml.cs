using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WBM.Common.Helper;

namespace Cover.POS
{
    public partial class App : Application
    {

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Taskbar.Show();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Taskbar.Hide();
            Application.Current.Resources["Color_Primary"] = Colors.Crimson;
            Application.Current.Resources["Color_PrimaryFont"] = Colors.White;
            Application.Current.Resources["Color_SecondaryFont"] = Colors.White;

            var mainDictionary = Application.Current.Resources.MergedDictionaries.First();
            foreach (var entry in mainDictionary.Keys)
            {
                if (entry.ToString().StartsWith("Img"))
                    Application.Current.Resources[entry] = BitmapFrame.Create(new Uri(Application.Current.Resources[entry].ToString().Replace("Black", "White"), UriKind.RelativeOrAbsolute));
            }

        }
    }
}
