using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Helper
{
    public class Dates
    {
        public static DateTime GetStartDateTime(Int16? StartDay, DateTime? StartDate, TimeSpan StartTime, Int16? EndDay, DateTime? EndDate, TimeSpan EndTime)
        {
            if (!StartDay.HasValue)
                return new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, StartTime.Hours, StartTime.Minutes, 0);

            DateTime sunday;
            var d = Helper.Dates.GetEndDateTime(StartDay, StartDate, StartTime, EndDay, EndDate, EndTime); ;

            if (EndDay < StartDay) //Es en diferentes semanas
            {
                if ((short)DateTime.Now.DayOfWeek >= 0 && (short)DateTime.Now.DayOfWeek < EndDay)
                    sunday = GetPenultimateSunday();//1-DS
                else if ((short)DateTime.Now.DayOfWeek >= 0 && (short)DateTime.Now.DayOfWeek == EndDay && DateTime.Now.TimeOfDay < EndTime)
                    sunday = GetPenultimateSunday();//2-DS
                else if ((short)DateTime.Now.DayOfWeek < StartDay && (short)DateTime.Now.DayOfWeek < EndDay)
                    sunday = GetLastSunday();//3-DS
                else if ((short)DateTime.Now.DayOfWeek < StartDay && (short)DateTime.Now.DayOfWeek > EndDay)
                    sunday = GetLastSunday();//4-DS
                else if ((short)DateTime.Now.DayOfWeek < StartDay && (short)DateTime.Now.DayOfWeek == EndDay && DateTime.Now.TimeOfDay > EndTime)
                    sunday = GetLastSunday();//5-DS
                else if ((short)DateTime.Now.DayOfWeek < StartDay && (short)DateTime.Now.DayOfWeek == EndDay && DateTime.Now.TimeOfDay < EndTime)
                    sunday = GetLastSunday();//6-DS
                else if ((short)DateTime.Now.DayOfWeek == StartDay && DateTime.Now.TimeOfDay > StartTime)
                    sunday = GetLastSunday();//7-DS
                else
                    sunday = GetLastSunday();//8-DS
            }
            else if (EndDay == StartDay)
            {
                if (StartTime > EndTime) //Es una semana completa
                {
                    if ((short)DateTime.Now.DayOfWeek == StartDay && StartDay == 0 && DateTime.Now.TimeOfDay < EndTime)
                        sunday = GetPenultimateSunday();//1-MD-SC
                    else if (StartDay == 0)
                        sunday = GetLastSunday();//2-MD-SC
                    else if ((short)DateTime.Now.DayOfWeek == StartDay && StartDay != 0 && DateTime.Now.TimeOfDay < EndTime)
                        sunday = GetPenultimateSunday();//3-MD-SC
                    else if ((short)DateTime.Now.DayOfWeek == StartDay)
                        sunday = GetLastSunday();//4-MD-SC
                    else
                        sunday = GetPenultimateSunday();//5-MD-SC
                }
                else//es el mismo día
                {
                    if ((short)DateTime.Now.DayOfWeek == StartDay && DateTime.Now.TimeOfDay > EndTime && StartDay != 0)
                        sunday = GetNextSunday();//1-MD
                    else if ((short)DateTime.Now.DayOfWeek == StartDay && DateTime.Now.TimeOfDay > EndTime && StartDay == 0)
                        sunday = GetNextNextSunday();//6-MD
                    else if ((short)DateTime.Now.DayOfWeek == StartDay && DateTime.Now.TimeOfDay < EndTime)
                        sunday = GetLastSunday();//2-MD
                    else if ((short)DateTime.Now.DayOfWeek < StartDay)
                        sunday = GetLastSunday();//3-MD
                    else if ((short)DateTime.Now.DayOfWeek > StartDay)
                        sunday = GetNextSunday();//4-MD
                    else
                        sunday = GetLastSunday();//5-MD
                }
            }
            else //if (EndDay > StartDay) diferente día misma semana
            {
                if ((short)DateTime.Now.DayOfWeek > EndDay)
                    sunday = GetNextSunday();//1-MS
                else if ((short)DateTime.Now.DayOfWeek == EndDay && DateTime.Now.TimeOfDay > EndTime)
                    sunday = GetNextSunday();//2-MS
                else
                    sunday = GetLastSunday();//3-MS
            }
            return new DateTime(sunday.AddDays((double)StartDay).Year, sunday.AddDays((double)StartDay).Month, sunday.AddDays((double)StartDay).Day, StartTime.Hours, StartTime.Minutes, 0);
        }

        public static DateTime GetEndDateTime(Int16? StartDay, DateTime? StartDate, TimeSpan StartTime, Int16? EndDay, DateTime? EndDate, TimeSpan EndTime)
        {
            if (!EndDay.HasValue)
                return new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, EndTime.Hours, EndTime.Minutes, 0);

            DateTime sunday;

            if (EndDay < StartDay)//brinco de semana
            {
                if ((short)DateTime.Now.DayOfWeek >= 0 && (short)DateTime.Now.DayOfWeek < EndDay)
                    sunday = GetLastSunday();//1-DS
                else if ((short)DateTime.Now.DayOfWeek >= 0 && (short)DateTime.Now.DayOfWeek == EndDay && DateTime.Now.TimeOfDay < EndTime)
                    sunday = GetLastSunday();//2-DS
                else if ((short)DateTime.Now.DayOfWeek >= 0 && (short)DateTime.Now.DayOfWeek == EndDay && DateTime.Now.TimeOfDay > EndTime && EndDay == 0)
                    sunday = GetNextNextSunday();//3-DS
                else
                    sunday = GetNextSunday();//4-DS
            }
            else if (EndDay == StartDay)
            {
                if (StartTime > EndTime) //Es una semana completa
                {
                    if ((short)DateTime.Now.DayOfWeek == StartDay && StartDay == 0 && DateTime.Now.TimeOfDay < EndTime)
                        sunday = GetNextSunday();//1-MD-SC
                    else if ((short)DateTime.Now.DayOfWeek == StartDay && StartDay == 0)
                        sunday = GetNextNextSunday();//2-MD-SC
                    else if (StartDay == 0)
                        sunday = GetNextSunday();//3-MD-SC
                    else if ((short)DateTime.Now.DayOfWeek == StartDay && StartDay != 0 && DateTime.Now.TimeOfDay < EndTime)
                        sunday = GetLastSunday();//4-MD-SC
                    else if ((short)DateTime.Now.DayOfWeek == StartDay)
                        sunday = GetNextSunday();//5-MD-SC
                    else
                        sunday = GetLastSunday();//6-MD-SC
                }
                else //es el mismo día
                {
                    if ((short)DateTime.Now.DayOfWeek == StartDay && DateTime.Now.TimeOfDay > EndTime && StartDay != 0)
                        sunday = GetNextSunday();//1-MD
                    else if ((short)DateTime.Now.DayOfWeek == StartDay && DateTime.Now.TimeOfDay > EndTime && StartDay == 0)
                        sunday = GetNextNextSunday();//6-MD
                    else if ((short)DateTime.Now.DayOfWeek == StartDay && DateTime.Now.TimeOfDay < EndTime)
                        sunday = GetLastSunday();//2-MD
                    else if ((short)DateTime.Now.DayOfWeek < StartDay)
                        sunday = GetLastSunday();//3-MD
                    else if ((short)DateTime.Now.DayOfWeek > StartDay)
                        sunday = GetNextSunday();//4-MD
                    else
                        sunday = GetLastSunday();//5-MD
                }
            }
            else //if (EndDay > StartDay) diferente día misma semana
            {
                if ((short)DateTime.Now.DayOfWeek > EndDay)
                    sunday = GetNextSunday();//1-MS
                else if ((short)DateTime.Now.DayOfWeek == EndDay && DateTime.Now.TimeOfDay > EndTime)
                    sunday = GetNextSunday();//2-MS
                else
                    sunday = GetLastSunday();//3-MS
            }
            return new DateTime(sunday.AddDays((double)EndDay).Year, sunday.AddDays((double)EndDay).Month, sunday.AddDays((double)EndDay).Day, EndTime.Hours, EndTime.Minutes, 0);
        }

        public static DateTime GetLastSunday()
        {
            var currentDate = DateTime.Now;
            while (currentDate.DayOfWeek != 0)
                currentDate = currentDate.AddDays(-1);
            return currentDate;
        }

        public static DateTime GetPenultimateSunday()
        {
            var currentDate = DateTime.Now;
            var count = 0;
            while (count < 2)
            {
                if (currentDate.DayOfWeek == 0)
                {
                    if (count == 1)
                        break;
                    count++;
                }
                currentDate = currentDate.AddDays(-1);
            }
            return currentDate;
        }

        public static DateTime GetNextSunday()
        {
            var currentDate = DateTime.Now;
            while (currentDate.DayOfWeek != 0)
                currentDate = currentDate.AddDays(1);
            return currentDate;
        }

        public static DateTime GetNextNextSunday()
        {
            var currentDate = DateTime.Now;
            var count = 0;
            while (count < 2)
            {
                if (currentDate.DayOfWeek == 0)
                {
                    if (count == 1)
                        break;
                    count++;
                }
                currentDate = currentDate.AddDays(1);
            }
            return currentDate;
        }
    }
}
