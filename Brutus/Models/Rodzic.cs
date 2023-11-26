using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Rodzic
    {
        [Key]
        public int ID_Rodzica { get; set; }

        [ForeignKey("ID_Rodzica")]
        public virtual Konto Konto { get; set; }
    }

}
