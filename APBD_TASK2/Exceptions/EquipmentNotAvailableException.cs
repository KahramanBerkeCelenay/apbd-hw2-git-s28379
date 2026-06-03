using APBD_TASK2.Enums;

namespace APBD_TASK2.Exceptions;

/// <summary>Thrown when an attempt is made to rent equipment that is not available.</summary>
public sealed class EquipmentNotAvailableException(int equipmentId, string name, EquipmentStatus status)
    : RentalException($"Equipment [#{equipmentId}] '{name}' cannot be rented because it is {status}.");
