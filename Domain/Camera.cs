namespace APBD_Cw1_s32103;

public class Camera : Equipment
{
    public int MegaPixels { get; set; }
    public bool HasOpticalZoom { get; set; }

    public Camera(string name, string description, string make, int megaPixels, bool hasOpticalZoom)
        : base(name, description, make)
    {
        MegaPixels = megaPixels;
        HasOpticalZoom = hasOpticalZoom;
    }
}
