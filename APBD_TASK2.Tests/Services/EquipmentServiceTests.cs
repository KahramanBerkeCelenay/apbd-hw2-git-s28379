using APBD_TASK2.Enums;
using APBD_TASK2.Exceptions;
using APBD_TASK2.Models;

namespace APBD_TASK2.Tests.Services;

public class EquipmentServiceTests
{
    [Fact]
    public void Register_AddsItemToCatalogue()
    {
        var sut = TestSystem.Build().Equipment;

        var laptop = sut.Register(new Laptop("Dell", 16, 15.6));

        Assert.Single(sut.GetAll());
        Assert.Equal(laptop.Id, sut.GetById(laptop.Id).Id);
    }

    [Fact]
    public void GetById_UnknownId_Throws()
    {
        var sut = TestSystem.Build().Equipment;

        Assert.Throws<EquipmentNotFoundException>(() => sut.GetById(999));
    }

    [Fact]
    public void GetAvailable_ExcludesRentedAndUnavailable()
    {
        var sut = TestSystem.Build().Equipment;
        var available = sut.Register(new Laptop("Dell", 16, 15.6));
        var rented = sut.Register(new Laptop("Mac", 8, 13.3));
        var broken = sut.Register(new Camera("Sony", 24, 3));

        rented.MarkAsRented();
        sut.MarkUnavailable(broken.Id);

        var result = sut.GetAvailable();

        Assert.Single(result);
        Assert.Equal(available.Id, result[0].Id);
    }

    [Fact]
    public void MarkUnavailable_ChangesStatus()
    {
        var sut = TestSystem.Build().Equipment;
        var camera = sut.Register(new Camera("Sony", 24, 3));

        sut.MarkUnavailable(camera.Id);

        Assert.Equal(EquipmentStatus.Unavailable, camera.Status);
    }

    [Fact]
    public void MarkUnavailable_WhileRented_Throws()
    {
        var sut = TestSystem.Build().Equipment;
        var laptop = sut.Register(new Laptop("Dell", 16, 15.6));
        laptop.MarkAsRented();

        Assert.Throws<InvalidEquipmentStateException>(() => sut.MarkUnavailable(laptop.Id));
    }
}
