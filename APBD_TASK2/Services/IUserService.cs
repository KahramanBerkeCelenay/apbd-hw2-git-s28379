using APBD_TASK2.Models;

namespace APBD_TASK2.Services;

/// <summary>Use cases that deal with system users.</summary>
public interface IUserService
{
    User Register(User user);
    User GetById(int id);
    IReadOnlyList<User> GetAll();
}
