using Brutus.Data;
using Brutus.Models;
//budowanie obiektu konto dla nauczyciela
public class NauczycielBuilder : UserBuilder
{
    private Konto konto = new Konto();
    private ApplicationUser applicationUser = new ApplicationUser();
    private readonly BrutusContext _context;

    public NauczycielBuilder(BrutusContext context)
    {
        _context = context;
    }
    //przypisuje imie do budowanego obiektu konto
    public UserBuilder SetImie(string imie)
    {
        konto.Imie = imie;
        return this;
    }
    //przypisuje nazwisko do budowanego obiektu konto
    public UserBuilder SetNazwisko(string nazwisko)
    {
        konto.Nazwisko = nazwisko;
        return this;
    }
    //przypisuje email do budowanego obiektu konto
    public UserBuilder SetEmail(string email)
    {
        string modifiedEmail = $"nauczyciel.{email}";
        konto.Email = modifiedEmail;
        applicationUser.Email = modifiedEmail;
        applicationUser.UserName = modifiedEmail;
        return this;
    }
    //przypisuje skrot hasla do budowanego obiektu konto
    public UserBuilder SetSkrotHasla(string skrotHasla)
    {
        konto.SkrotHasla = skrotHasla;
        return this;
    }
    //przypisuje nr tel do budowanego obiektu konto
    public UserBuilder SetNrTelefonu(int nrTelefonu)
    {
        konto.NrTelefonu = nrTelefonu;
        applicationUser.PhoneNumber = nrTelefonu.ToString();
        return this;
    }
    //zapis do bazy danych
    public void Save(ApplicationUser user, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
    {

        konto.ApplicationUserId = user.Id;
        _context.Konta.Add(konto);
        _context.SaveChanges();
        // stworzenie obiektu nauczyciel i przypisanie mu konta, nastepnie zapis w bazie danych
        Nauczyciel nauczyciel = new Nauczyciel
        {
            ID_Nauczyciela = konto.ID_Konta,
            Konto = konto
        };
        _context.Nauczyciele.Add(nauczyciel);
        _context.SaveChanges();
    }
    //build tworzy i zwraca nowy obiekt applicationuser
    public ApplicationUser Build()
    {
        return new ApplicationUser
        {
            UserName = konto.Email,
            Email = konto.Email,
            PhoneNumber = konto.NrTelefonu.ToString()
        };
    }
}