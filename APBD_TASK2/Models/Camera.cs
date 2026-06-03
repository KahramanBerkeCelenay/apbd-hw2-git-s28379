namespace APBD_TASK2.Models;

/// <summary>A camera. Adds two camera-specific properties on top of <see cref="Equipment"/>.</summary>
public sealed class Camera : Equipment
{
    public int Megapixels { get; }
    public int OpticalZoom { get; }

    public Camera(string name, int megapixels, int opticalZoom) : base(name)
    {
        if (megapixels <= 0)
            throw new ArgumentOutOfRangeException(nameof(megapixels), "Megapixels must be positive.");
        if (opticalZoom <= 0)
            throw new ArgumentOutOfRangeException(nameof(opticalZoom), "Optical zoom must be positive.");

        Megapixels = megapixels;
        OpticalZoom = opticalZoom;
    }

    public override string GetDescription() =>
        $"{base.GetDescription()} | {Megapixels} MP, {OpticalZoom}x optical zoom";
}
