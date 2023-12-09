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

        public int CalculatePercentageDiscount(int originalValue, int discountValue)
        {
            // Ensure the discount percentage is within a valid range (0 to 100)
            int discountPercentage = Math.Max(0, Math.Min(100, discountValue));

            // Calculate
            decimal discountAmount = ((decimal)discountPercentage / 100) * originalValue;
            decimal finalPrice = originalValue - discountAmount;

            return (int)finalPrice;
        }
    }
}
