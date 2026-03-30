namespace APBD_Cw1_s32103;

public class Laptop : Equipment
{
    public string OperatingSystem { get; set; }
    public int RamGb { get; set; }

    public Laptop(string name, string description, string make, string operatingSystem, int ramGb)
        : base(name, description, make)
    {
        OperatingSystem = operatingSystem;
        RamGb = ramGb;
    }
}
