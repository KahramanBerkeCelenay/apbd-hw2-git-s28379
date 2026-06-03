namespace APBD_TASK2.Exceptions;

/// <summary>Thrown when no equipment exists for a requested id.</summary>
public sealed class EquipmentNotFoundException(int equipmentId)
    : RentalException($"No equipment found with id #{equipmentId}.");
