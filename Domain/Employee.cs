namespace APBD_Cw1_s32103;

public class Employee : User
{
    public override int MaxActiveRentals => 5;

    public Employee(string firstName, string lastName) : base(firstName, lastName) { }
}
