using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class CoverConfigurationPrice
    {
        public Int64 Id { get; set; }
        public Int64 CoverConfigurationId { get; set; }
        public DateTime? StartDate { get; set; }
        public Int16? StartDay { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public Int16? EndDay { get; set; }
        public TimeSpan EndTime { get; set; }
        public Decimal Price { get; set; }
        public Boolean Active { get; set; }

        public String StartDateDescription
        {
            get
            {
                if (StartDay.HasValue)
                    return WBM.Common.Helper.CommonFunctions.GetDayName(StartDay.Value) + " " + StartTime.Hours.ToString("00") + ":" + StartTime.Minutes.ToString("00");
                return StartDate.Value.ToString("yyyy-MM-dd") + " " + StartTime.Hours.ToString("00") + ":" + StartTime.Minutes.ToString("00");
            }
        }

        public String EndDateDescription
        {
            get
            {
                if (EndDay.HasValue)
                    return WBM.Common.Helper.CommonFunctions.GetDayName(EndDay.Value) + " " + EndTime.Hours.ToString("00") + ":" + EndTime.Minutes.ToString("00");
                return EndDate.Value.ToString("yyyy-MM-dd") + " " + EndTime.Hours.ToString("00") + ":" + EndTime.Minutes.ToString("00");
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return Helper.Dates.GetStartDateTime(StartDay, StartDate, StartTime, EndDay, EndDate, EndTime);
            }
        }

        public DateTime EndDateTime
        {
            get
            {
                return Helper.Dates.GetEndDateTime(StartDay, StartDate, StartTime, EndDay, EndDate, EndTime);
            }
        }

    }
}
