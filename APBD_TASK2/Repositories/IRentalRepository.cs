using APBD_TASK2.Models;

namespace APBD_TASK2.Repositories;

/// <summary>
/// Storage boundary for rentals. Deliberately small: it only stores and returns
/// rentals. Questions like "which rentals are overdue?" are business questions
/// and are answered in the service, not here.
/// </summary>
public interface IRentalRepository
{
    void Add(Rental rental);
    IReadOnlyList<Rental> GetAll();
}
