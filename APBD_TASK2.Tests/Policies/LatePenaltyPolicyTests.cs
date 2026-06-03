using APBD_TASK2.Models;
using APBD_TASK2.Policies;

namespace APBD_TASK2.Tests.Policies;

public class LatePenaltyPolicyTests
{
    private static readonly DateTime Start = new(2026, 6, 1);

    private static Rental Rental(int durationDays = 7) =>
        new(new Student("Alice", "Nowak"), new Laptop("Dell", 16, 15.6), Start, durationDays);

    [Fact]
    public void OnTimeReturn_HasNoPenalty()
    {
        var policy = new LatePenaltyPolicy(ratePerDay: 10m);
        var rental = Rental(durationDays: 7); // due 2026-06-08

        Assert.Equal(0m, policy.CalculatePenalty(rental, new DateTime(2026, 6, 8)));
    }

    [Fact]
    public void LateReturn_ChargesRatePerDay()
    {
        var policy = new LatePenaltyPolicy(ratePerDay: 10m);
        var rental = Rental(durationDays: 7); // due 2026-06-08

        Assert.Equal(60m, policy.CalculatePenalty(rental, new DateTime(2026, 6, 14)));
    }

    [Fact]
    public void DailyRate_IsConfigurable()
    {
        var policy = new LatePenaltyPolicy(ratePerDay: 25m);
        var rental = Rental(durationDays: 7); // due 2026-06-08, 2 days late

        Assert.Equal(50m, policy.CalculatePenalty(rental, new DateTime(2026, 6, 10)));
    }
}
