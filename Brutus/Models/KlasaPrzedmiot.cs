using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class KlasaPrzedmiot
    {
        public int Id { get; set; }
        [ForeignKey("ID_Klasy")]
        public virtual Klasa? Klasa { get; set; }

        [ForeignKey("ID_Przedmiotu")]
        public virtual Przedmiot? Przedmiot { get; set; }
    }
}
