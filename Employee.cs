namespace APBD_Cw1_s32103;

public class Employee : User
{
    public override int MaxActiveRentals => RentalRules.EmployeeMaxRentals;

    public Employee(string firstName, string lastName) : base(firstName, lastName) { }
}
