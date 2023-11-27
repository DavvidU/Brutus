using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Konto
    {
        [Key]
        public int ID_Konta { get; set; }
        [MaxLength(20)]
        public string Imie { get; set; }
        [MaxLength(30)]
        public string Nazwisko { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(10)]
        public string SkrotHasla { get; set; }
        public int NrTelefonu { get; set; }
    }
}
