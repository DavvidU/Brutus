using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Wiadomosc
    {
        [Key]
        public int ID_Wiadomosci { get; set; }

        [ForeignKey("ID_Nadawcy")]
        public virtual Konto? Nadawca { get; set; }

        public DateTime Data { get; set; }

        [MaxLength(500)]
        public string Tresc { get; set; }
    }
}
