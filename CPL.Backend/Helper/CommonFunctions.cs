using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Helper
{
    public class CommonFunctions
    {
        public static String GetDayName(Int16 dayNumber)
        {
            switch (dayNumber)
            {
                case 0: return "Domingo";
                case 1: return "Lunes";
                case 2: return "Martes";
                case 3: return "Miércoles";
                case 4: return "Jueves";
                case 5: return "Viernes";
                case 6: return "Sábado";
                default: return "";
            }
        }
    }
}
