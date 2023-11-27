using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Pytanie
    {
        [Key]
        public int ID_Pytania { get; set; }

        [ForeignKey("ID_Testu")]
        public virtual Test? Test { get; set; }

        [MaxLength(200)]
        public string Tresc { get; set; }
        public int Punkty { get; set; }

        [MaxLength(30)]
        public string WariantA { get; set; }

        [MaxLength(30)]
        public string WariantB { get; set; }

        [MaxLength(30)]
        public string WariantC { get; set; }

        [MaxLength(30)]
        public string WariantD { get; set; }

        [MaxLength(1)]
        public string PoprawnaOdpowiedz { get; set; }
    }
}
