using APBD_TASK2.Models;

namespace APBD_TASK2.Database;

/// <summary>
/// A tiny in-memory stand-in for a real database. It only holds the raw lists;
/// it contains no business logic. Services never talk to it directly — they go
/// through repository interfaces — so this concrete store can be swapped for a
/// real database later without touching the business layer.
/// </summary>
public sealed class Singleton
{
    private static Singleton? _instance;

    public static Singleton Instance => _instance ??= new Singleton();

    private Singleton() { }

    public List<Equipment> Equipments { get; } = new();
    public List<User> Users { get; } = new();
    public List<Rental> Rentals { get; } = new();
}
