namespace APBD_TASK2.Models;

/// <summary>A projector. Adds two projector-specific properties on top of <see cref="Equipment"/>.</summary>
public sealed class Projector : Equipment
{
    public string Brand { get; }
    public int LumenBrightness { get; }

    public Projector(string name, string brand, int lumenBrightness) : base(name)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Projector brand must not be empty.", nameof(brand));
        if (lumenBrightness <= 0)
            throw new ArgumentOutOfRangeException(nameof(lumenBrightness), "Brightness must be positive.");

        Brand = brand.Trim();
        LumenBrightness = lumenBrightness;
    }

    public override string GetDescription() =>
        $"{base.GetDescription()} | Brand: {Brand}, Brightness: {LumenBrightness} lm";
}
