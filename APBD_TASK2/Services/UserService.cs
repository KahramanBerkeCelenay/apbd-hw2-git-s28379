using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;
using APBD_TASK2.Repositories;

namespace APBD_TASK2.Services;

/// <summary>Owns user-related use cases, depending only on <see cref="IUserRepository"/>.</summary>
public sealed class UserService(IUserRepository userRepository) : IUserService
{
    public User Register(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        userRepository.Add(user);
        return user;
    }

    public User GetById(int id) =>
        userRepository.GetById(id) ?? throw new UserNotFoundException(id);

    public IReadOnlyList<User> GetAll() => userRepository.GetAll();
}
