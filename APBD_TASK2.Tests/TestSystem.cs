using APBD_TASK2.Models;
using APBD_TASK2.Policies;
using APBD_TASK2.Repositories;
using APBD_TASK2.Services;

namespace APBD_TASK2.Tests;

/// <summary>
/// Builds a fresh service graph backed by throwaway in-memory lists. Because the
/// repositories take their backing list through the constructor, every test gets
/// a clean, isolated system with no shared state and no dependence on the global
/// Singleton.
/// </summary>
internal static class TestSystem
{
    public static Fixture Build(decimal ratePerDay = 10m)
    {
        var equipmentRepo = new InMemoryEquipmentRepository(new List<Equipment>());
        var userRepo = new InMemoryUserRepository(new List<User>());
        var rentalRepo = new InMemoryRentalRepository(new List<Rental>());

        IPenaltyPolicy penalty = new LatePenaltyPolicy(ratePerDay);

        var equipment = new EquipmentService(equipmentRepo);
        var users = new UserService(userRepo);
        var rentals = new RentalService(rentalRepo, penalty);
        var reports = new ReportService(equipment, users, rentals);

        return new Fixture(equipment, users, rentals, reports);
    }

    internal sealed record Fixture(
        IEquipmentService Equipment,
        IUserService Users,
        IRentalService Rentals,
        IReportService Reports);
}
