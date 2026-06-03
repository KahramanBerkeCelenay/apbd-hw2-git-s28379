using APBD_TASK2.Models;

namespace APBD_TASK2.Policies;

/// <summary>
/// The default rule: a flat fee for every day a return is late. The daily rate is
/// a constructor parameter, so changing the fee (or charging zero in a "grace
/// period" build) is a one-line change in one place.
/// </summary>
public sealed class LatePenaltyPolicy(decimal ratePerDay = 10m) : IPenaltyPolicy
{
    public decimal CalculatePenalty(Rental rental, DateTime returnDate)
    {
        ArgumentNullException.ThrowIfNull(rental);
        return rental.DaysLate(returnDate) * ratePerDay;
    }
}
