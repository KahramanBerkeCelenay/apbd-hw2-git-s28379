using APBD_TASK2.Models;

namespace APBD_TASK2.Repositories;

/// <summary>In-memory <see cref="IRentalRepository"/> backed by a constructor-supplied list.</summary>
public sealed class InMemoryRentalRepository(List<Rental> store) : IRentalRepository
{
    public void Add(Rental rental) => store.Add(rental);

    public IReadOnlyList<Rental> GetAll() => store.AsReadOnly();
}
