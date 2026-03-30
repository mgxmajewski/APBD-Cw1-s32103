# Uczelniana Wypożyczalnia Sprzętu

Aplikacja konsolowa w C# symulująca system wypożyczalni sprzętu na uczelni. Obsługuje rejestrację sprzętu i użytkowników, wypożyczenia, zwroty z naliczaniem kar oraz generowanie raportów.

## Uruchomienie

Wymagane: .NET 8 SDK lub nowszy.

```bash
dotnet run
```

## Struktura projektu

```
Domain/
  Equipment.cs, Laptop.cs, Projector.cs, Camera.cs   — hierarchia sprzętu
  AvailabilityStatus.cs                               — enum statusu sprzętu
  User.cs, Student.cs, Employee.cs                    — hierarchia użytkowników
  RentalRules.cs                                      — wszystkie stałe biznesowe
  Rental.cs                                           — model wypożyczenia

Repositories/
  EquipmentRepository.cs
  UserRepository.cs
  RentalRepository.cs                                 — proste repozytoria in-memory

Services/
  RentalService.cs       — logika wypożyczeń i zwrotów
  EquipmentService.cs    — zarządzanie sprzętem
  UserService.cs         — zarządzanie użytkownikami
  ReportService.cs       — generowanie raportu

UI/
  ConsoleUI.cs           — wyświetlanie, wszystkie komunikaty po polsku

Program.cs               — scenariusz demonstracyjny
```

## Decyzje projektowe

### Kohezja
Każda klasa ma jedną odpowiedzialność. `RentalService` zajmuje się wyłącznie logiką wypożyczeń. `ReportService` tylko agreguje statystyki. `ConsoleUI` tylko formatuje wyjście — żaden serwis nie wypisuje nic na konsolę samodzielnie.

### Reguły biznesowe w jednym miejscu
`RentalRules` to jedna statyczna klasa z trzema stałymi: `StudentMaxRentals`, `EmployeeMaxRentals`, `PenaltyPerDay`. Zmiana limitu lub stawki kary wymaga edycji jednego wiersza w jednym pliku.

### Słabe sprzężenie
Serwisy zależą od repozytoriów przez konstruktor (prosta wstrzyknięcie zależności). `RentalService` nie zna `UserService` ani `EquipmentService` — operuje na przekazanych obiektach domenowych.

### Dziedziczenie uzasadnione modelem domeny
`Equipment → Laptop / Projector / Camera` — wspólne pola (Id, Name, Make, Status) w bazie, każdy podtyp ma własne pola specyficzne. Analogicznie `User → Student / Employee` z nadpisaną właściwością `MaxActiveRentals`, co pozwala serwisowi sprawdzać limit bez rzutowania ani sprawdzania typów.

### Kompozycja zamiast dziedziczenia — gdzie to ma sens
Tam gdzie relacja nie jest „jest czymś", lecz „ma coś", stosujemy kompozycję:

- **`Rental` komponuje `User` i `Equipment`** — wypożyczenie nie dziedziczy po użytkowniku ani sprzęcie, lecz przechowuje referencje do obu obiektów. Dzięki temu `Rental` może operować na ich danych (np. `rental.Equipment.AvailabilityStatus`) bez silnego sprzężenia przez dziedziczenie.
- **Serwisy komponują repozytoria** — `RentalService` przyjmuje `RentalRepository` i `EquipmentRepository` przez konstruktor. `ReportService` komponuje wszystkie trzy repozytoria. Żaden serwis nie rozszerza innego — zależności są jawne i łatwe do podmiany.

### Obsługa błędów
Nieprawidłowe operacje (sprzęt niedostępny, przekroczony limit, próba ponownego zwrotu) rzucają `InvalidOperationException` z opisowym komunikatem po polsku. Wywołujący (`Program.cs` lub przyszłe UI) sam decyduje, jak obsłużyć błąd.

## Reguły biznesowe

| Reguła | Wartość |
|--------|---------|
| Maks. aktywnych wypożyczeń — student | 2 |
| Maks. aktywnych wypożyczeń — pracownik | 5 |
| Kara za opóźnienie | 15,00 PLN / dobę |
