using Brutus.Models;
using Microsoft.AspNet.Identity;
//okresla metody ktore powinien implementowac kazdy builder uzytkownika
public interface UserBuilder
{
    UserBuilder SetImie(string imie);
    UserBuilder SetNazwisko(string nazwisko);
    UserBuilder SetEmail(string email);
    UserBuilder SetSkrotHasla(string skrotHasla);
    UserBuilder SetNrTelefonu(int nrTelefonu);
    void Save(ApplicationUser user, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager);
    ApplicationUser Build();
    
}