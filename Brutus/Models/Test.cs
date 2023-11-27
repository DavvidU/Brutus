using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Test
    {
        [Key]
        public int ID_Testu { get; set; }
        public TimeSpan CzasTrwania { get; set; }
        public int LiczbaZadan { get; set; }

        [MaxLength(50)]
        public string Nazwa { get; set; }

        [ForeignKey("ID_Nauczyciela")]
        public virtual Nauczyciel? Nauczyciel { get; set; }
    }
}
