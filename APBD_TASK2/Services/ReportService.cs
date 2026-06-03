using System.Text;
using APBD_TASK2.Enums;

namespace APBD_TASK2.Services;

/// <summary>
/// Builds the summary report by composing read-only data from the other services.
/// It depends on their interfaces, so it neither knows nor cares how the data is
/// stored. It only *builds* the report text; printing it is the caller's job
/// (keeps presentation out of the service).
/// </summary>
public sealed class ReportService(
    IEquipmentService equipmentService,
    IUserService userService,
    IRentalService rentalService) : IReportService
{
    public string BuildSummary(DateTime asOf)
    {
        var equipment = equipmentService.GetAll();
        var users = userService.GetAll();
        var rentals = rentalService.GetAllRentals();
        var overdue = rentalService.GetOverdueRentals(asOf);

        int available = equipment.Count(e => e.Status == EquipmentStatus.Available);
        int rented = equipment.Count(e => e.Status == EquipmentStatus.Rented);
        int unavailable = equipment.Count(e => e.Status == EquipmentStatus.Unavailable);
        int active = rentals.Count(r => r.IsActive);
        decimal penalties = rentals.Sum(r => r.Penalty);

        var sb = new StringBuilder();
        sb.AppendLine($"Rental service summary (as of {asOf:yyyy-MM-dd})");
        sb.AppendLine($"  Users registered : {users.Count}");
        sb.AppendLine($"  Equipment items  : {equipment.Count} (available {available}, rented {rented}, unavailable {unavailable})");
        sb.AppendLine($"  Rentals total    : {rentals.Count} (active {active}, returned {rentals.Count - active})");
        sb.AppendLine($"  Overdue rentals  : {overdue.Count}");
        sb.Append($"  Penalties charged: {penalties} PLN");
        return sb.ToString();
    }
}
