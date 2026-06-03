namespace APBD_TASK2.Database;

/// <summary>
/// A tiny in-memory stand-in for a real database. Collections for the domain
/// objects are added once the domain model exists.
/// </summary>
public sealed class Singleton
{
    private static Singleton? _instance;

    public static Singleton Instance => _instance ??= new Singleton();

    private Singleton() { }
}
