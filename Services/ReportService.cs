namespace APBD_Cw1_s32103;

public class ReportService
{
    private readonly EquipmentRepository _equipmentRepo;
    private readonly RentalRepository _rentalRepo;
    private readonly UserRepository _userRepo;

    public ReportService(EquipmentRepository equipmentRepo, RentalRepository rentalRepo, UserRepository userRepo)
    {
        _equipmentRepo = equipmentRepo;
        _rentalRepo = rentalRepo;
        _userRepo = userRepo;
    }

    public RaportSystemu GenerujRaport()
    {
        var sprzety = _equipmentRepo.GetAll();
        var wypozyczenia = _rentalRepo.GetAll();
        var uzytkownicy = _userRepo.GetAll();

        return new RaportSystemu
        {
            LiczbaUzytkownikow = uzytkownicy.Count,
            LacznieSprzetu = sprzety.Count,
            Dostepnych = sprzety.Count(s => s.AvailabilityStatus == AvailabilityStatus.Available),
            Wypozyczonych = sprzety.Count(s => s.AvailabilityStatus == AvailabilityStatus.Rented),
            Niedostepnych = sprzety.Count(s => s.AvailabilityStatus == AvailabilityStatus.Unavailable),
            AktywneWypozyczenia = wypozyczenia.Count(w => w.IsActive),
            PrzeterminowaneWypozyczenia = wypozyczenia.Count(w => w.IsOverdue),
            LaczneKary = wypozyczenia.Where(w => !w.IsActive).Sum(w => w.Penalty)
        };
    }
}

public class RaportSystemu
{
    public int LiczbaUzytkownikow { get; init; }
    public int LacznieSprzetu { get; init; }
    public int Dostepnych { get; init; }
    public int Wypozyczonych { get; init; }
    public int Niedostepnych { get; init; }
    public int AktywneWypozyczenia { get; init; }
    public int PrzeterminowaneWypozyczenia { get; init; }
    public decimal LaczneKary { get; init; }
}
