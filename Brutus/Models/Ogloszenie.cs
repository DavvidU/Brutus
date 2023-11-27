using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Ogloszenie
    {
        [Key]
        public int ID_Ogloszenia { get; set; }

        [ForeignKey("ID_Nauczyciela")]
        public virtual Nauczyciel? Nauczyciel { get; set; }

        [MaxLength(500)]
        public string Tresc { get; set; }
    }

}
