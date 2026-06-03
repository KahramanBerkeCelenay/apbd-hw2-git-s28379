using APBD_TASK2.Models;

namespace APBD_TASK2.Services;

/// <summary>Use cases that deal with the equipment catalogue and its availability.</summary>
public interface IEquipmentService
{
    Equipment Register(Equipment equipment);
    Equipment GetById(int id);
    IReadOnlyList<Equipment> GetAll();
    IReadOnlyList<Equipment> GetAvailable();
    void MarkUnavailable(int id);
    void MarkAvailable(int id);
}
