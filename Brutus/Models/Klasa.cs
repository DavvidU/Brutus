using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Klasa
    {
        [Key]
        public int ID_Klasy { get; set; }
        public int NumerRoku { get; set; }
        [MaxLength(1)]
        public string LiteraKlasy { get; set; }

        [ForeignKey("ID_Wychowawcy")]
        public virtual Wychowawca? Wychowawca { get; set; }
    }
}
