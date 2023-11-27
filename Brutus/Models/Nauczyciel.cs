using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Nauczyciel
    {
        [Key]
        public int ID_Nauczyciela { get; set; }

        [ForeignKey("ID_Nauczyciela")]
        public virtual Konto? Konto { get; set; }
    }
}
