namespace APBD_TASK2.Exceptions;

/// <summary>Thrown when returning equipment that the system does not have out on an active rental.</summary>
public sealed class ActiveRentalNotFoundException(int equipmentId, string name)
    : RentalException($"Equipment [#{equipmentId}] '{name}' has no active rental to return.");
