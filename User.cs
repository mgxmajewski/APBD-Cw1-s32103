namespace APBD_Cw1_s32103;

public abstract class User
{
    public Guid Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public abstract int MaxActiveRentals { get; }

    protected User(string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
    }

    public string FullName => $"{FirstName} {LastName}";
}
