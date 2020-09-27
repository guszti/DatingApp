using System;

namespace DatingApp.API.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalculateAge(this DateTime dateTime)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateTime.Year;
    
            if (dateTime.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}