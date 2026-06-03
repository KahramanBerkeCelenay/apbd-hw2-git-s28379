using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;
using APBD_TASK2.Policies;
using APBD_TASK2.Repositories;

namespace APBD_TASK2.Services;

/// <summary>
/// The heart of the system. It enforces the rental business rules and delegates
/// the two decisions that are most likely to change to collaborators it receives
/// through its constructor:
///   * how many rentals a user may hold  -> the <see cref="User"/> subclass,
///   * how big a late penalty is          -> the injected <see cref="IPenaltyPolicy"/>.
/// It depends only on abstractions, so it is easy to test with fake repositories.
/// </summary>
public sealed class RentalService(IRentalRepository rentalRepository, IPenaltyPolicy penaltyPolicy) : IRentalService
{
    public Rental Rent(User user, Equipment equipment, DateTime rentedAt, int durationDays)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(equipment);

        if (!equipment.IsAvailable)
            throw new EquipmentNotAvailableException(equipment.Id, equipment.Name, equipment.Status);

        int activeCount = rentalRepository.GetAll().Count(r => r.User.Id == user.Id && r.IsActive);
        if (activeCount >= user.MaxActiveRentals)
            throw new RentalLimitExceededException(user.FullName, user.MaxActiveRentals);

        // Build the rental first: its constructor validates the duration, so if that
        // fails the equipment status is left untouched.
        var rental = new Rental(user, equipment, rentedAt, durationDays);
        equipment.MarkAsRented();
        rentalRepository.Add(rental);
        return rental;
    }

    public decimal Return(Equipment equipment, DateTime returnedAt)
    {
        ArgumentNullException.ThrowIfNull(equipment);

        Rental rental = rentalRepository.GetAll()
                            .FirstOrDefault(r => r.Equipment.Id == equipment.Id && r.IsActive)
                        ?? throw new ActiveRentalNotFoundException(equipment.Id, equipment.Name);

        decimal penalty = penaltyPolicy.CalculatePenalty(rental, returnedAt);
        rental.CompleteReturn(returnedAt, penalty);
        equipment.MarkAsReturned();
        return penalty;
    }

    public IReadOnlyList<Rental> GetActiveRentals(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return rentalRepository.GetAll()
            .Where(r => r.User.Id == user.Id && r.IsActive)
            .ToList();
    }

    public IReadOnlyList<Rental> GetOverdueRentals(DateTime asOf) =>
        rentalRepository.GetAll().Where(r => r.IsOverdue(asOf)).ToList();

    public IReadOnlyList<Rental> GetAllRentals() => rentalRepository.GetAll();
}
