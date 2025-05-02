namespace SACS.Services.Data
{
    using System;

    using SACS.Services.Data.Interfaces;

    public class DateTimeProviderService : IDateTimeProviderService
    {
        public DateTime Today => DateTime.Today;
    }
}
