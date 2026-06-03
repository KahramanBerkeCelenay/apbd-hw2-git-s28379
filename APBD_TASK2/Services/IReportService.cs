namespace APBD_TASK2.Services;

/// <summary>
/// Produces a textual summary of the system state. It is a separate service (and a
/// separate interface) from <see cref="IRentalService"/> because reporting is a
/// different responsibility from renting — the class notes' Single Responsibility
/// example splits exactly these two concerns.
/// </summary>
public interface IReportService
{
    string BuildSummary(DateTime asOf);
}
