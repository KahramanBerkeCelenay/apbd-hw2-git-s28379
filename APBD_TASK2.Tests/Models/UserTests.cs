using APBD_TASK2.Enums;
using APBD_TASK2.Models;

namespace APBD_TASK2.Tests.Models;

public class UserTests
{
    [Fact]
    public void Student_HasLimitOfTwo_AndStudentType()
    {
        var student = new Student("Alice", "Nowak");

        Assert.Equal(2, student.MaxActiveRentals);
        Assert.Equal(UserType.Student, student.Type);
    }

    [Fact]
    public void Employee_HasLimitOfFive_AndEmployeeType()
    {
        var employee = new Employee("Eve", "Lewandowska");

        Assert.Equal(5, employee.MaxActiveRentals);
        Assert.Equal(UserType.Employee, employee.Type);
    }

    [Fact]
    public void FullName_CombinesFirstAndLastName()
    {
        var student = new Student("Alice", "Nowak");

        Assert.Equal("Alice Nowak", student.FullName);
    }

    [Theory]
    [InlineData("", "Nowak")]
    [InlineData("Alice", "  ")]
    public void Constructor_BlankName_Throws(string first, string last)
    {
        Assert.Throws<ArgumentException>(() => new Student(first, last));
    }

    [Fact]
    public void UserIds_AreUnique()
    {
        var a = new Student("A", "A");
        var b = new Employee("B", "B");

        Assert.NotEqual(a.Id, b.Id);
    }
}
