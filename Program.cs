using APBD_Cw1_s32103;

// ── Konfiguracja zależności ───────────────────────────────────
var repoSprzetu     = new EquipmentRepository();
var repoUzytk       = new UserRepository();
var repoWypozyczen  = new RentalRepository();

var serwisSprzetu    = new EquipmentService(repoSprzetu);
var serwisUzytk      = new UserService(repoUzytk);
var serwisWypozyczen = new RentalService(repoWypozyczen);
var serwisRaportow   = new ReportService(repoSprzetu, repoWypozyczen, repoUzytk);

// ── 1. Dodanie sprzętu ────────────────────────────────────────
ConsoleUI.PrintHeader("1. Rejestracja sprzętu");

var laptop1   = new Laptop("Lenovo ThinkPad E14", "Laptop służbowy", "Lenovo", "Windows 11 Pro", 16);
var laptop2   = new Laptop("Dell Latitude 5540", "Laptop studencki", "Dell", "Ubuntu 22.04", 8);
var laptop3   = new Laptop("HP EliteBook 840", "Laptop zapasowy", "HP", "Windows 11 Home", 8);
var projektor = new Projector("BenQ MX560", "Projektor sali 204", "BenQ", 1024, 4000);
var projektor2 = new Projector("Epson EB-X51", "Projektor sali 105", "Epson", 1280, 3600);
var aparat    = new Camera("Sony Alpha 6400", "Aparat bezlusterkowy", "Sony", 24, true);

serwisSprzetu.DodajSprzet(laptop1);
serwisSprzetu.DodajSprzet(laptop2);
serwisSprzetu.DodajSprzet(laptop3);
serwisSprzetu.DodajSprzet(projektor);
serwisSprzetu.DodajSprzet(projektor2);
serwisSprzetu.DodajSprzet(aparat);

ConsoleUI.PrintOk("Dodano: Lenovo ThinkPad E14 (Laptop)");
ConsoleUI.PrintOk("Dodano: Dell Latitude 5540 (Laptop)");
ConsoleUI.PrintOk("Dodano: HP EliteBook 840 (Laptop)");
ConsoleUI.PrintOk("Dodano: BenQ MX560 (Projektor)");
ConsoleUI.PrintOk("Dodano: Epson EB-X51 (Projektor)");
ConsoleUI.PrintOk("Dodano: Sony Alpha 6400 (Aparat)");

// ── 2. Dodanie użytkowników ───────────────────────────────────
ConsoleUI.PrintHeader("2. Rejestracja użytkowników");

var student1  = new Student("Tomasz", "Brylski");
var student2  = new Student("Kacper", "Zając");
var pracownik = new Employee("Agnieszka", "Dąbrowska");

serwisUzytk.DodajUzytkownika(student1);
serwisUzytk.DodajUzytkownika(student2);
serwisUzytk.DodajUzytkownika(pracownik);

ConsoleUI.PrintOk($"Student:   {student1.FullName}  (limit: {student1.MaxActiveRentals} wypożyczenia)");
ConsoleUI.PrintOk($"Student:   {student2.FullName}    (limit: {student2.MaxActiveRentals} wypożyczenia)");
ConsoleUI.PrintOk($"Pracownik: {pracownik.FullName} (limit: {pracownik.MaxActiveRentals} wypożyczeń)");

// ── 3. Cały sprzęt z aktualnym statusem ──────────────────────
ConsoleUI.PrintHeader("3. Stan całego sprzętu");
ConsoleUI.PrintListaSprzetu(serwisSprzetu.PobierzWszystkie());

// ── 4. Poprawne wypożyczenia ──────────────────────────────────
ConsoleUI.PrintHeader("4. Wypożyczenia (poprawne)");

var wyp1 = serwisWypozyczen.Wypozycz(student1, laptop1, 7);
ConsoleUI.PrintOk($"{student1.FullName} wypożyczył '{laptop1.Name}' na 7 dni. Termin: {wyp1.DueDate:dd.MM.yyyy}");

var wyp2 = serwisWypozyczen.Wypozycz(student1, projektor, 3);
ConsoleUI.PrintOk($"{student1.FullName} wypożyczył '{projektor.Name}' na 3 dni. Termin: {wyp2.DueDate:dd.MM.yyyy}");

var wyp3 = serwisWypozyczen.Wypozycz(pracownik, aparat, 14);
ConsoleUI.PrintOk($"{pracownik.FullName} wypożyczyła '{aparat.Name}' na 14 dni. Termin: {wyp3.DueDate:dd.MM.yyyy}");

// ── 5. Blokowane operacje ─────────────────────────────────────
ConsoleUI.PrintHeader("5. Próby nieprawidłowych operacji");

// 5a. Student osiągnął limit (2 aktywne wypożyczenia)
try
{
    serwisWypozyczen.Wypozycz(student1, laptop2, 5);
}
catch (LimitWypożyczeńPrzekroczonyException ex)
{
    ConsoleUI.PrintBlad($"Przekroczony limit: {ex.Message}");
}

// 5b. Oznaczenie laptop2 jako niedostępny, próba wypożyczenia
serwisSprzetu.OznaczJakoNiedostepny(laptop2.Id);
ConsoleUI.PrintInfo($"'{laptop2.Name}' oznaczony jako niedostępny (serwis).");

try
{
    serwisWypozyczen.Wypozycz(student2, laptop2, 3);
}
catch (SprzętNiedostępnyException ex)
{
    ConsoleUI.PrintBlad($"Niedostępny sprzęt: {ex.Message}");
}

// 5c. Próba oznaczenia już niedostępnego sprzętu
try
{
    serwisSprzetu.OznaczJakoNiedostepny(laptop2.Id);
}
catch (SprzętJużNiedostępnyException ex)
{
    ConsoleUI.PrintBlad($"Duplikat statusu: {ex.Message}");
}

// 5d. Nieprawidłowa liczba dni wypożyczenia
try
{
    serwisWypozyczen.Wypozycz(student2, projektor, -1);
}
catch (NieprawidłowaLiczbaDniException ex)
{
    ConsoleUI.PrintBlad($"Błędne dane: {ex.Message}");
}

// 5e. Data zwrotu wcześniejsza niż data wypożyczenia
try
{
    serwisWypozyczen.Zwroc(wyp2.Id, wyp2.RentedAt.AddDays(-3));
}
catch (NieprawidłowaDataZwrotuException ex)
{
    ConsoleUI.PrintBlad($"Błędna data: {ex.Message}");
}

// ── 6. Sprzęt dostępny do wypożyczenia ───────────────────────
ConsoleUI.PrintHeader("6. Dostępny sprzęt");
ConsoleUI.PrintListaSprzetu(serwisSprzetu.PobierzDostepne());

// ── 7. Zwrot w terminie ───────────────────────────────────────
ConsoleUI.PrintHeader("7. Zwrot w terminie");

var zwrot1 = serwisWypozyczen.Zwroc(wyp3.Id, wyp3.DueDate.AddDays(-2));
ConsoleUI.PrintOk($"'{zwrot1.Equipment.Name}' zwrócony przed terminem. Kara: {zwrot1.Penalty:C}");

// ── 8. Zwrot z opóźnieniem ────────────────────────────────────
ConsoleUI.PrintHeader("8. Zwrot z opóźnieniem");

var zwrot2 = serwisWypozyczen.Zwroc(wyp1.Id, wyp1.DueDate.AddDays(5));
ConsoleUI.PrintOk($"'{zwrot2.Equipment.Name}' zwrócony 5 dni po terminie. Kara: {zwrot2.Penalty:C}");

// ── 9. Aktywne wypożyczenia studenta ─────────────────────────
ConsoleUI.PrintHeader($"9. Aktywne wypożyczenia — {student1.FullName}");
var aktywne = serwisWypozyczen.PobierzAktywneWypozyczenia(student1).ToList();
if (aktywne.Any())
    ConsoleUI.PrintListaWypozyczen(aktywne);
else
    ConsoleUI.PrintInfo("Brak aktywnych wypożyczeń.");

// ── 10. Przeterminowane wypożyczenia ─────────────────────────
ConsoleUI.PrintHeader("10. Przeterminowane wypożyczenia");
var przeterminowane = serwisWypozyczen.PobierzPrzeterminowane().ToList();
if (przeterminowane.Any())
    ConsoleUI.PrintListaWypozyczen(przeterminowane);
else
    ConsoleUI.PrintInfo("Brak przeterminowanych wypożyczeń.");

// ── 11. Filtrowane widoki ─────────────────────────────────────
ConsoleUI.PrintHeader("11. Filtr: sprzęt niedostępny (serwis)");
var niedostepne = serwisSprzetu.FiltrujPoStatusie(AvailabilityStatus.Unavailable).ToList();
if (niedostepne.Any())
    ConsoleUI.PrintListaSprzetu(niedostepne);
else
    ConsoleUI.PrintInfo("Brak sprzętu o tym statusie.");

ConsoleUI.PrintHeader("11b. Filtr: sprzęt aktualnie wypożyczony");
var wypozyczony = serwisSprzetu.FiltrujPoStatusie(AvailabilityStatus.Rented).ToList();
if (wypozyczony.Any())
    ConsoleUI.PrintListaSprzetu(wypozyczony);
else
    ConsoleUI.PrintInfo("Brak sprzętu o tym statusie.");

ConsoleUI.PrintHeader("11c. Filtr: przeterminowane wypożyczenia");
var przeterminowane2 = serwisWypozyczen.PobierzPrzeterminowane().ToList();
if (przeterminowane2.Any())
    ConsoleUI.PrintListaWypozyczen(przeterminowane2);
else
    ConsoleUI.PrintInfo("Brak przeterminowanych wypożyczeń.");

// ── 12. Raport końcowy ────────────────────────────────────────
var raport = serwisRaportow.GenerujRaport();
ConsoleUI.PrintRaport(raport);