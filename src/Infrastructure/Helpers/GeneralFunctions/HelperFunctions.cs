using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.enums;

namespace Infrastructure.Helpers.GeneralFunctions
{
    public class HelperFunctions
    {
        public DateOnly GetNextWeekday(string day)
        {
            DateTime currentDate = DateTime.UtcNow;
            DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;

            DayOfWeek targetDay = Enum.Parse<DayOfWeek>(day);

            // Calculate the number of days until the target day in the next week
            int daysUntilTargetDay = ((int)targetDay - (int)currentDayOfWeek + 7) % 7;

            // Add the days to the current date to get the target day in the next week
            DateTime nextWeekday = currentDate.AddDays(daysUntilTargetDay);

            return DateOnly.FromDateTime(nextWeekday);
        }
    }
}
