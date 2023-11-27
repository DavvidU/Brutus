using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Uczen
    {
        [Key]
        public int ID_Ucznia { get; set; }

        [ForeignKey("ID_Ucznia")]
        public virtual Konto? Konto { get; set; }

        [ForeignKey("ID_Rodzica")]
        public virtual Rodzic? Rodzic { get; set; }

        [ForeignKey("ID_Klasy")]
        public virtual Klasa? Klasa { get; set; }
    }
}
