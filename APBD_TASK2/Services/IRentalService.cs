using APBD_TASK2.Models;

namespace APBD_TASK2.Services;

/// <summary>
/// The rental business logic: renting, returning and querying rentals.
/// Reporting deliberately lives in its own <see cref="IReportService"/> so this
/// interface stays focused (Interface Segregation) and highly cohesive.
/// </summary>
public interface IRentalService
{
    Rental Rent(User user, Equipment equipment, DateTime rentedAt, int durationDays);

    /// <summary>Returns the equipment and yields the penalty that was charged (0 if on time).</summary>
    decimal Return(Equipment equipment, DateTime returnedAt);

    IReadOnlyList<Rental> GetActiveRentals(User user);
    IReadOnlyList<Rental> GetOverdueRentals(DateTime asOf);
    IReadOnlyList<Rental> GetAllRentals();
}
