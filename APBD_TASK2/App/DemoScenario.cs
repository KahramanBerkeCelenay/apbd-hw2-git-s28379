using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;
using APBD_TASK2.Services;

namespace APBD_TASK2.App;

/// <summary>
/// The required end-to-end demonstration. It is kept out of <c>Program.cs</c> so
/// the entry point stays a thin composition root. This class only orchestrates the
/// services and prints the results — it holds no business rules of its own.
///
/// Fixed dates are used so the demo is deterministic: the "rental" happens on
/// 2026-06-01 and the report is produced "as of" 2026-06-15.
/// </summary>
public sealed class DemoScenario(
    IEquipmentService equipmentService,
    IUserService userService,
    IRentalService rentalService,
    IReportService reportService)
{
    private static readonly DateTime RentDate = new(2026, 6, 1);
    private static readonly DateTime Today = new(2026, 6, 15);

    public void Run()
    {
        Console.WriteLine("=== UNIVERSITY EQUIPMENT RENTAL SYSTEM ===");

        // 1. Register equipment of different types.
        Section("1. Register equipment");
        var laptopDell = equipmentService.Register(new Laptop("Dell XPS 15", ramGb: 16, screenSizeInches: 15.6));
        var laptopMac = equipmentService.Register(new Laptop("MacBook Air", ramGb: 8, screenSizeInches: 13.3));
        var projector = equipmentService.Register(new Projector("Epson EB-X05", brand: "Epson", lumenBrightness: 3300));
        var camera = equipmentService.Register(new Camera("Sony A7 III", megapixels: 24, opticalZoom: 3));
        Console.WriteLine($"Registered {equipmentService.GetAll().Count} items.");

        // 2. Register users of different types.
        Section("2. Register users");
        var alice = userService.Register(new Student("Alice", "Nowak"));
        var bob = userService.Register(new Student("Bob", "Kowalski"));
        var eve = userService.Register(new Employee("Eve", "Lewandowska"));
        Console.WriteLine($"Registered {userService.GetAll().Count} users.");

        // 3. Display the full equipment list with status.
        Section("3. All equipment");
        PrintEquipment(equipmentService.GetAll());

        // 4. A correct rental operation.
        Section("4. Rent equipment (valid)");
        var aliceLaptop = rentalService.Rent(alice, laptopDell, RentDate, durationDays: 7);
        Console.WriteLine($"OK: {alice.FullName} rented '{laptopDell.Name}' (due {aliceLaptop.DueAt:yyyy-MM-dd}).");
        rentalService.Rent(alice, projector, RentDate, durationDays: 7);
        Console.WriteLine($"OK: {alice.FullName} rented '{projector.Name}'.");

        // 5. Invalid operation: renting unavailable equipment.
        Section("5. Rent unavailable equipment (blocked)");
        equipmentService.MarkUnavailable(camera.Id);
        Console.WriteLine($"'{camera.Name}' marked Unavailable (maintenance).");
        TryRent(bob, camera);

        // 6. Invalid operation: exceeding the rental limit.
        Section("6. Exceed rental limit (blocked)");
        Console.WriteLine($"{alice.FullName} already holds {rentalService.GetActiveRentals(alice).Count} of {alice.MaxActiveRentals} allowed rentals.");
        TryRent(alice, laptopMac);

        // 7. Display only equipment currently available.
        Section("7. Available equipment only");
        PrintEquipment(equipmentService.GetAvailable());

        // 8. Display active rentals for a selected user.
        Section($"8. Active rentals for {alice.FullName}");
        PrintRentals(rentalService.GetActiveRentals(alice));

        // 9. A return completed on time.
        Section("9. Return on time (no penalty)");
        decimal onTimePenalty = rentalService.Return(projector, new DateTime(2026, 6, 6));
        Console.WriteLine($"Returned '{projector.Name}'. Penalty: {onTimePenalty} PLN.");

        // 10. A delayed return that leads to a penalty.
        Section("10. Late return (penalty charged)");
        var eveLaptop = rentalService.Rent(eve, laptopMac, RentDate, durationDays: 3);
        Console.WriteLine($"{eve.FullName} rented '{laptopMac.Name}' (due {eveLaptop.DueAt:yyyy-MM-dd}).");
        decimal latePenalty = rentalService.Return(laptopMac, new DateTime(2026, 6, 10));
        Console.WriteLine($"Returned '{laptopMac.Name}' late. Penalty: {latePenalty} PLN.");

        // 11. Display the list of overdue rentals.
        Section($"11. Overdue rentals (as of {Today:yyyy-MM-dd})");
        PrintRentals(rentalService.GetOverdueRentals(Today));

        // 12. A short summary report of the rental service state.
        Section("12. Summary report");
        Console.WriteLine(reportService.BuildSummary(Today));
    }

    /// <summary>Attempts a rental and reports the outcome, turning a domain failure into a clear message.</summary>
    private void TryRent(User user, Equipment equipment)
    {
        try
        {
            rentalService.Rent(user, equipment, RentDate, durationDays: 3);
            Console.WriteLine($"OK: {user.FullName} rented '{equipment.Name}'.");
        }
        catch (RentalException ex)
        {
            Console.WriteLine($"BLOCKED: {ex.Message}");
        }
    }

    private static void PrintEquipment(IReadOnlyList<Equipment> items)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("  (none)");
            return;
        }

        foreach (var e in items)
            Console.WriteLine($"  {e.GetDescription()}");
    }

    private static void PrintRentals(IReadOnlyList<Rental> rentals)
    {
        if (rentals.Count == 0)
        {
            Console.WriteLine("  (none)");
            return;
        }

        foreach (var r in rentals)
            Console.WriteLine($"  Rental #{r.Id}: {r.User.FullName} -> '{r.Equipment.Name}' (due {r.DueAt:yyyy-MM-dd}{(r.IsActive ? ", active" : "")})");
    }

    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"--- {title} ---");
    }
}
