namespace APBD_Cw1_s32103;

public abstract class Equipment
{
    public Guid Id { get; }
    public string Name { get; set; }
    public AvailabilityStatus AvailabilityStatus { get; set; }
    public string Description { get; set; }
    public string Make { get; set; }

    protected Equipment(string name, string description, string make)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Make = make;
    }
}
