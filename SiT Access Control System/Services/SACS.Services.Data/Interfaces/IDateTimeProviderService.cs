using System;

namespace SACS.Services.Data.Interfaces;

public interface IDateTimeProviderService
{
    /// <summary>
    /// Gets the current date and time.
    /// </summary>
    /// <returns>The current date and time.</returns>
    DateTime Now { get; }

    /// <summary>
    /// Gets the current date.
    /// </summary>
    /// <returns>The current date.</returns>
    DateTime Today { get; }

    /// <summary>
    /// Gets a value indicating whether the current date is a weekend.
    /// </summary>
    /// <returns>True if the current date is a weekend; otherwise, false.</returns>
    bool IsWeekend { get; }
}