namespace Solution.Domain.Equipment;

public class Camera : Equipment
{
    public Camera(string id, string name, int megapixels, string lensType)
        : base(id, name)
    {
        Megapixels = megapixels;
        LensType = lensType;
    }

    public int Megapixels { get; }
    public string LensType { get; }

    public override string ToString()
    {
        return $"{base.ToString()} | Type: Camera | Megapixels: {Megapixels} MP | Lens: {LensType}";
    }
}