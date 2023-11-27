using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Brutus.Models;

namespace Brutus.Data
{
    public class BrutusContext : DbContext
    {
        public DbSet<Konto> Konta { get; set; }
        public DbSet<Rodzic> Rodzice { get; set; }
        public DbSet<Nauczyciel> Nauczyciele { get; set; }
        public DbSet<Wychowawca> Wychowawcy { get; set; }
        public DbSet<Klasa> Klasy { get; set; }
        public DbSet<Uczen> Uczniowie { get; set; }
        public DbSet<Przedmiot> Przedmioty { get; set; }
        public DbSet<Zalacznik> Zalaczniki { get; set; }
        public DbSet<KlasaPrzedmiot> KlasyPrzedmioty { get; set; }
        public DbSet<Admin> Admini { get; set; }
        public DbSet<Ocena> Oceny { get; set; }
        public DbSet<Test> Testy { get; set; }
        public DbSet<Pytanie> Pytania { get; set; }
        public DbSet<Ogloszenie> Ogloszenia { get; set; }
        public DbSet<Wiadomosc> Wiadomosci { get; set; }
        public DbSet<WiadomoscZalacznik> WiadomosciZalaczniki { get; set; }
        public DbSet<UczenTest> UczniowieTesty { get; set; }
        public DbSet<KontoWiadomosc> KontaWiadomosci { get; set; }
        public DbSet<UczenOcena> UczniowieOceny { get; set; }
        public DbSet<PrzedmiotZalacznik> PrzedmiotyZalaczniki { get; set; }
        public DbSet<NauczycielPrzedmiot> NauczycielePrzedmioty { get; set; }
        public BrutusContext (DbContextOptions<BrutusContext> options)
            : base(options)
        {
        }
    }
}
