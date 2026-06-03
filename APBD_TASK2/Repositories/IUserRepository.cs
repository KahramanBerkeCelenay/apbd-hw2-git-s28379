using APBD_TASK2.Models;

namespace APBD_TASK2.Repositories;

/// <summary>Storage boundary for users.</summary>
public interface IUserRepository
{
    void Add(User user);
    User? GetById(int id);
    IReadOnlyList<User> GetAll();
}
