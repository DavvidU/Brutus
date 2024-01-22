﻿using Brutus.Data;
using Brutus.Models;
//budowanie obiektu konto dla rodzica
public class RodzicBuilder : UserBuilder
{
    private Konto konto = new Konto();
    private ApplicationUser applicationUser = new ApplicationUser();
    private readonly BrutusContext _context;

    public RodzicBuilder(BrutusContext context)
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
        string modifiedEmail = $"rodzic.{email}";
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
    //zapis do bazy
    public void Save(ApplicationUser user, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
    {
        
        konto.ApplicationUserId = user.Id;
        _context.Konta.Add(konto);
        _context.SaveChanges();
        // stworzenie obiektu rodzic i przypisanie mu konta, nastepnie zapis w bazie danych
        Rodzic rodzic = new Rodzic
        {
            ID_Rodzica = konto.ID_Konta,
            Konto = konto
        };
        _context.Rodzice.Add(rodzic);
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