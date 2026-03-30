namespace APBD_Cw1_s32103;

public class Student : User
{
    public override int MaxActiveRentals => 2;

    public Student(string firstName, string lastName) : base(firstName, lastName) { }
}
