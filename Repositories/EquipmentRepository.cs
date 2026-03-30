namespace APBD_Cw1_s32103;

public class EquipmentRepository
{
    private readonly List<Equipment> _equipment = new();

    public void Add(Equipment equipment) => _equipment.Add(equipment);

    public IReadOnlyList<Equipment> GetAll() => _equipment.AsReadOnly();

    public Equipment? GetById(Guid id) => _equipment.FirstOrDefault(e => e.Id == id);
}
