namespace APBD_Cw1_s32103;

public class UserRepository
{
    private readonly List<User> _users = new();

    public void Add(User user) => _users.Add(user);

    public IReadOnlyList<User> GetAll() => _users.AsReadOnly();

    public User? GetById(Guid id) => _users.FirstOrDefault(u => u.Id == id);
}
