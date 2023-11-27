using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Przedmiot
    {
        [Key]
        public int ID_Przedmiotu { get; set; }
        [MaxLength(20)]
        public string Nazwa { get; set; }
        [MaxLength(500)]
        public string Opis { get; set; }
    }
}
