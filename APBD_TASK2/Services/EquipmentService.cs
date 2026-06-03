using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;
using APBD_TASK2.Repositories;

namespace APBD_TASK2.Services;

/// <summary>
/// Owns equipment-related use cases. It depends only on the
/// <see cref="IEquipmentRepository"/> abstraction (injected through the
/// constructor), never on a concrete store.
/// </summary>
public sealed class EquipmentService(IEquipmentRepository equipmentRepository) : IEquipmentService
{
    public Equipment Register(Equipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment);
        equipmentRepository.Add(equipment);
        return equipment;
    }

    public Equipment GetById(int id) =>
        equipmentRepository.GetById(id) ?? throw new EquipmentNotFoundException(id);

    public IReadOnlyList<Equipment> GetAll() => equipmentRepository.GetAll();

    public IReadOnlyList<Equipment> GetAvailable() =>
        equipmentRepository.GetAll().Where(e => e.IsAvailable).ToList();

    public void MarkUnavailable(int id) => GetById(id).MarkAsUnavailable();

    public void MarkAvailable(int id) => GetById(id).MarkAsAvailable();
}
