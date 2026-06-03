using System.Globalization;

namespace APBD_TASK2.Models;

/// <summary>A laptop. Adds two laptop-specific properties on top of <see cref="Equipment"/>.</summary>
public sealed class Laptop : Equipment
{
    public int RamGb { get; }
    public double ScreenSizeInches { get; }

    public Laptop(string name, int ramGb, double screenSizeInches) : base(name)
    {
        if (ramGb <= 0)
            throw new ArgumentOutOfRangeException(nameof(ramGb), "RAM must be a positive number of gigabytes.");
        if (screenSizeInches <= 0)
            throw new ArgumentOutOfRangeException(nameof(screenSizeInches), "Screen size must be positive.");

        RamGb = ramGb;
        ScreenSizeInches = screenSizeInches;
    }

    public override string GetDescription() =>
        $"{base.GetDescription()} | RAM: {RamGb} GB, Screen: {ScreenSizeInches.ToString(CultureInfo.InvariantCulture)}\"";
}
