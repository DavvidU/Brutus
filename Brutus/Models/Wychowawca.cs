using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brutus.Models
{
    public class Wychowawca
    {
        [Key]
        public int ID_Wychowawcy { get; set; }

        [ForeignKey("ID_Wychowawcy")]
        public virtual Nauczyciel? Nauczyciel { get; set; }
    }
}
