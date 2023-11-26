using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class NauczycielPrzedmiot
    {
        public int Id { get; set; }
        [ForeignKey("ID_Nauczyciela")]
        public virtual Nauczyciel? Nauczyciel { get; set; }

        [ForeignKey("ID_Przedmiotu")]
        public virtual Przedmiot? Przedmiot { get; set; }
    }
}
