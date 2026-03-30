namespace APBD_Cw1_s32103;

public class RentalRepository
{
    private readonly List<Rental> _rentals = new();

    public void Add(Rental rental) => _rentals.Add(rental);

    public IReadOnlyList<Rental> GetAll() => _rentals.AsReadOnly();

    public Rental? GetById(Guid id) => _rentals.FirstOrDefault(r => r.Id == id);

    public IEnumerable<Rental> GetActiveByUser(User user) =>
        _rentals.Where(r => r.User.Id == user.Id && r.IsActive);

    public IEnumerable<Rental> GetOverdue() =>
        _rentals.Where(r => r.IsOverdue);
}
