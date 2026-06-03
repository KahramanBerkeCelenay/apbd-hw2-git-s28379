namespace APBD_TASK2.Exceptions;

/// <summary>Thrown when a user tries to rent beyond their allowed number of active rentals.</summary>
public sealed class RentalLimitExceededException(string userFullName, int maxActiveRentals)
    : RentalException($"{userFullName} has reached the maximum of {maxActiveRentals} active rental(s).");
