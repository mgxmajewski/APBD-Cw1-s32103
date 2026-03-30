namespace APBD_Cw1_s32103;

public class EquipmentRepository
{
    private readonly List<Equipment> _items = new();

    public void Add(Equipment equipment) => _items.Add(equipment);

    public IReadOnlyList<Equipment> GetAll() => _items.AsReadOnly();

    public Equipment? GetById(Guid id) => _items.FirstOrDefault(e => e.Id == id);
}
