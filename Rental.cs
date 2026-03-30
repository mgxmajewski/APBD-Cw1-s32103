namespace APBD_Cw1_s32103;

public class Rental
{
    public Guid Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime RentedAt { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }
    public decimal Penalty { get; private set; }

    public bool IsActive => ReturnedAt == null;
    public bool IsOverdue => IsActive && DateTime.Now > DueDate;

    public Rental(User user, Equipment equipment, DateTime rentedAt, int rentalDays)
    {
        Id = Guid.NewGuid();
        User = user;
        Equipment = equipment;
        RentedAt = rentedAt;
        DueDate = rentedAt.AddDays(rentalDays);
    }

    public void Return(DateTime returnedAt)
    {
        ReturnedAt = returnedAt;

        if (returnedAt > DueDate)
        {
            int daysLate = (int)Math.Ceiling((returnedAt - DueDate).TotalDays);
            Penalty = daysLate * RentalRules.PenaltyPerDay;
        }
    }
}
