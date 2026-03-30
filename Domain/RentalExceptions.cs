namespace APBD_Cw1_s32103;

public class SprzętNiedostępnyException : InvalidOperationException
{
    public SprzętNiedostępnyException(string nazwaSprzetu, AvailabilityStatus status)
        : base($"Sprzęt '{nazwaSprzetu}' nie jest dostępny do wypożyczenia (status: {status}).") { }
}

public class LimitWypożyczeńPrzekroczonyException : InvalidOperationException
{
    public LimitWypożyczeńPrzekroczonyException(string nazwaUzytkownika, int limit)
        : base($"{nazwaUzytkownika} osiągnął limit {limit} aktywnych wypożyczeń.") { }
}

public class SprzętJużNiedostępnyException : InvalidOperationException
{
    public SprzętJużNiedostępnyException(string nazwaSprzetu)
        : base($"'{nazwaSprzetu}' jest już oznaczony jako niedostępny.") { }
}

public class SprzętWypożyczonyException : InvalidOperationException
{
    public SprzętWypożyczonyException(string nazwaSprzetu)
        : base($"Nie można oznaczyć '{nazwaSprzetu}' jako niedostępny — sprzęt jest aktualnie wypożyczony.") { }
}

public class WypożyczenieJużZwróconeException : InvalidOperationException
{
    public WypożyczenieJużZwróconeException()
        : base("To wypożyczenie zostało już wcześniej zwrócone.") { }
}

public class NieprawidłowaDataZwrotuException : InvalidOperationException
{
    public NieprawidłowaDataZwrotuException(DateTime dataZwrotu, DateTime dataWypozyczenia)
        : base($"Data zwrotu ({dataZwrotu:dd.MM.yyyy}) nie może być wcześniejsza niż data wypożyczenia ({dataWypozyczenia:dd.MM.yyyy}).") { }
}

public class NieprawidłowaLiczbaDniException : ArgumentException
{
    public NieprawidłowaLiczbaDniException(int dni)
        : base($"Liczba dni wypożyczenia musi być większa od zera (podano: {dni}).", "rentalDays") { }
}
