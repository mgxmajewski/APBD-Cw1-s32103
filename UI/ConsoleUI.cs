namespace APBD_Cw1_s32103;

public class ConsoleUI
{
    public static void PrintHeader(string tytul)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"  {tytul}");
        Console.WriteLine(new string('=', 60));
    }

    public static void PrintSekcja(string tytul)
    {
        Console.WriteLine();
        Console.WriteLine($"--- {tytul} ---");
    }

    public static void PrintOk(string wiadomosc) =>
        Console.WriteLine($"[OK]  {wiadomosc}");

    public static void PrintBlad(string wiadomosc) =>
        Console.WriteLine($"[BŁĄD] {wiadomosc}");

    public static void PrintInfo(string wiadomosc) =>
        Console.WriteLine($"      {wiadomosc}");

    public static void PrintListaSprzetu(IEnumerable<Equipment> sprzety)
    {
        foreach (var s in sprzety)
        {
            string szczegoly = s switch
            {
                Laptop l    => $"System: {l.OperatingSystem}, RAM: {l.RamGb} GB",
                Projector p => $"Rozdzielczość: {p.ResolutionWidth}px, Jasność: {p.Lumens} lm",
                Camera c    => $"Megapiksele: {c.MegaPixels} MP, Zoom optyczny: {(c.HasOpticalZoom ? "tak" : "nie")}",
                _           => string.Empty
            };
            string status = s.AvailabilityStatus switch
            {
                AvailabilityStatus.Available    => "Dostępny",
                AvailabilityStatus.Rented       => "Wypożyczony",
                AvailabilityStatus.Unavailable  => "Niedostępny",
                _                               => s.AvailabilityStatus.ToString()
            };
            Console.WriteLine($"  [{status,-13}] {s.Name,-28} ({s.Make})  {szczegoly}");
        }
    }

    public static void PrintListaWypozyczen(IEnumerable<Rental> wypozyczenia)
    {
        foreach (var w in wypozyczenia)
        {
            string status = w.IsActive
                ? (w.IsOverdue ? "PRZETERMINOWANE" : "aktywne")
                : $"zwrócone {w.ReturnedAt:yyyy-MM-dd}";

            string kara = w.Penalty > 0 ? $"  Kara: {w.Penalty:C}" : string.Empty;

            Console.WriteLine($"  {w.User.FullName,-22} -> {w.Equipment.Name,-26} Termin: {w.DueDate:yyyy-MM-dd}  [{status}]{kara}");
        }
    }

    public static void PrintRaport(RaportSystemu raport)
    {
        PrintHeader("RAPORT STANU WYPOŻYCZALNI");
        Console.WriteLine($"  Użytkownicy             : {raport.LiczbaUzytkownikow}");
        Console.WriteLine($"  Sprzęt łącznie          : {raport.LacznieSprzetu}");
        Console.WriteLine($"    Dostępny              : {raport.Dostepnych}");
        Console.WriteLine($"    Wypożyczony           : {raport.Wypozyczonych}");
        Console.WriteLine($"    Niedostępny           : {raport.Niedostepnych}");
        Console.WriteLine($"  Aktywne wypożyczenia    : {raport.AktywneWypozyczenia}");
        Console.WriteLine($"  Przeterminowane         : {raport.PrzeterminowaneWypozyczenia}");
        Console.WriteLine($"  Naliczone kary łącznie  : {raport.LaczneKary:C}");
        Console.WriteLine(new string('=', 60));
    }
}
