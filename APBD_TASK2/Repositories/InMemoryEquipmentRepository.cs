using APBD_TASK2.Models;

namespace APBD_TASK2.Repositories;

/// <summary>
/// In-memory <see cref="IEquipmentRepository"/>. The backing list is supplied
/// through the constructor, which keeps the repository decoupled from any
/// particular store: production wires in the <c>Singleton</c> lists, while tests
/// pass a throwaway list so each test starts from a clean slate.
/// </summary>
public sealed class InMemoryEquipmentRepository(List<Equipment> store) : IEquipmentRepository
{
    public void Add(Equipment equipment) => store.Add(equipment);

    public Equipment? GetById(int id) => store.FirstOrDefault(e => e.Id == id);

    public IReadOnlyList<Equipment> GetAll() => store.AsReadOnly();
}
