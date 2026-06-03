using APBD_TASK2.Enums;

namespace APBD_TASK2.Models;

/// <summary>
/// A person who can rent equipment. Abstract because the rental limit depends on
/// the concrete kind of user. Making <see cref="MaxActiveRentals"/> an abstract
/// member (instead of a <c>switch</c> over <see cref="UserType"/>) keeps the design
/// open for extension: a new kind of user is a new subclass, with no existing code
/// to edit (Open/Closed principle).
/// </summary>
public abstract class User
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public string FullName => $"{FirstName} {LastName}";

    /// <summary>The user category, useful for reports and serialization.</summary>
    public abstract UserType Type { get; }

    /// <summary>How many rentals this user may hold at the same time.</summary>
    public abstract int MaxActiveRentals { get; }

    protected User(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name must not be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name must not be empty.", nameof(lastName));

        Id = _nextId++;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }
}
