namespace APBD_TASK2.Exceptions;

/// <summary>Thrown when no user exists for a requested id.</summary>
public sealed class UserNotFoundException(int userId)
    : RentalException($"No user found with id #{userId}.");
