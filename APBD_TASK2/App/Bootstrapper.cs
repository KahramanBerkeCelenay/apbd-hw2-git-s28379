using APBD_TASK2.Database;
using APBD_TASK2.Policies;
using APBD_TASK2.Repositories;
using APBD_TASK2.Services;

namespace APBD_TASK2.App;

/// <summary>
/// The composition root: the one place that is allowed to create concrete types
/// and wire them together. Every class above it receives its dependencies through
/// its constructor, so the rest of the code depends only on abstractions.
/// </summary>
public static class Bootstrapper
{
    public static DemoScenario BuildDemo()
    {
        var db = Singleton.Instance;

        // Data access (concrete stores chosen here, used everywhere else via interfaces).
        IEquipmentRepository equipmentRepository = new InMemoryEquipmentRepository(db.Equipments);
        IUserRepository userRepository = new InMemoryUserRepository(db.Users);
        IRentalRepository rentalRepository = new InMemoryRentalRepository(db.Rentals);

        // Business policy that is most likely to change lives in one swappable place.
        IPenaltyPolicy penaltyPolicy = new LatePenaltyPolicy(ratePerDay: 10m);

        // Services depend on abstractions only.
        IEquipmentService equipmentService = new EquipmentService(equipmentRepository);
        IUserService userService = new UserService(userRepository);
        IRentalService rentalService = new RentalService(rentalRepository, penaltyPolicy);
        IReportService reportService = new ReportService(equipmentService, userService, rentalService);

        return new DemoScenario(equipmentService, userService, rentalService, reportService);
    }
}
