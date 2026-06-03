using APBD_TASK2.Models;

namespace APBD_TASK2.Repositories;

/// <summary>In-memory <see cref="IUserRepository"/> backed by a constructor-supplied list.</summary>
public sealed class InMemoryUserRepository(List<User> store) : IUserRepository
{
    public void Add(User user) => store.Add(user);

    public User? GetById(int id) => store.FirstOrDefault(u => u.Id == id);

    public IReadOnlyList<User> GetAll() => store.AsReadOnly();
}
