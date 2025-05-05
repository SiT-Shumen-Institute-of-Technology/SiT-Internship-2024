using System;
using SACS.Services.Data.Interfaces;

namespace SACS.Services.Data
{
    public class DateTimeProviderService : IDateTimeProviderService
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// Gets the current date.
        /// </summary>
        public DateTime Today => DateTime.Today;

        /// <summary>
        /// Gets a value indicating whether the current date is a weekend.
        /// </summary>
        public bool IsWeekend => Now.DayOfWeek == DayOfWeek.Saturday || Now.DayOfWeek == DayOfWeek.Sunday;
    }
}
