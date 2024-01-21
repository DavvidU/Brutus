using Brutus.Data;
using Brutus.Models;

public class RodzicBuilder : UserBuilder
{
    private Konto konto = new Konto();
    private ApplicationUser applicationUser = new ApplicationUser();
    private readonly BrutusContext _context;

    public RodzicBuilder(BrutusContext context)
    {
        _context = context;
    }

    public UserBuilder SetImie(string imie)
    {
        konto.Imie = imie;
        return this;
    }

    public UserBuilder SetNazwisko(string nazwisko)
    {
        konto.Nazwisko = nazwisko;
        return this;
    }

    public UserBuilder SetEmail(string email)
    {
        string modifiedEmail = $"rodzic.{email}";
        konto.Email = modifiedEmail;
        applicationUser.Email = modifiedEmail;
        applicationUser.UserName = modifiedEmail;
        return this;
    }

    public UserBuilder SetSkrotHasla(string skrotHasla)
    {
        konto.SkrotHasla = skrotHasla;
        return this;
    }

    public UserBuilder SetNrTelefonu(int nrTelefonu)
    {
        konto.NrTelefonu = nrTelefonu;
        applicationUser.PhoneNumber = nrTelefonu.ToString();
        return this;
    }
    public void Save(ApplicationUser user, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
    {
        
        konto.ApplicationUserId = user.Id;
        _context.Konta.Add(konto);
        _context.SaveChanges();

        Rodzic rodzic = new Rodzic
        {
            ID_Rodzica = konto.ID_Konta,
            Konto = konto
        };
        _context.Rodzice.Add(rodzic);
        _context.SaveChanges();
    }

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