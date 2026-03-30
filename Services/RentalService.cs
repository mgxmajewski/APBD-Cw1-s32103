namespace APBD_Cw1_s32103;

public class RentalService
{
    private readonly RentalRepository _rentalRepo;

    public RentalService(RentalRepository rentalRepo)
    {
        _rentalRepo = rentalRepo;
    }

    public Rental Wypozycz(User uzytkownik, Equipment sprzet, int liczbaDni, DateTime? dataWypozyczenia = null)
    {
        if (sprzet.AvailabilityStatus != AvailabilityStatus.Available)
            throw new InvalidOperationException(
                $"Sprzęt '{sprzet.Name}' nie jest dostępny do wypożyczenia (status: {sprzet.AvailabilityStatus}).");

        int aktywne = _rentalRepo.GetActiveByUser(uzytkownik).Count();
        if (aktywne >= uzytkownik.MaxActiveRentals)
            throw new InvalidOperationException(
                $"{uzytkownik.FullName} osiągnął limit {uzytkownik.MaxActiveRentals} aktywnych wypożyczeń.");

        var wypozyczenie = new Rental(uzytkownik, sprzet, dataWypozyczenia ?? DateTime.Now, liczbaDni);
        sprzet.AvailabilityStatus = AvailabilityStatus.Rented;

        _rentalRepo.Add(wypozyczenie);
        return wypozyczenie;
    }

    public Rental Zwroc(Guid idWypozyczenia, DateTime? dataZwrotu = null)
    {
        var wypozyczenie = _rentalRepo.GetById(idWypozyczenia)
            ?? throw new InvalidOperationException($"Nie znaleziono wypożyczenia o ID {idWypozyczenia}.");

        if (!wypozyczenie.IsActive)
            throw new InvalidOperationException("To wypożyczenie zostało już wcześniej zwrócone.");

        wypozyczenie.Return(dataZwrotu ?? DateTime.Now);
        wypozyczenie.Equipment.AvailabilityStatus = AvailabilityStatus.Available;

        return wypozyczenie;
    }

    public IEnumerable<Rental> PobierzAktywneWypozyczenia(User uzytkownik) =>
        _rentalRepo.GetActiveByUser(uzytkownik);

    public IEnumerable<Rental> PobierzPrzeterminowane() =>
        _rentalRepo.GetOverdue();

    public IEnumerable<Rental> PobierzWszystkie() =>
        _rentalRepo.GetAll();
}
