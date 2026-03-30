namespace APBD_Cw1_s32103;

public class Projector : Equipment
{
    public int ResolutionWidth { get; set; }
    public int Lumens { get; set; }

    public Projector(string name, string description, string make, int resolutionWidth, int lumens)
        : base(name, description, make)
    {
        ResolutionWidth = resolutionWidth;
        Lumens = lumens;
    }
}