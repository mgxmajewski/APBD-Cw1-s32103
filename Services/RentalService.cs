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
        if (liczbaDni <= 0)
            throw new NieprawidłowaLiczbaDniException(liczbaDni);

        if (sprzet.AvailabilityStatus != AvailabilityStatus.Available)
            throw new SprzętNiedostępnyException(sprzet.Name, sprzet.AvailabilityStatus);

        int aktywne = _rentalRepo.GetActiveByUser(uzytkownik).Count();
        if (aktywne >= uzytkownik.MaxActiveRentals)
            throw new LimitWypożyczeńPrzekroczonyException(uzytkownik.FullName, uzytkownik.MaxActiveRentals);

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
            throw new WypożyczenieJużZwróconeException();

        var dataZwrotuFinal = dataZwrotu ?? DateTime.Now;
        if (dataZwrotuFinal < wypozyczenie.RentedAt)
            throw new NieprawidłowaDataZwrotuException(dataZwrotuFinal, wypozyczenie.RentedAt);

        wypozyczenie.Return(dataZwrotuFinal);
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
