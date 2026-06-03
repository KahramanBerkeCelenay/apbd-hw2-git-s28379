using APBD_TASK2.Enums;
using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;

namespace APBD_TASK2.Tests.Models;

public class EquipmentTests
{
    [Fact]
    public void NewEquipment_IsAvailable()
    {
        var laptop = new Laptop("Dell", 16, 15.6);

        Assert.Equal(EquipmentStatus.Available, laptop.Status);
        Assert.True(laptop.IsAvailable);
    }

    [Fact]
    public void EquipmentIds_AreUniqueAndGenerated()
    {
        var a = new Laptop("A", 8, 13);
        var b = new Camera("B", 24, 3);

        Assert.NotEqual(a.Id, b.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_BlankName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => new Projector(name, "Epson", 3000));
    }

    [Fact]
    public void Laptop_NonPositiveRam_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Laptop("Dell", 0, 15.6));
    }

    [Fact]
    public void MarkAsRented_FromAvailable_SetsRented()
    {
        var laptop = new Laptop("Dell", 16, 15.6);

        laptop.MarkAsRented();

        Assert.Equal(EquipmentStatus.Rented, laptop.Status);
    }

    [Fact]
    public void MarkAsRented_WhenNotAvailable_Throws()
    {
        var laptop = new Laptop("Dell", 16, 15.6);
        laptop.MarkAsRented();

        Assert.Throws<InvalidEquipmentStateException>(() => laptop.MarkAsRented());
    }

    [Fact]
    public void MarkAsUnavailable_WhenRented_Throws()
    {
        var laptop = new Laptop("Dell", 16, 15.6);
        laptop.MarkAsRented();

        Assert.Throws<InvalidEquipmentStateException>(() => laptop.MarkAsUnavailable());
    }

    [Fact]
    public void MarkAsReturned_FromRented_SetsAvailable()
    {
        var laptop = new Laptop("Dell", 16, 15.6);
        laptop.MarkAsRented();

        laptop.MarkAsReturned();

        Assert.Equal(EquipmentStatus.Available, laptop.Status);
    }

    [Fact]
    public void MarkAsAvailable_FromUnavailable_SetsAvailable()
    {
        var camera = new Camera("Sony", 24, 3);
        camera.MarkAsUnavailable();

        camera.MarkAsAvailable();

        Assert.Equal(EquipmentStatus.Available, camera.Status);
    }

    [Fact]
    public void GetDescription_Laptop_ContainsTypeSpecificFields()
    {
        var laptop = new Laptop("Dell", 16, 15.6);

        string description = laptop.GetDescription();

        Assert.Contains("Dell", description);
        Assert.Contains("16 GB", description);
        Assert.Contains("15.6", description); // invariant formatting, independent of machine locale
    }
}
