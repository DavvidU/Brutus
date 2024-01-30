using Brutus.Data;
using Brutus.Models;
//budowanie obiektu konto dla admina
public class AdminBuilder : UserBuilder
{
    private Konto konto = new Konto();
    private ApplicationUser applicationUser = new ApplicationUser();
    private readonly BrutusContext _context;

    public AdminBuilder(BrutusContext context)
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
        string modifiedEmail = $"admin.{email}";
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
        // przypisanie id uzytkownika z applicationuser do konta
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

        // stworzenie obiektu admin i przypisanie mu konta, nastepnie zapis w bazie danych
        Admin admin = new Admin
        {
            ID_Admina = konto.ID_Konta,
            Konto = konto
        };
        _context.Admini.Add(admin);
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
