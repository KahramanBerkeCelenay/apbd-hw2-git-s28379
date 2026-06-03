using APBD_TASK2.Models;

namespace APBD_TASK2.Policies;

/// <summary>
/// Decides the penalty for a returned rental. Pulling this rule behind an
/// interface means the "how much do we charge for a late return?" decision lives
/// in exactly one place and can be changed (or swapped for a different policy)
/// without editing the rental service.
/// </summary>
public interface IPenaltyPolicy
{
    decimal CalculatePenalty(Rental rental, DateTime returnDate);
}
