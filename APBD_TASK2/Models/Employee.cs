using APBD_TASK2.Enums;

namespace APBD_TASK2.Models;

/// <summary>An employee. May hold at most 5 active rentals at a time.</summary>
public sealed class Employee : User
{
    public Employee(string firstName, string lastName) : base(firstName, lastName) { }

    public override UserType Type => UserType.Employee;
    public override int MaxActiveRentals => 5;
}
