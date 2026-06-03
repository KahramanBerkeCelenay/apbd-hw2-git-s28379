using APBD_TASK2.Models;

namespace APBD_TASK2.Repositories;

/// <summary>
/// Storage boundary for equipment. Pure data access (no business rules) so that
/// the in-memory store used here can be replaced by a real database without any
/// change to the services that depend on this interface.
/// </summary>
public interface IEquipmentRepository
{
    void Add(Equipment equipment);
    Equipment? GetById(int id);
    IReadOnlyList<Equipment> GetAll();
}
