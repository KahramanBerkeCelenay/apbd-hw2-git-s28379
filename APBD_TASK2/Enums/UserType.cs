namespace APBD_TASK2.Enums;

/// <summary>
/// Discriminator describing what kind of person a user is.
/// The numeric values are fixed so the type survives (de)serialization.
/// Behaviour that depends on the type (e.g. the rental limit) lives on the
/// concrete <see cref="Models.User"/> subclasses, not in a switch over this enum.
/// </summary>
public enum UserType
{
    Student = 1,
    Employee = 2
}
