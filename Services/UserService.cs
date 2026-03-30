namespace APBD_Cw1_s32103;

public class UserService
{
    private readonly UserRepository _repo;

    public UserService(UserRepository repo)
    {
        _repo = repo;
    }

    public void DodajUzytkownika(User uzytkownik) => _repo.Add(uzytkownik);

    public User PobierzUzytkownika(Guid id) =>
        _repo.GetById(id) ?? throw new InvalidOperationException($"Nie znaleziono użytkownika o ID {id}.");

    public IReadOnlyList<User> PobierzWszystkich() => _repo.GetAll();
}
