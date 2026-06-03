using APBD_TASK2.Models;

namespace APBD_TASK2.Tests.Models;

public class RentalTests
{
    private static readonly DateTime Start = new(2026, 6, 1);

    private static Rental NewRental(int durationDays = 7) =>
        new(new Student("Alice", "Nowak"), new Laptop("Dell", 16, 15.6), Start, durationDays);

    [Fact]
    public void Constructor_SetsDueDate_FromDuration()
    {
        var rental = NewRental(durationDays: 7);

        Assert.Equal(Start.AddDays(7), rental.DueAt);
        Assert.True(rental.IsActive);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Constructor_NonPositiveDuration_Throws(int duration)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NewRental(duration));
    }

    [Fact]
    public void IsOverdue_IsFalse_BeforeDueDate()
    {
        var rental = NewRental(durationDays: 7); // due 2026-06-08

        Assert.False(rental.IsOverdue(new DateTime(2026, 6, 5)));
    }

    [Fact]
    public void IsOverdue_IsTrue_AfterDueDate_WhileActive()
    {
        var rental = NewRental(durationDays: 7); // due 2026-06-08

        Assert.True(rental.IsOverdue(new DateTime(2026, 6, 9)));
    }

    [Fact]
    public void IsOverdue_IsFalse_OnceReturned()
    {
        var rental = NewRental(durationDays: 7);
        rental.CompleteReturn(new DateTime(2026, 6, 20), penalty: 0);

        Assert.False(rental.IsOverdue(new DateTime(2026, 6, 25)));
    }

    [Fact]
    public void DaysLate_IsZero_WhenReturnedOnDueDate()
    {
        var rental = NewRental(durationDays: 7); // due 2026-06-08

        Assert.Equal(0, rental.DaysLate(new DateTime(2026, 6, 8)));
    }

    [Fact]
    public void DaysLate_CountsWholeDaysPastDue()
    {
        var rental = NewRental(durationDays: 7); // due 2026-06-08

        Assert.Equal(6, rental.DaysLate(new DateTime(2026, 6, 14)));
    }

    [Fact]
    public void WasReturnedOnTime_TrueWhenWithinDue()
    {
        var rental = NewRental(durationDays: 7);
        rental.CompleteReturn(new DateTime(2026, 6, 6), penalty: 0);

        Assert.True(rental.WasReturnedOnTime);
    }

    [Fact]
    public void CompleteReturn_Twice_Throws()
    {
        var rental = NewRental();
        rental.CompleteReturn(new DateTime(2026, 6, 6), penalty: 0);

        Assert.Throws<InvalidOperationException>(() => rental.CompleteReturn(new DateTime(2026, 6, 7), 0));
    }

    [Fact]
    public void CompleteReturn_NegativePenalty_Throws()
    {
        var rental = NewRental();

        Assert.Throws<ArgumentOutOfRangeException>(() => rental.CompleteReturn(new DateTime(2026, 6, 6), penalty: -1));
    }
}
