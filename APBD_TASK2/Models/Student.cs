using APBD_TASK2.Enums;

namespace APBD_TASK2.Models;

/// <summary>A student. May hold at most 2 active rentals at a time.</summary>
public sealed class Student : User
{
    public Student(string firstName, string lastName) : base(firstName, lastName) { }

    public override UserType Type => UserType.Student;
    public override int MaxActiveRentals => 2;
}
