using APBD_TASK2.Models;

namespace APBD_TASK2.Tests.Services;

public class ReportServiceTests
{
    private static readonly DateTime Start = new(2026, 6, 1);

    [Fact]
    public void Summary_ReflectsSystemState()
    {
        var sys = TestSystem.Build();
        var alice = sys.Users.Register(new Student("Alice", "Nowak"));
        sys.Users.Register(new Employee("Eve", "Lewandowska"));

        var l1 = sys.Equipment.Register(new Laptop("Dell", 16, 15.6));
        var l2 = sys.Equipment.Register(new Laptop("Mac", 8, 13.3));
        var camera = sys.Equipment.Register(new Camera("Sony", 24, 3));
        sys.Equipment.MarkUnavailable(camera.Id);

        sys.Rentals.Rent(alice, l1, Start, durationDays: 3); // due 2026-06-04, left active -> overdue
        sys.Rentals.Rent(alice, l2, Start, durationDays: 3);
        sys.Rentals.Return(l2, new DateTime(2026, 6, 10));    // 6 days late -> 60 PLN

        string summary = sys.Reports.BuildSummary(new DateTime(2026, 6, 15));

        Assert.Contains("Users registered : 2", summary);
        Assert.Contains("available 1, rented 1, unavailable 1", summary);
        Assert.Contains("active 1, returned 1", summary);
        Assert.Contains("Overdue rentals  : 1", summary);
        Assert.Contains("60 PLN", summary);
    }
}
