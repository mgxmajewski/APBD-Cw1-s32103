namespace APBD_Cw1_s32103;

public class EquipmentService
{
    private readonly EquipmentRepository _repo;

    public EquipmentService(EquipmentRepository repo)
    {
        _repo = repo;
    }

    public void DodajSprzet(Equipment sprzet) => _repo.Add(sprzet);

    public IReadOnlyList<Equipment> PobierzWszystkie() => _repo.GetAll();

    public IEnumerable<Equipment> PobierzDostepne() =>
        FiltrujPoStatusie(AvailabilityStatus.Available);

    public IEnumerable<Equipment> FiltrujPoStatusie(AvailabilityStatus status) =>
        _repo.GetAll().Where(e => e.AvailabilityStatus == status);

    public Equipment OznaczJakoNiedostepny(Guid id)
    {
        var sprzet = _repo.GetById(id)
            ?? throw new InvalidOperationException($"Nie znaleziono sprzętu o ID {id}.");

        if (sprzet.AvailabilityStatus == AvailabilityStatus.Rented)
            throw new SprzętWypożyczonyException(sprzet.Name);

        if (sprzet.AvailabilityStatus == AvailabilityStatus.Unavailable)
            throw new SprzętJużNiedostępnyException(sprzet.Name);

        sprzet.AvailabilityStatus = AvailabilityStatus.Unavailable;
        return sprzet;
    }
}
