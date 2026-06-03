namespace APBD_TASK2.Models;

/// <summary>
/// A single rental transaction linking a <see cref="User"/> to a piece of
/// <see cref="Equipment"/>. It records who rented what, when, for how long, when it
/// came back and whether that was on time. The transaction is its own class so the
/// equipment and the user stay focused on their own concerns (separation of
/// responsibilities / low coupling).
///
/// All overdue/late calculations are date based (time-of-day is ignored) so the
/// behaviour is deterministic and easy to reason about.
/// </summary>
public sealed class Rental
{
    private static int _nextId = 1;

    public int Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime RentedAt { get; }
    public DateTime DueAt { get; }
    public DateTime? ReturnedAt { get; private set; }

    /// <summary>The penalty charged on return. Zero until the item is returned.</summary>
    public decimal Penalty { get; private set; }

    public Rental(User user, Equipment equipment, DateTime rentedAt, int durationDays)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(equipment);
        if (durationDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationDays), "Rental duration must be at least one day.");

        Id = _nextId++;
        User = user;
        Equipment = equipment;
        RentedAt = rentedAt;
        DueAt = rentedAt.AddDays(durationDays);
    }

    public bool IsActive => ReturnedAt is null;

    /// <summary>True when the rental is still out and the due date has already passed.</summary>
    public bool IsOverdue(DateTime asOf) => IsActive && asOf.Date > DueAt.Date;

    /// <summary>Number of whole days a return on <paramref name="returnDate"/> is late (never negative).</summary>
    public int DaysLate(DateTime returnDate)
    {
        int late = (returnDate.Date - DueAt.Date).Days;
        return late > 0 ? late : 0;
    }

    public bool WasReturnedOnTime => ReturnedAt is not null && ReturnedAt.Value.Date <= DueAt.Date;

    /// <summary>Closes the rental, recording the return moment and the penalty that was charged.</summary>
    public void CompleteReturn(DateTime returnedAt, decimal penalty)
    {
        if (!IsActive)
            throw new InvalidOperationException($"Rental #{Id} has already been returned.");
        if (penalty < 0)
            throw new ArgumentOutOfRangeException(nameof(penalty), "Penalty cannot be negative.");

        ReturnedAt = returnedAt;
        Penalty = penalty;
    }
}
