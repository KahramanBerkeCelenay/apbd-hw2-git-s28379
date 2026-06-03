namespace APBD_TASK2.Exceptions;

/// <summary>
/// Base type for every expected, domain-level failure in the rental system
/// (broken business rule, missing entity, illegal state change).
///
/// Having one base type means the console/UI layer can catch <see cref="RentalException"/>
/// once and present a friendly message, without being coupled to each concrete case.
/// </summary>
public abstract class RentalException(string message) : Exception(message);
