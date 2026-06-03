using APBD_TASK2.Enums;
using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;
using APBD_TASK2.Services;

namespace APBD_TASK2.Tests.Services;

public class RentalServiceTests
{
    private static readonly DateTime Start = new(2026, 6, 1);

    private static Laptop RegisterLaptop(IEquipmentService equipment, string name = "Dell") =>
        (Laptop)equipment.Register(new Laptop(name, 16, 15.6));

    [Fact]
    public void Rent_Valid_CreatesActiveRental_AndMarksEquipmentRented()
    {
        var sys = TestSystem.Build();
        var student = sys.Users.Register(new Student("Alice", "Nowak"));
        var laptop = RegisterLaptop(sys.Equipment);

        var rental = sys.Rentals.Rent(student, laptop, Start, durationDays: 7);

        Assert.True(rental.IsActive);
        Assert.Equal(EquipmentStatus.Rented, laptop.Status);
        Assert.Single(sys.Rentals.GetActiveRentals(student));
    }

    [Fact]
    public void Rent_UnavailableEquipment_Throws()
    {
        var sys = TestSystem.Build();
        var student = sys.Users.Register(new Student("Alice", "Nowak"));
        var camera = sys.Equipment.Register(new Camera("Sony", 24, 3));
        sys.Equipment.MarkUnavailable(camera.Id);

        Assert.Throws<EquipmentNotAvailableException>(
            () => sys.Rentals.Rent(student, camera, Start, 3));
    }

    [Fact]
    public void Rent_CannotRentSameItemTwice()
    {
        var sys = TestSystem.Build();
        var alice = sys.Users.Register(new Student("Alice", "Nowak"));
        var bob = sys.Users.Register(new Student("Bob", "Kowalski"));
        var laptop = RegisterLaptop(sys.Equipment);

        sys.Rentals.Rent(alice, laptop, Start, 3);

        Assert.Throws<EquipmentNotAvailableException>(
            () => sys.Rentals.Rent(bob, laptop, Start, 3));
    }

    [Fact]
    public void Rent_StudentBeyondTwoActive_Throws()
    {
        var sys = TestSystem.Build();
        var student = sys.Users.Register(new Student("Alice", "Nowak"));
        var l1 = RegisterLaptop(sys.Equipment, "L1");
        var l2 = RegisterLaptop(sys.Equipment, "L2");
        var l3 = RegisterLaptop(sys.Equipment, "L3");

        sys.Rentals.Rent(student, l1, Start, 3);
        sys.Rentals.Rent(student, l2, Start, 3);

        Assert.Throws<RentalLimitExceededException>(
            () => sys.Rentals.Rent(student, l3, Start, 3));
    }

    [Fact]
    public void Rent_EmployeeAllowsFive_SixthThrows()
    {
        var sys = TestSystem.Build();
        var employee = sys.Users.Register(new Employee("Eve", "Lewandowska"));
        var laptops = Enumerable.Range(1, 6)
            .Select(i => RegisterLaptop(sys.Equipment, $"L{i}"))
            .ToList();

        for (int i = 0; i < 5; i++)
            sys.Rentals.Rent(employee, laptops[i], Start, 3);

        Assert.Equal(5, sys.Rentals.GetActiveRentals(employee).Count);
        Assert.Throws<RentalLimitExceededException>(
            () => sys.Rentals.Rent(employee, laptops[5], Start, 3));
    }

    [Fact]
    public void Return_OnTime_NoPenalty_AndEquipmentBecomesAvailable()
    {
        var sys = TestSystem.Build();
        var student = sys.Users.Register(new Student("Alice", "Nowak"));
        var laptop = RegisterLaptop(sys.Equipment);
        sys.Rentals.Rent(student, laptop, Start, durationDays: 7); // due 2026-06-08

        decimal penalty = sys.Rentals.Return(laptop, new DateTime(2026, 6, 6));

        Assert.Equal(0m, penalty);
        Assert.Equal(EquipmentStatus.Available, laptop.Status);
        Assert.Empty(sys.Rentals.GetActiveRentals(student));
    }

    [Fact]
    public void Return_Late_ChargesPenalty_AndEquipmentBecomesAvailable()
    {
        var sys = TestSystem.Build(ratePerDay: 10m);
        var student = sys.Users.Register(new Student("Alice", "Nowak"));
        var laptop = RegisterLaptop(sys.Equipment);
        sys.Rentals.Rent(student, laptop, Start, durationDays: 3); // due 2026-06-04

        decimal penalty = sys.Rentals.Return(laptop, new DateTime(2026, 6, 10)); // 6 days late

        Assert.Equal(60m, penalty);
        Assert.Equal(EquipmentStatus.Available, laptop.Status);
    }

    [Fact]
    public void Return_EquipmentNotRented_Throws()
    {
        var sys = TestSystem.Build();
        var laptop = RegisterLaptop(sys.Equipment);

        Assert.Throws<ActiveRentalNotFoundException>(
            () => sys.Rentals.Return(laptop, new DateTime(2026, 6, 10)));
    }

    [Fact]
    public void Return_FreesSlot_AllowingAnotherRental()
    {
        var sys = TestSystem.Build();
        var student = sys.Users.Register(new Student("Alice", "Nowak"));
        var l1 = RegisterLaptop(sys.Equipment, "L1");
        var l2 = RegisterLaptop(sys.Equipment, "L2");
        var l3 = RegisterLaptop(sys.Equipment, "L3");
        sys.Rentals.Rent(student, l1, Start, 3);
        sys.Rentals.Rent(student, l2, Start, 3);

        sys.Rentals.Return(l1, new DateTime(2026, 6, 3)); // back under the limit

        var third = sys.Rentals.Rent(student, l3, Start, 3); // now allowed
        Assert.True(third.IsActive);
        Assert.Equal(2, sys.Rentals.GetActiveRentals(student).Count);
    }

    [Fact]
    public void GetActiveRentals_ReturnsOnlyActiveRentalsForThatUser()
    {
        var sys = TestSystem.Build();
        var alice = sys.Users.Register(new Student("Alice", "Nowak"));
        var bob = sys.Users.Register(new Student("Bob", "Kowalski"));
        var l1 = RegisterLaptop(sys.Equipment, "L1");
        var l2 = RegisterLaptop(sys.Equipment, "L2");
        var l3 = RegisterLaptop(sys.Equipment, "L3");

        sys.Rentals.Rent(alice, l1, Start, 3);
        var returned = sys.Rentals.Rent(alice, l2, Start, 3);
        sys.Rentals.Rent(bob, l3, Start, 3);
        sys.Rentals.Return(l2, new DateTime(2026, 6, 3)); // alice's second rental is now closed

        var active = sys.Rentals.GetActiveRentals(alice);

        Assert.Single(active);
        Assert.Equal(l1.Id, active[0].Equipment.Id);
        Assert.DoesNotContain(active, r => r.Id == returned.Id);
    }

    [Fact]
    public void GetOverdueRentals_IncludesActivePastDue_ExcludesReturnedAndNotYetDue()
    {
        var sys = TestSystem.Build();
        var employee = sys.Users.Register(new Employee("Eve", "Lewandowska"));
        var overdueItem = RegisterLaptop(sys.Equipment, "Overdue");
        var futureItem = RegisterLaptop(sys.Equipment, "Future");
        var returnedItem = RegisterLaptop(sys.Equipment, "Returned");

        sys.Rentals.Rent(employee, overdueItem, Start, durationDays: 3);   // due 2026-06-04
        sys.Rentals.Rent(employee, futureItem, Start, durationDays: 30);   // due 2026-07-01
        sys.Rentals.Rent(employee, returnedItem, Start, durationDays: 3);  // due 2026-06-04
        sys.Rentals.Return(returnedItem, new DateTime(2026, 6, 4));        // returned on time

        var overdue = sys.Rentals.GetOverdueRentals(new DateTime(2026, 6, 10));

        Assert.Single(overdue);
        Assert.Equal(overdueItem.Id, overdue[0].Equipment.Id);
    }
}
