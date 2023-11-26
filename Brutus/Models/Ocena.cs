using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Ocena
    {
        [Key]
        public int ID_Oceny { get; set; }
        public int Waga { get; set; }
        [MaxLength(100)]
        public string Komentarz { get; set; }

        [ForeignKey("ID_Ucznia")]
        public virtual Uczen? Uczen { get; set; }

        [ForeignKey("ID_Nauczyciela")]
        public virtual Wychowawca? Nauczyciel { get; set; }

        [ForeignKey("ID_Przedmiotu")]
        public virtual Przedmiot? Przedmiot { get; set; }

        public DateTime Data { get; set; }
        public int Wartosc { get; set; }
    }
}
