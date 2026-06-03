using APBD_TASK2.Enums;

namespace APBD_TASK2.Exceptions;

/// <summary>Thrown when an equipment status transition is not allowed from the current state.</summary>
public sealed class InvalidEquipmentStateException(int equipmentId, string name, EquipmentStatus status, string action)
    : RentalException($"Cannot {action} equipment [#{equipmentId}] '{name}' while it is {status}.");
