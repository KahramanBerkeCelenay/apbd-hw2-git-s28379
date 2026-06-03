namespace APBD_TASK2.Enums;

/// <summary>
/// The lifecycle state of a single piece of equipment.
/// Kept as an enum so the set of valid states is closed and explicit.
/// </summary>
public enum EquipmentStatus
{
    Available,
    Rented,
    Unavailable
}
